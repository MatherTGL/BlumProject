using System.Linq;
using GameAssets.TestServer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAssets.Meta.Quests
{
    [CreateAssetMenu(fileName = "QuestDataOpenLink", menuName = "Configs/QuestData/Open Link", order = 0)]
    public sealed class OpenQuestsConfig : BaseQuest, IQuest
    {
        [SerializeField, Required, BoxGroup("Quest Settings")]
        private string link;

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
            //Application.OpenURL(link);
            Debug.Log("IQuest.Complete()");
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
    }
}
