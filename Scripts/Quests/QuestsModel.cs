using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAssets.TestServer;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameAssets.Meta.Quests
{
    public sealed class QuestsModel : IQuestsModel
    {
        private async Task<List<IQuest>> LoadQuestsAsync()
        {
            var quests = await Addressables.LoadAssetsAsync<IQuest>("Quest", null).Task;
            return GetSortedQuests((List<IQuest>)quests);
        }

        private async Task<IQuest> LoadOneQuestAsync(string guid)
            => await Addressables.LoadAssetAsync<IQuest>(guid).Task;

        private List<IQuest> GetSortedQuests(List<IQuest> quests)
        {
            quests = quests.OrderByDescending(quest => quest.IsComplete()).ToList();
            return quests;
        }

        void IQuestsModel.StartQuest(string guid)
        {
            TestServerController.BeginQuest(guid);
        }

        async Task<bool> IQuestsModel.IsTakeRewardAsync(string guid)
        {
            var config = await LoadOneQuestAsync(guid);
            Debug.Log($"config is done: {config}");

            if (config.IsComplete())
            {
                TestServerController.TakeReward(guid);
                TestServerController.CompleteQuest(guid);
                config.Complete();
                Addressables.Release(config);
                return true;
            }

            Addressables.Release(config);
            return false;
        }

        async Task<List<IQuest>> IQuestsModel.GetAllQuestsAsync()
            => await LoadQuestsAsync();
    }
}
