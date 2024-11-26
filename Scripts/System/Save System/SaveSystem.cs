//#define YG

using System;
using System.Collections.Generic;
using GameAssets.General.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

#if YG
using YG;
#endif

namespace GameAssets.System.SaveSystem
{
    public static class SaveSystem
    {
        private static Dictionary<string, string> GameDataSaves = new Dictionary<string, string>();
        private static Dictionary<string, object> OtherDataSave; //для простых типов
        private static bool IsDataLoaded;
        public static event Action ForceReload;
        private static bool pendingReload;

        public static void PendingForceReload()
        {
            pendingReload = true;
            LoadAllData();
            ForceReload.Invoke();
        }

        public static void SaveGameData<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning($"Key is empty");
                return;
            }

            LoadAllData();
            var gameDataSave = JsonConvert.SerializeObject(data);
            Debug.Log($"Saved key: {key} Data: {gameDataSave}");

            if (GameDataSaves.ContainsKey(key))
                GameDataSaves[key] = gameDataSave;
            else
                GameDataSaves.Add(key, gameDataSave);

            SaveAllData();
        }

        public static void SaveOtherData(string key, object data)
        {
            LoadAllData();
            if (OtherDataSave.ContainsKey(key))
            {
                OtherDataSave[key] = data;
            }
            else
            {
                OtherDataSave.Add(key, data);
            }

            SaveAllData();
        }

        public static T LoadGameData<T>(string key)
        {
            LoadAllData();

            if (GameDataSaves.TryGetValue(key, out var save))
            {
                //bug При десереализации создается новый экземпляр объекта, юнити ругается на это, так как SO
                return JsonConvert.DeserializeObject<T>(save);
            }
            else
            {
                throw new UnityException("Save key is don't exist");
            }
        }

        public static object LoadOtherData(string key)
        {
            LoadAllData();

            if (OtherDataSave.TryGetValue(key, out var data))
            {
                return data;
            }
            else
            {
                throw new UnityException("Save key is don't exist");
            }
        }

        public static bool TryLoadOtherData<T>(string key, out T @object)
        {
            LoadAllData();

            if (OtherDataSave.TryGetValue(key, out var data))
            {
                JObject jObj = (JObject)data;
                @object = JsonConvert.DeserializeObject<T>(jObj.ToString());
                return true;
            }
            else
            {
                @object = default;
                Debug.Log(key + " does not exist");
                return false;
            }
        }

        public static void DeleteGameData(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning($"Key is empty");
                return;
            }

            LoadAllData();
            if (GameDataSaves.ContainsKey(key))
            {
                GameDataSaves.Remove(key);
            }

            SaveAllData();
        }

        public static void DeleteAllData()
        {
            GameDataSaves = new Dictionary<string, string>();
            OtherDataSave = new Dictionary<string, object>();
            SaveAllData();
            PendingForceReload();
        }

        public static void DeleteOtherData(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogWarning($"Key is empty");
                return;
            }

            LoadAllData();

            if (OtherDataSave.ContainsKey(key))
                OtherDataSave.Remove(key);

            SaveAllData();
        }

        public static bool HasGameData(string key)
        {
            LoadAllData();
            return GameDataSaves.ContainsKey(key);
        }

        public static bool HasOtherData(string key)
        {
            LoadAllData();
            return OtherDataSave.ContainsKey(key);
        }

        private static (string, string) LoadDataPrefs()
        {
            string gameDataSaves = PlayerPrefs.GetString("gameData");
            string otherDataSaves = PlayerPrefs.GetString("otherData");
            return (gameDataSaves, otherDataSaves);
        }

        private static void SavePrefs(string gameDataSaves, string otherDataSaves)
        {
            PlayerPrefs.SetString("gameData", gameDataSaves);
            PlayerPrefs.SetString("otherData", otherDataSaves);
        }

        private static async void LoadAllData()
        {
            bool needReload = !IsDataLoaded || pendingReload;

            if (!needReload) return;

            (string gameDataSaves, string otherDataSaves) loadData = (null, null);
            GameDataSaves = new Dictionary<string, string>();
            OtherDataSave = new Dictionary<string, object>();

            Debug.Log($"await PlayfabController.LoadDataAsync(): {await PlayfabController.LoadDataAsync()}");
            loadData = await PlayfabController.LoadDataAsync();
            Debug.Log($"loadData: {loadData}");

            if (!string.IsNullOrEmpty(loadData.gameDataSaves))
                GameDataSaves = JsonConvert.DeserializeObject<Dictionary<string, string>>(loadData.gameDataSaves);

            if (!string.IsNullOrEmpty(loadData.otherDataSaves))
                OtherDataSave = JsonConvert.DeserializeObject<Dictionary<string, object>>(loadData.otherDataSaves);

            IsDataLoaded = true;
            pendingReload = false;
        }


        //TODO сделать кеш для сохранения на сервак
        private static async void SaveAllData()
        {
            string gameDataSaves = JsonConvert.SerializeObject(GameDataSaves);
            string otherDataSaves = JsonConvert.SerializeObject(OtherDataSave);

            //SavePrefs(gameDataSaves, otherDataSaves);
            await PlayfabController.LoginAsync();
            Debug.Log($"gameDataSaves in saveAllData(): {gameDataSaves}");
            PlayfabController.SaveData(otherDataSaves, gameDataSaves);
        }
    }
}