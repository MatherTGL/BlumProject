using UnityEngine;

namespace GameAssets.Meta.Quests
{
    [CreateAssetMenu(fileName = "QuestDataCollectFriends", menuName = "Configs/QuestData/Collect/Friends", order = 0)]
    public sealed class CollectFriendsQuest : CollectQuestsConfig, IQuest
    {
        bool IQuest.isProgress { get; }

        bool IQuest.isDone { get; }

        bool IQuest.canTake { get; }

        
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

        //TODO дописать
        bool IQuest.IsCompleteConditions()
        {
            Debug.Log("Произошло обновление рефералов у игрока. Проводится проверка их количества");
            return false;
        }
    }
}
