using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAssets.Meta.Quests
{
    //TODO подгружать конфиги и инфу с сервера о квестах и устанавливать данные
    public sealed class QuestView : MonoBehaviour, ISpecificQuest
    {
        [ShowInInspector, ReadOnly]
        private IQuest Iquest;

        [SerializeField, ReadOnly]
        private string guid;


        void ISpecificQuest.Init(string guid, IQuest config)
        {
            Iquest = config;
            this.guid = guid;
            Debug.Log($"Quest Inited {guid}");
        }

        //TODO как-то обновлять UI
        private void OnEnable()
        {
            Debug.Log("Quest info updated!");
        }

        [Button("Start Quest"), BoxGroup("Quest Control")]
        public void StartQuest() //TODO обновлять UI
        {
            transform.parent.GetComponent<IControlQuestsView>().StartQuest(guid);
        }

        [Button("Done Quest"), BoxGroup("Quest Control")]
        public void TakeReward() //TODO обновлять UI
        {
            transform.parent.GetComponent<IControlQuestsView>().TakeReward(guid);
        }

        bool ISpecificQuest.IsComplete()
        {
            if (Iquest == null)
                return false;

            return Iquest.IsComplete();
        }
    }
}
