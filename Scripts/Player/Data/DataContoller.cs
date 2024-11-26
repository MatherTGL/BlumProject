using UnityEngine;

namespace GameAssets.Player.Data
{
    public static class DataContoller
    {
        public static IDataModel Imodel { get; } = new DataModel();


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init() => Imodel.InitAsync();
    }
}
