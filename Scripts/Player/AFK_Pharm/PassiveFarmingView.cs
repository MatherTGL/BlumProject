using Boot;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAssets.Player.Pharm
{
    public sealed class PassiveFarmingView : MonoBehaviour, IBoot
    {
        void IBoot.InitAwake() { }

        void IBoot.InitStart()
            => PassiveFarmingController.instance.Imodel.UpdatedData += UpdateView;

        (Bootstrap.TypeLoadObject typeLoad, Bootstrap.TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (Bootstrap.TypeLoadObject.UI, Bootstrap.TypeSingleOrLotsOf.Single);

        private void UpdateView()
        {
            //TODO обновляем вьюшку
            Debug.Log("Вьюшка обновлена!");
        }

        [Button("Start Pharm"), BoxGroup("Control")]
        public void StartPharm()
            => PassiveFarmingController.instance.Imodel.StartPharm();
    }
}
