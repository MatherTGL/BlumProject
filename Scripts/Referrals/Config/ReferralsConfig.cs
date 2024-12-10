using System;
using System.Threading.Tasks;
using GameAssets.Meta.Items.ScriptableObjects;
using GameAssets.System.SaveSystem;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAssets.Meta.Referrals
{
    [CreateAssetMenu(fileName = "ReferralsConf", menuName = "Configs/Referrals", order = 1)]
    public sealed class ReferralsConfig : ItemInfo, ISaveable<ReferralsConfig>
    {
        [SerializeField, BoxGroup("Parameters"), Title("The number of coins that the player receives by clicking on the referral link")]
        [MinValue(0)]
        private double _referralPlayerBonus;

        [JsonIgnore]
        public double referralsPlayerBonus => _referralPlayerBonus;

        [SerializeField, BoxGroup("Parameters"), Title("Number of coins received by the owner of the referral link")]
        [MinValue(0)]
        private double _inviterBonus;

        [JsonIgnore]
        public double inviterBonus => _inviterBonus;

        [field: SerializeField, ReadOnly, BoxGroup("Parameters/ReadOnly")]
        public int referralsCount { get; set; }
        
        [JsonIgnore] string ISaveable<ReferralsConfig>.SaveId => GUID;

        [field: NonSerialized] bool ISaveable<ReferralsConfig>.Loaded { get; set; }
        
        
        Task IPreloadable.AsyncPreload()  => Task.CompletedTask;

        void ISaveable<ReferralsConfig>.OnLoad(ReferralsConfig loadedItem)
        {
            referralsCount = loadedItem.referralsCount;
        }

        Task ISaveable<ReferralsConfig>.OnFirstTimeLoad()
        {
            referralsCount = 0;
            return Task.CompletedTask;
        }
    }
}
