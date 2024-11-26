using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace GameAssets.System.SaveSystem
{
    public interface ISaveable<in T> : IPreloadable
    {
        [JsonIgnore] public string SaveId { get; }
        [JsonIgnore] public bool Loaded { get; set; }

        public void Save()
            => SaveSystem.SaveGameData(SaveId, this);

        public async Task AsyncLoad()
        {
            if (string.IsNullOrEmpty(SaveId))
            {
                Debug.LogError("SaveId is empty");
                return;
            }

            Debug.Log($"loaded: {Loaded}");
            if (Loaded) return;

            SaveSystem.ForceReload += ForceReload;

            if (!SaveSystem.HasGameData(SaveId))
            {
                await OnFirstTimeLoad();
                Loaded = true;
                return;
            }

            T loadedItem = SaveSystem.LoadGameData<T>(SaveId);
            OnLoad(loadedItem);
            Loaded = true;
        }

        public async void ForceReload()
        {
            SaveSystem.ForceReload -= ForceReload;
            Loaded = false;
            await AsyncLoad();
        }

        public void Delete()
            => SaveSystem.DeleteGameData(SaveId);

        void OnLoad(T loadedItem);

        Task OnFirstTimeLoad();
    }
}