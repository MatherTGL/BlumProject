using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameAssets.Meta.Quests
{
    public interface IQuestsModel
    {
        void StartQuest(string guid);

        Task<bool> IsTakeRewardAsync(string guid);

        Task<List<IQuest>> GetAllQuestsAsync();
    }
}
