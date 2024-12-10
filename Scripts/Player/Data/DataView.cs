using Boot;
using GameAssets.Meta.Quests;
using UnityEngine;
using static Boot.Bootstrap;
using NotImplementedException = System.NotImplementedException;

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

        private void UpdateView(BaseQuest.TypeQuest typeQuest, BaseQuest.JobSubtype jobSubtype)
        {
            Debug.Log($"View updated! Data - Coins: {DataContoller.Imodel.coins} Tickets: {DataContoller.Imodel.tickets}");
            //todo работаем с отображением данных
        }

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.UI, TypeSingleOrLotsOf.Single);
    }
}
