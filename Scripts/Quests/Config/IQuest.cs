using System.Linq;
using GameAssets.TestServer;
using UnityEngine;

namespace GameAssets.Meta.Quests
{
    public interface IQuest
    {
        bool isProgress { get; protected set; }

        bool isDone { get; protected set; }

        bool canTake { get; protected set; }

        string guid { get; }


        void Init()
        {
            var quest = TestServerController.userQuests.quests.Where(quest => quest.id == guid).FirstOrDefault();

            if (quest != null)
            {
                isProgress = quest.isInProgress;
                isDone = quest.isDone;
                canTake = quest.CanTake;
            }

            Debug.Log($"Quest inited and set server data - Progress: {isProgress} Done: {isDone} CanTake: {canTake}");
        }

        bool IsComplete();

        void Complete();
    }
}
