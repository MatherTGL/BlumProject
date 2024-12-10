using Cysharp.Threading.Tasks;

namespace GameAssets.General.Server
{
    public sealed class PlayfabTakeRewardQuest
    {
        public async UniTask<bool> TryTakeAsync(string guid)
        {
            return await IsTakedAsync();
        }

        private async UniTask<bool> IsTakedAsync()
        {
            return false;
        }
    }
}