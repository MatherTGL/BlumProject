using Boot;
using UnityEngine;
using static Boot.Bootstrap;

namespace GameAssets.Player.Data
{
    public sealed class DataView : MonoBehaviour, IBoot
    {
        private void OnEnable()
            => DataContoller.Imodel.Update();

        void IBoot.InitAwake() { }

        void IBoot.InitStart()
        {
            //todo получаем доступ к DataController и подписываемся
            DataContoller.Imodel.UpdatedData += UpdateView;
        }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.UI, TypeSingleOrLotsOf.Single);

        private void UpdateView()
        {
            Debug.Log($"View updated! Data - Coins: {DataContoller.Imodel.coins} Tickets: {DataContoller.Imodel.tickets}");
            //todo работаем с отображением данных
        }
    }
}
