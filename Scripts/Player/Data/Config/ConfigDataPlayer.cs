using System;
using System.Threading.Tasks;
using GameAssets.Meta.Items.ScriptableObjects;
using GameAssets.Scripts.Service.Time;
using GameAssets.System.SaveSystem;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAssets.Player.Data.Config
{
    [CreateAssetMenu(fileName = "ConfDataPlayer", menuName = "Configs/Data/Player", order = 1)]
    public sealed class ConfigDataPlayer : ItemInfo, ISaveable<ConfigDataPlayer>
    {
        [field: SerializeField, BoxGroup("Init Parameters", true), LabelText("Coins")]
        [field: JsonIgnore] public uint startCoins { get; private set; }

        [field: SerializeField, BoxGroup("Init Parameters", true), LabelText("Tickets")]
        [field: JsonIgnore] public ushort startTickets { get; private set; }

        public float currentCoins { get; set; }

        public ushort currentTickets { get; set; }

        public DateTime lastGameTime { get; set; }

        [field: NonSerialized] public bool Loaded { get; set; }

        [JsonIgnore] string ISaveable<ConfigDataPlayer>.SaveId => GUID;


        Task IPreloadable.AsyncPreload()
            => Task.CompletedTask;

        async Task ISaveable<ConfigDataPlayer>.OnFirstTimeLoad()
        {
            lastGameTime = await WorldTime.GetAsync();
            currentCoins = startCoins;
            currentTickets = startTickets;
        }

        void ISaveable<ConfigDataPlayer>.OnLoad(ConfigDataPlayer loadedItem)
        {
            lastGameTime = loadedItem.lastGameTime;
            currentCoins = loadedItem.currentCoins;
            currentTickets = loadedItem.currentTickets;
        }
    }
}
