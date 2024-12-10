using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAssets.Meta.Quests
{
    public sealed class QuestView : MonoBehaviour, ISpecificQuest
    {
        [ShowInInspector, ReadOnly]
        private IQuest Iquest;


        void ISpecificQuest.Init(string guid, IQuest config)
        {
            Iquest = config;
            Debug.Log($"Quest Inited {guid}");
        }

        //TODO как-то обновлять UI + проверять условия (возможно)
        private void OnEnable()
        {
            Debug.Log("Quest info updated!");
        }

        [Button("Start Quest"), BoxGroup("Quest Control")]
        public void StartQuest() //TODO обновлять UI
        {
            transform.parent.GetComponent<IControlQuestsView>().StartQuest(Iquest.guid);
        }

        [Button("Done Quest"), BoxGroup("Quest Control")]
        public void TakeReward() //TODO обновлять UI
        {
            transform.parent.GetComponent<IControlQuestsView>().TakeReward(Iquest.guid);
        }

        bool ISpecificQuest.IsComplete()
        {
            if (Iquest == null)
                return false;

            return Iquest.IsComplete();
        }
    }
}
