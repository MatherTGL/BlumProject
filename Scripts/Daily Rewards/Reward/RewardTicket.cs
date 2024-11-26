using GameAssets.Player.Data;
using UnityEngine;

namespace GameAssets.Meta.DailyReward
{
    public sealed class RewardTicket : IReward
    {
        void IReward.Complete(uint reward)
        {
            DataContoller.Imodel.AddTickets((ushort)reward);
            Debug.Log("Reward ticket");
        }
    }
}
