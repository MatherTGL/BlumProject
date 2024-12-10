using GameAssets.Player.Data;
using UnityEngine;

namespace GameAssets.Meta.Quests
{
    [CreateAssetMenu(fileName = "QuestDataCollectMoney", menuName = "Configs/QuestData/Collect/Money", order = 0)]
    public sealed class CollectMoneyQuest : CollectQuestsConfig, IQuest
    {
        bool IQuest.isProgress => isProgress;

        bool IQuest.isDone => isDone;

        bool IQuest.canTake => canTake;
        

        private new void OnValidate()
        {
            base.OnValidate();
            typeQuest = TypeQuest.Collect;
        }

        public void Init(bool isProgress, bool isDone, bool canTake)
        {
            this.isProgress = isProgress;
            this.isDone = isDone;
            this.canTake = canTake;
        }

        void IQuest.StartQuest()
        {
            Debug.Log("This task is already in progress");
            // Debug.Log("1XXX");
            // Debug.Log($"quest start {isProgress}");
            // if (isProgress == false)
            // {
            //     isProgress = true;
            //     Debug.Log("isProgress = true");
            // }
            // Debug.Log("2XXX");
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
        
        bool IQuest.IsCompleteConditions()
        {
            if (DataContoller.Imodel.IsSpendCoins(collectAmount, false))
            {
                Debug.Log($"У игрока хватило денег для выполнения задания.");
                Complete();
                Debug.Log("Задание выполнено");
                return true;
            }
            
            Debug.Log("Условия для задания не выполнены!");
            return false;
        }
    }
}
