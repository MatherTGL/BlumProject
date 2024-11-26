using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameAssets.Meta.Items.ScriptableObjects;
using GameAssets.System.SaveSystem;
using Newtonsoft.Json;
using SerializableDictionary.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using static GameAssets.Meta.DailyReward.DailyRewardConfig;

namespace GameAssets.Meta.DailyReward
{
    [CreateAssetMenu(fileName = "DailyRewardsConf", menuName = "Configs/DailyRewards", order = 1)]
    public sealed class DailyRewardConfig : ItemInfo, ISaveable<DailyRewardConfig>
    {
        public enum TypeReward : byte
        {
            Coin, Ticket
        }

        [SerializeField, BoxGroup("Parameters")]
        private List<RewardParameters> _days = new();

        [JsonIgnore]
        public List<RewardParameters> days => _days;

        public DateTime? lastClaimReward { get; set; }

        [field: SerializeField, BoxGroup("Parameters"), JsonIgnore, PropertySpace(10, 0)]
        public byte maxDaysInSequence { get; private set; } = 15;

        [field: SerializeField, ReadOnly, BoxGroup("Parameters")]
        public byte dayInSequence { get; set; } = 0;

        public bool isCollect { get; set; }

        [field: NonSerialized] public bool Loaded { get; set; }

        [JsonIgnore] string ISaveable<DailyRewardConfig>.SaveId => GUID;


        void ISaveable<DailyRewardConfig>.OnLoad(DailyRewardConfig loadedItem)
        {
            lastClaimReward = loadedItem.lastClaimReward;
            dayInSequence = loadedItem.dayInSequence;
            isCollect = loadedItem.isCollect;
        }

        Task ISaveable<DailyRewardConfig>.OnFirstTimeLoad()
        {
            dayInSequence = 0;
            isCollect = false;
            return Task.CompletedTask;
        }

        Task IPreloadable.AsyncPreload() => Task.CompletedTask;
    }

    [Serializable]
    public sealed class RewardParameters
    {
        [field: SerializeField, BoxGroup("Reward"), JsonIgnore]
        public SerializableDictionary<TypeReward, uint> reward { get; private set; }
    }
}
