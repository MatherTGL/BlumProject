using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GameAssets.Meta.Referrals
{
    public sealed class ConcreteReferralView : MonoBehaviour, IReferral
    {
        [SerializeField, Required]
        private TextMeshProUGUI textReferralName;

        [SerializeField, ReadOnly]
        private string referralName;


        void IReferral.Init(string referralName)
        {
            this.referralName = referralName;
            Debug.Log($"ConcreteRef Inited / {this.referralName}");
        }
    }
}
