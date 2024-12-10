using System.Collections.Generic;
using Boot;
using Sirenix.OdinInspector;
using UnityEngine;
using static Boot.Bootstrap;

namespace GameAssets.Meta.Quests
{
    public sealed class QuestsView : MonoBehaviour, IBoot, IControlQuestsView
    {
        [SerializeField, Required, BoxGroup("Parameters"), AssetsOnly]
        private QuestView prefab;

        [SerializeField, Required, BoxGroup("Parameters"), SceneObjectsOnly]
        private GameObject root;

        [SerializeField, ReadOnly, BoxGroup("Parameters")]
        private int availableQuestCount;


        void IBoot.InitAwake() { }

        async void IBoot.InitStart()
        {
            var quests = await QuestsController.Imodel.GetAllQuestsAsync();
            
            Show(ref quests);
            Debug.Log("QuestsView inited");
        }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.UI, TypeSingleOrLotsOf.Single);

        private void Show(ref List<IQuest> quests)
        {
            GenerateQuestPrefabs(ref quests);
            CalculateAvailableQuestCount(ref quests);
            Debug.Log($"Show quests count: {quests.Count}");
        }

        private void GenerateQuestPrefabs(ref List<IQuest> quests)
        {
            for (byte i = 0; i < quests.Count; i++)
            {
                var quest = Instantiate(prefab, transform.position, Quaternion.identity, root.transform);
                quest.GetComponent<ISpecificQuest>().Init((quests[i] as BaseQuest).guid, quests[i]);
            }
        }

        private void CalculateAvailableQuestCount(ref List<IQuest> quests)
        {
            for (byte i = 0; i < quests.Count; i++)
            {
                if (quests[i].IsComplete() == false)
                    availableQuestCount++;
            }
        }

        void IControlQuestsView.StartQuest(string guid)
            => QuestsController.Imodel.StartQuest(guid);

        async void IControlQuestsView.TakeReward(string guid)
        {
            if (await QuestsController.Imodel.TryTakeRewardAsync(guid))
                availableQuestCount--;
        }

        [Button("Show Available Quests", ButtonSizes.Medium), BoxGroup("Control")]
        public void ShowAvailableQuests()
        {
            Debug.Log($"Available count: {availableQuestCount}");
        }
    }
}
