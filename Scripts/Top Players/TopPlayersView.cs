using System.Collections.Generic;
using Boot;
using PlayFab.ClientModels;
using Sirenix.OdinInspector;
using UnityEngine;
using static Boot.Bootstrap;

namespace GameAssets.Meta.TopPlayers
{
    public sealed class TopPlayersView : MonoBehaviour, IBoot
    {
        [SerializeField, LabelText("Prefab"), Required, BoxGroup("Parameters"), AssetsOnly]
        private GameObject prefabTopPlayer;

        [SerializeField, Required, BoxGroup("Parameters"), SceneObjectsOnly]
        private GameObject root;


        void IBoot.InitAwake() => TopPlayers.Updated += UpdateView;

        void IBoot.InitStart() => TopPlayers.Init();

        (TypeLoadObject typeLoad, TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (TypeLoadObject.UI, TypeSingleOrLotsOf.Single);

        private void UpdateView(List<PlayerLeaderboardEntry> playersInTop)
        {
            Debug.Log($"playersInTop.Count: {playersInTop.Count}");
            for (byte player = 0; player < playersInTop.Count; player++)
            {
                Debug.Log($"created top player: username: {playersInTop[player].PlayFabId} and coins: {playersInTop[player].StatValue}");
                var playerObj = Instantiate(prefabTopPlayer, transform.position, Quaternion.identity, root.transform);

                //TODO передавать DisplayName, а не id
                playerObj.GetComponent<ITopPlayer>().Init(playersInTop[player].PlayFabId, playersInTop[player].StatValue, playersInTop[player].Position);
            }
        }
    }
}
