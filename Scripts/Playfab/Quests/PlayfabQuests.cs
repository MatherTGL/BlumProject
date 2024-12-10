using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameAssets.Meta.Quests;

namespace GameAssets.General.Server
{
    public sealed class PlayfabQuests
    {
        private readonly PlayfabQuestsList _questList = new();
        
        private readonly PlayfabUpdateQuestState _updateQuestState = new();
        
        private readonly PlayfabLoadQuests _loadQuests = new();
        
        private readonly PlayfabTakeRewardQuest _takeRewardQuest = new();
        
        
        public async UniTask<List<BaseQuest>> LoadAllServerQuestsAsync()
            => await _loadQuests.LoadAllQuestsAsync(_questList);

        public async UniTask<bool> TryStartQuestAsync(IQuest quest)
            => await _updateQuestState.TryUpdateAsync(quest, _questList, PlayfabUpdateQuestState.UpdateType.Start);

        public async UniTask<bool> TryCompleteQuestAsync(IQuest quest)
            => await _updateQuestState.TryUpdateAsync(quest, _questList, PlayfabUpdateQuestState.UpdateType.Complete);

        public async UniTask<bool> TryTakeRewardAsync(string guid)
            => await _takeRewardQuest.TryTakeAsync(guid);
    }
}
