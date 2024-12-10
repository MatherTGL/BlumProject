using System;
using Cysharp.Threading.Tasks;
using GameAssets.General.Server;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAssets.Meta.Quests
{
    [CreateAssetMenu(fileName = "QuestDataOpenLink", menuName = "Configs/QuestData/Open Link", order = 0)]
    public sealed class OpenQuestsConfig : BaseQuest, IQuest
    {
        [SerializeField, Required, BoxGroup("Quest Settings")]
        private string link;

        bool IQuest.isProgress => isProgress;

        bool IQuest.isDone => isDone;

        bool IQuest.canTake => canTake;


        public void Init(bool isProgress, bool isDone, bool canTake)
        {
            this.isProgress = isProgress;
            this.isDone = isDone;
            this.canTake = canTake;
        }

        public async void StartQuest()
        {
            Debug.Log("1XXX");
            Debug.Log($"quest start {isProgress}");
            if (isProgress == false && isDone == false)
            {
                isProgress = true;
                Debug.Log("isProgress = true");
            }
            Debug.Log("2XXX");
            
#if UNITY_WEBGL
            if (link.Contains("t.me"))
                TelegramManager.webAppService.OpenTelegramLink(link);
            else
                TelegramManager.webAppService.OpenLink(link);
#endif
            
#if UNITY_EDITOR
            Application.OpenURL(link);            
#endif
            Debug.Log("Проверка перехода по ссылке");
            await UniTask.Delay(TimeSpan.FromSeconds(5), DelayType.Realtime);
            Debug.Log("Переход был успешен. Задание выполнено!");
            Complete();
        }
        
        public void Complete()
        {
            Debug.Log("Complete invoke base quest");
            isDone = true;
            isProgress = false;
            Debug.Log("Complete end invoke base quest");
            Debug.Log("Дальше своя реализация");
        }

        void IQuest.TakeReward()
        {
            if (canTake == false)
            {
                canTake = true;
                Debug.Log("Игрок забрал награду!");
            }
        }

        public bool IsCompleteConditions()
        {
            throw new NotImplementedException();
        }
    }
}
