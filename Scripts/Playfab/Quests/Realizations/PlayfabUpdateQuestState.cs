using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameAssets.Meta.Quests;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;

namespace GameAssets.General.Server
{
    public sealed class PlayfabUpdateQuestState
    {
        public enum UpdateType : byte
        {
            Start, Complete, TakeReward
        }
        
        private Dictionary<string, TaskCompletionSource<(UpdateUserDataResult dataResult, bool success)>> _requestQueueQuest = new();


        public async UniTask<bool> TryUpdateAsync(IQuest quest, PlayfabQuestsList questsList, UpdateType updateType)
        {
            bool result;

            if (_requestQueueQuest.ContainsKey(quest.guid) == false)
            {
                Debug.Log($"_requestQueueQuest.Count0: {_requestQueueQuest.Count}");
                await PlayfabController.LoginAsync(); 
                _requestQueueQuest.Add(quest.guid, new TaskCompletionSource<(UpdateUserDataResult, bool)>());
                Debug.Log($"_requestQueueQuest.Count1: {_requestQueueQuest.Count}");
                
                if (updateType == UpdateType.Start)
                    result = TryStartQuest(quest);
                else if (updateType == UpdateType.Complete)
                    result = TryCompleteQuest(quest);
                else
                    result = TryTakeReward(quest);
                
                Debug.Log("Началась запись задания на сервер...");
                if (result)
                {                
                    var gettedQuests = await GetQuestsRequestWithRetries(quest.guid, questsList).FirstOrDefault().ToUniTask();
                    await WriteStartedQuestToServer(quest as BaseQuest, gettedQuests).FirstOrDefault().ToUniTask();
                }
                Debug.Log("Закончилась запись задания на сервер!");
            }

            Debug.Log("The quest is in progress! Try later");
            Debug.Log($"_requestQueueQuest.ContainsKey(quest.guid): {_requestQueueQuest.ContainsKey(quest.guid)}");
            var resultOperation = await _requestQueueQuest[quest.guid].Task;
            result = resultOperation.success;
            Debug.Log($"resultOperation.success: {resultOperation.success} / result: {result}");
            Debug.Log("Операция по обновлению задания завершена!");
            _requestQueueQuest.Remove(quest.guid);
            Debug.Log($"_requestQueueQuest.Count2: {_requestQueueQuest.Count}");
            return result;
        }

        private bool TryTakeReward(IQuest quest)
        {
            if (quest.IsTakedReward())
            {
                Debug.Log($"The quest is already taked reward: {quest.guid}");
                return false;
            }
            
            quest.TakeReward();
            Debug.Log("Состояние задания обновлено!");
            return true;
        }

        private bool TryStartQuest(IQuest quest)
        {
            Debug.Log("StartQuest 0");
            if (quest.IsStarted())
            {
                Debug.Log($"The quest is already in progress: {quest.guid}");
                Debug.Log("StartQuest 1");
                return false;
            }
                
            Debug.Log("StartQuest 2");
            quest.StartQuest();  
            Debug.Log("Состояние задания обновлено!");
            return true;
        }

        private bool TryCompleteQuest(IQuest quest)
        {
            if (quest.IsComplete())
            {
                Debug.Log("The quest is already complete");
                return false;
            }
                    
            quest.Complete();
            Debug.Log("Состояние задания обновлено!");
            return true;
        }
        
        private IObservable<PlayfabQuestsList> GetQuestsRequestWithRetries(string guid, PlayfabQuestsList questsList)
        {
            return Observable.Defer(() => SendGetQuests(guid, questsList)).Retry(3).Delay(TimeSpan.FromSeconds(2)).Do(
                _ => Debug.Log("Request succeeded"),
                ex => Debug.LogError($"Request failed after retries: {ex.Message}")
            );
        }

        private IObservable<PlayfabQuestsList> SendGetQuests(string guid, PlayfabQuestsList questsList)
        {
            return Observable.Create<PlayfabQuestsList>(observer =>
            {
                PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
                    result =>
                    {
                        if (result.Data != null && result.Data.ContainsKey("questsData"))
                        {
                            questsList = JsonConvert.DeserializeObject<PlayfabQuestsList>(result.Data["questsData"].Value);
                            Debug.Log($"questsList: {questsList} / {questsList?.quests} / {questsList?.quests?.Count}");
                        }
                        else
                        {
                            if (_requestQueueQuest[guid].Task.IsCompleted == false)
                                _requestQueueQuest[guid].SetResult((null, false));
                            Debug.Log("No quest data found on server. Operation is cancelled.");
                        }
                        
                        observer.OnNext(questsList);
                        observer.OnCompleted();
                    },
                    error =>
                    {
                        var ex = new Exception(error.ErrorMessage);
                        _requestQueueQuest[guid].SetException(ex);
                        observer.OnError(ex);
                    }
                );
                
                return Disposable.Empty;
            });
        }

        private IObservable<Unit> WriteStartedQuestToServer(BaseQuest baseQuest, PlayfabQuestsList gettedQuests)
        {
            return Observable.Create<Unit>(observer =>
            {
                for (int i = 0; i < gettedQuests.quests.Count; i++)
                {
                    Debug.Log($"ddd: {gettedQuests.quests} / {gettedQuests.quests[i]}");
                }
                
                if (gettedQuests.quests.Any(item => item.guid == baseQuest.guid) == false)
                {
                    Debug.Log(gettedQuests.quests.Contains(baseQuest) == false);
                    gettedQuests.quests.Add(baseQuest);
                    Debug.Log($"Quests loaded from server & QuestData = {gettedQuests.quests.Count}");
                    
                    string json = JsonConvert.SerializeObject(gettedQuests);

                    PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest 
                        {
                            Data = new Dictionary<string, string>
                            {
                                { "questsData", json }
                            }
                        },
                        result =>
                        {
                            if (_requestQueueQuest[baseQuest.guid].Task.IsCompleted == false)
                            {
                                _requestQueueQuest[baseQuest.guid].SetResult((result, true));
                                Debug.Log("Закончилась запись задания на сервер");
                            }
                            observer.OnCompleted();
                        },
                        error =>
                        {
                            var ex = new Exception(error.ErrorMessage);
                            _requestQueueQuest[baseQuest.guid].SetException(ex);
                            observer.OnError(ex);
                        });

                    Debug.Log("Квест записался на сервер и успешно стартовал!");
                }
                else
                {
                    Debug.Log("The quest has already been added to the server!");
                }

                return Disposable.Empty;
            });
        }
    }
}