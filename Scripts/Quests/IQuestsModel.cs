using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace GameAssets.Meta.Quests
{
    public interface IQuestsModel
    {
        void StartQuest(string guid);

        UniTask<bool> TryTakeRewardAsync(string guid);

        UniTask<List<IQuest>> GetAllQuestsAsync();
    }
}
