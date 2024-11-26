using Boot;
using Sirenix.OdinInspector;
using UnityEngine;
using static Boot.Bootstrap;

namespace GameAssets.Meta.Referrals
{
    public sealed class ReferralsView : MonoBehaviour, IBoot
    {
        [SerializeField, Required, BoxGroup("Parameters"), AssetsOnly]
        private GameObject prefab;

        [SerializeField, Required, BoxGroup("Parameters"), SceneObjectsOnly]
        private GameObject root;


        void IBoot.InitAwake() { }

        void IBoot.InitStart()
        {
            UpdateView();
            Debug.Log("Inited RefView");
        }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.UI, TypeSingleOrLotsOf.Single);

        private void UpdateView()
        {
            var referralsList = ReferralsController.Imodel.GetReferralList();

            for (byte referral = 0; referral < referralsList.Count; referral++)
            {
                var refObj = Instantiate(prefab, transform.position, Quaternion.identity, root.transform);
                refObj.GetComponent<IReferral>().Init(referralsList[referral].Username);
            }
            Debug.Log($"Update View for Ref / {referralsList.Count}");
        }
    }
}
