using System;
using System.Threading.Tasks;
using GameAssets.Meta.Items.ScriptableObjects;
using GameAssets.Scripts.Service.Time;
using GameAssets.System.SaveSystem;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAssets.Player.Pharm
{
    [CreateAssetMenu(fileName = "AFKPharm", menuName = "Configs/Pharm/AFK", order = 1)]
    public sealed class ConfigPassiveFarming : ItemInfo, ISaveable<ConfigPassiveFarming>
    {
        [field: SerializeField, BoxGroup("Parameters"), MinValue(1.0f), InfoBox("In Seconds!")]
        public float pharmTick { get; private set; } = 1.0f;

        [field: SerializeField, BoxGroup("Parameters"), MinValue(1), InfoBox("In Hours!")]
        public byte maxPharmTime { get; private set; } = 8;

        [field: SerializeField, BoxGroup("Parameters/Income"), MinValue(0.01f)]
        public float incomeInTick { get; private set; } = 1.0f;

        public DateTime? lastAddedTime { get; set; }

        public DateTime? startPharmTime { get; set; }

        public float currentPharmMoney { get; set; }

        public bool isActivePharm { get; set; }

        [field: NonSerialized] public bool Loaded { get; set; }

        [JsonIgnore] string ISaveable<ConfigPassiveFarming>.SaveId => GUID;


        Task IPreloadable.AsyncPreload() => Task.CompletedTask;

        async Task ISaveable<ConfigPassiveFarming>.OnFirstTimeLoad()
        {
            currentPharmMoney = 0;
            isActivePharm = false;
            lastAddedTime = await WorldTime.GetAsync();
        }

        void ISaveable<ConfigPassiveFarming>.OnLoad(ConfigPassiveFarming loadedItem)
        {
            isActivePharm = loadedItem.isActivePharm;
            currentPharmMoney = loadedItem.currentPharmMoney;
            lastAddedTime = loadedItem.lastAddedTime;
            startPharmTime = loadedItem.startPharmTime;
        }
    }
}
