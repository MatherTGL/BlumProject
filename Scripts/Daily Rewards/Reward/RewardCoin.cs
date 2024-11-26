using GameAssets.Player.Data;
using UnityEngine;

namespace GameAssets.Meta.DailyReward
{
    public sealed class RewardCoin : IReward
    {
        void IReward.Complete(uint reward)
        {
            DataContoller.Imodel.AddCoins(reward);
            Debug.Log("Reward coin");
        }
    }
}
