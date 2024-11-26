using System.Linq;
using GameAssets.TestServer;
using UnityEngine;

namespace GameAssets.Meta.Quests
{
    [CreateAssetMenu(fileName = "QuestDataCollectFriends", menuName = "Configs/QuestData/Collect/Friends", order = 0)]
    public sealed class CollectFriendsQuest : CollectQuestsConfig, IQuest
    {
        string IQuest.guid => GUID;

        private bool isProgress;

        private bool isDone;

        private bool canTake;

        bool IQuest.isProgress
        {
            get => isProgress; set => isProgress = value;
        }

        bool IQuest.isDone
        {
            get => isDone; set => isDone = value;
        }

        bool IQuest.canTake
        {
            get => canTake; set => canTake = value;
        }


        void IQuest.Complete()
        {
            Debug.Log("Complete quest");
        }

        bool IQuest.IsComplete()
        {
            if (TestServerController.userQuests.quests.Any(quest => quest.id == GUID && quest.isDone))
            {
                Debug.Log("Quest completed");
                return true;
            }

            Debug.Log("Quest is not completed");
            return false;
        }

        private new void OnValidate()
        {
            base.OnValidate();
            typeQuest = TypeQuest.Collect;
        }
    }
}
