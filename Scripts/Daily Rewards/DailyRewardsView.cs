using Boot;
using Sirenix.OdinInspector;
using UnityEngine;
using static Boot.Bootstrap;

namespace GameAssets.Meta.DailyReward
{
    public sealed class DailyRewardsView : MonoBehaviour, IBoot
    {
        void IBoot.InitAwake() { }

        void IBoot.InitStart()
        {
            DailyRewardsController.UpdatedInfo += UpdateView;
            Debug.Log("Init Daily Rewards View");
        }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.UI, TypeSingleOrLotsOf.Single);

        private void UpdateView()
        {
            Debug.Log("UpdateView in DailyRewardsView");
        }

        [Button("Claim", ButtonSizes.Medium), BoxGroup("Control")]
        public void Claim()
            => DailyRewardsController.Claim();
    }
}
