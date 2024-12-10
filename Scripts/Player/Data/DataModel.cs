using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameAssets.General.Server;
using GameAssets.Meta.Quests;
using GameAssets.Player.Data.Config;
using GameAssets.Scripts.Service.Time;
using GameAssets.System.GameEvents;
using GameAssets.System.SaveSystem;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace GameAssets.Player.Data
{
    public sealed class DataModel : IDataModel
    {
        private ConfigDataPlayer _config;

        float IDataModel.coins
        {
            get => _config.currentCoins;
            set => _config.currentCoins = value;
        }

        ushort IDataModel.tickets
        {
            get => _config.currentTickets;
            set => _config.currentTickets = value;
        }

        //TODO подгружаем с телеги
        public static string username { get; private set; } = "Player";

        //TODO подгружаем с телеги
        public static Image avatar { get; private set; }

        public Action<BaseQuest.TypeQuest, BaseQuest.JobSubtype> UpdatedData { get; set; }


        public async void InitAsync()
        {
            GameEventsController.endGame += EndGame;
            await InitConfigParametersAsync();
            InitServerData();
        }

        private async UniTask InitConfigParametersAsync()
        {
            _config = await GetConfigAsync();
            await ((ISaveable<ConfigDataPlayer>)_config).AsyncLoad();
        }

        private async void SaveData()
        {
            _config ??= await GetConfigAsync();
            ((ISaveable<ConfigDataPlayer>)_config).Save();
        }

        private async void InitServerData()
        {
            if (_config == null)
                await InitConfigParametersAsync();

            await PlayfabController.LoginAsync();
            var refCoins = await PlayfabController.GetRefCoinsAsync();

            _config.currentCoins += refCoins;
            ((ISaveable<ConfigDataPlayer>)_config).Save();

            Update();
        }

        private async void EndGame()
        {
            _config.lastGameTime = await WorldTime.GetAsync();
            Debug.Log($"_config.lastGameTime: {_config.lastGameTime} / {await WorldTime.GetAsync()}");
            SaveData();
        }

        public async UniTask<ConfigDataPlayer> GetConfigAsync()
            => await Addressables.LoadAssetAsync<ConfigDataPlayer>("DataPlayer").Task;

        public void Update()
        {
            UpdatedData?.Invoke(BaseQuest.TypeQuest.Collect, BaseQuest.JobSubtype.Money);
            SaveData();
        }

        async UniTask<float> IDataModel.GetCoinsAsync()
        {
            if (_config == null)
                await InitConfigParametersAsync();

            return _config.currentCoins;
        }
    }
}
