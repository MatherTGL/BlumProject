using System;
using UnityEngine;

namespace GameAssets.Meta.DailyReward
{
    public static class DailyRewardsController
    {
        private static IDailyRewards IdailyRewards = new DailyRewardsModel();

        public static Action UpdatedInfo { get; set; }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            IdailyRewards.Init();
            UpdatedInfo?.Invoke();
        }

        public static void Claim() => IdailyRewards.Claim();
    }
}
