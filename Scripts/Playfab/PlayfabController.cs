using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boot;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace GameAssets.General.Server
{
    public sealed class PlayfabController : MonoBehaviour, IBoot
    {
        //TODO использовать
        public static int MaxRetries { get; private set; } = 3;

        public static float RetryDelay { get; private set; } = 2f;

        private static readonly PlayfabLogin _login = new();

        private static readonly PlayfabLeaderboard _leaderboard = new();

        private static readonly PlayfabSaves _saves = new();

        private static readonly PlayfabFriends _friends = new();

        private static readonly PlayfabGetUserData _userData = new();

        //TODO поменять userid
        private static readonly string _userID = "SFSFI";


        void IBoot.InitAwake() { }

        async void IBoot.InitStart()
        {
            _friends.Init();
            await _login.LoginAsync(_userID);

            //TODO перенести в более нужное место отправку данных в борд
            SendDataToLeaderboard();
        }

        (Bootstrap.TypeLoadObject typeLoad, Bootstrap.TypeSingleOrLotsOf singleOrLotsOf) IBoot.GetTypeLoad()
            => (Bootstrap.TypeLoadObject.SuperImportant, Bootstrap.TypeSingleOrLotsOf.Single);

        public static async ValueTask LoginAsync()
            => await _login.LoginAsync(_userID);

        public static void SendDataToLeaderboard()
            => _leaderboard.Send();

        public static async UniTask<List<PlayerLeaderboardEntry>> GetLeaderboardAsync()
            => await _leaderboard.GetAsync();

        public static void SaveData(string otherData, string gameData)
            => _saves.Save(otherData, gameData);

        public static async UniTask<(string, string)> LoadDataAsync()
            => await _saves.LoadAsync();

        public static async UniTask<bool> IsRegistationAsync()
            => await _login.IsRegistationAsync();

        public static async UniTask<List<FriendInfo>> GetFriendsListAsync()
            => await _friends.GetFriendsAsync();

        public static async UniTask<float> GetRefCoinsAsync()
        => await _userData.GetRefCoinsAsync();
    }
}
