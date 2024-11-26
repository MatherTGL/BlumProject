using UnityEngine;
using static GameAssets.General.InternetConnection;

namespace GameAssets.General
{
    //TODO должна отображать ui тех работ или отсутствия соединения чекая InternetConnectionController
    public sealed class InternetConnectionView : MonoBehaviour
    {
        private void Start()
        {
            InternetConnection.connectionBroken += ShowConnectionBroken;
        }

        private void ShowConnectionBroken(DisconnectType disconnectType)
        {
            Debug.LogWarning("Connection Broken... Try again!");
        }
    }
}
