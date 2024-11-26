using System;
using UnityEngine;

namespace GameAssets.System.GameEvents
{
    public sealed class GameEventsController : MonoBehaviour
    {
        public static Action endGame { get; set; }


        private void OnApplicationQuit() => endGame?.Invoke();
    }
}
