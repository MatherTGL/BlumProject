using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameAssets.General.Server;
using GameAssets.Meta.Referrals;
using GameAssets.Player.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameAssets.Meta.Quests
{
    public sealed class QuestsModel : IQuestsModel
    {
        private async UniTask<List<IQuest>> LoadQuestsAsync()
        {
            SubToEvents();
            
            var getLocalListAsyncOp = Addressables.LoadAssetsAsync<IQuest>("Quest", null);
            var getServerListAsyncOp =  PlayfabController.LoadAllServerQuestsAsync();

            List<BaseQuest> serverQuests = await getServerListAsyncOp;
            List<IQuest> quests = (List<IQuest>)await getLocalListAsyncOp.Task;

            for (int i = 0; i < quests.Count; i++)
            {
                Debug.Log($"{i} number quest {quests[i]} & state: {quests[i].isProgress} / {quests[i].isDone} / {quests[i].canTake}");
                quests[i].Init(quests[i].typeQuest == BaseQuest.TypeQuest.Collect, false, false);
                Debug.Log($"{i} number quest {quests[i]} & state1: {quests[i].isProgress} / {quests[i].isDone} / {quests[i].canTake}");
            }

            if (serverQuests == null) 
                return GetSortedQuests(quests);
            
            Debug.Log($"serverQuests LOP: {serverQuests.Count}");
            
            foreach (var quest in serverQuests)
            {
                if (quest == null)
                    continue;
            
                if (quests.FirstOrDefault(item => item.guid == quest.guid) is { } localQuest)
                {
                    localQuest.Init(quest.isProgress, quest.isDone, quest.canTake);
                    Debug.Log($"localQuest: {localQuest.isProgress} / {localQuest.canTake} / {localQuest.isDone}");
                }
            }

            return GetSortedQuests(quests);
        }

        private void SubToEvents()
        {
            ReferralsController.Imodel.OnFriendsUpdated += CheckConditions;
            DataContoller.Imodel.UpdatedData += CheckConditions;
        }
        
        private async UniTask<IQuest> LoadOneQuestAsync(string guid)
            => await Addressables.LoadAssetAsync<IQuest>(guid).Task;

        private List<IQuest> GetSortedQuests(List<IQuest> quests)
        {
            quests = quests.OrderByDescending(quest => quest.IsComplete()).ToList();
            return quests;
        }

        async void IQuestsModel.StartQuest(string guid)
        {
            var questConfig = await Addressables.LoadAssetAsync<IQuest>(guid).Task;

            if (await PlayfabController.TryStartQuestAsync(questConfig))
                Debug.Log("X1 Quest started");
            
            Addressables.Release(questConfig);
        }

        async UniTask<bool> IQuestsModel.TryTakeRewardAsync(string guid)
        {
            var config = await LoadOneQuestAsync(guid);
            var result = await PlayfabController.TryCompleteQuestAsync(config);

            if (result)
                await PlayfabController.TryCompleteQuestAsync(config);
            
            Debug.Log($"config is done: {config.isDone}");

            if (config.IsComplete() && config.canTake)
                config.TakeReward();

            Addressables.Release(config);
            return config.IsTakedReward();
        }

        async UniTask<List<IQuest>> IQuestsModel.GetAllQuestsAsync()
            => await LoadQuestsAsync();

        private async void CheckConditions(BaseQuest.TypeQuest questType, BaseQuest.JobSubtype jobSubtype)
        {
            if (questType == BaseQuest.TypeQuest.OpenLink)
                return;
            
            var questConfigs = await Addressables.LoadAssetsAsync<IQuest>("Quest", null).Task;
            var config = questConfigs.Where(quest => quest.typeQuest == questType && quest.jobSubtype == jobSubtype).FirstOrDefault();
            Debug.Log($"config is done: {config}");

            if (config == null)
            {
                Debug.LogException(new Exception("No quest config found"));
                Addressables.Release(questConfigs);
            }
            
            if (config.IsCompleteConditions())
            {
                Debug.Log("Условия проверены и они соответствуют заданию. Можно обновлять view и выполнять задание");
            }
            else
            {
                Debug.Log("Условия пока не соответствуют выполнению задания!");
            }
        }
    }
}
