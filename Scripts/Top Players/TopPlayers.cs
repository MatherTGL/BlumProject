using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAssets.General.Server;
using PlayFab.ClientModels;
using UnityEngine;

namespace GameAssets.Meta.TopPlayers
{
    public static class TopPlayers
    {
        public static Action<List<PlayerLeaderboardEntry>> Updated;


        public static async void Init() => await GetServerDataAsync();

        private static async ValueTask GetServerDataAsync()
        {
            Debug.Log("Leaderboard request");

            List<PlayerLeaderboardEntry> leaderboard = null;
            leaderboard = await PlayfabController.GetLeaderboardAsync();

            FormatTopList(ref leaderboard);
        }

        private static void FormatTopList(ref List<PlayerLeaderboardEntry> playersInTop)
        {
            playersInTop = playersInTop.OrderBy(item => item.Position).ToList();
            Updated?.Invoke(playersInTop);
        }
    }
}
