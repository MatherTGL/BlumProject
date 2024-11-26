using GameAssets.Meta.Items.ScriptableObjects;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAssets.Meta.Referrals
{
    [CreateAssetMenu(fileName = "ReferralsConf", menuName = "Configs/Referrals", order = 1)]
    public sealed class ReferralsConfig : ItemInfo
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
    }
}
