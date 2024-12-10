using System;
using System.Collections.Generic;
using GameAssets.Meta.Items.ScriptableObjects;
using Newtonsoft.Json;
using SerializableDictionary.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using static GameAssets.Meta.DailyReward.DailyRewardConfig;

namespace GameAssets.Meta.Quests
{
    [Serializable]
    public class BaseQuest : ItemInfo
    {
        public enum TypeQuest : byte
        {
            Collect, OpenLink
        }

        [field: SerializeField, EnumToggleButtons, LabelText("Type"), BoxGroup("Parameters")]
        public TypeQuest typeQuest { get; set; }

        public enum JobSubtype : byte
        {
            Money, Friends
        }
        
        [field: SerializeField, EnumToggleButtons, LabelText("Subtype"), BoxGroup("Parameters"), ShowIf("typeQuest", TypeQuest.Collect)]
        [PropertySpace(10, 0)]
        public JobSubtype jobSubtype { get; set; }
        
        [JsonIgnore, SerializeField, BoxGroup("Parameters"), ]
        private SerializableDictionary<TypeReward, uint> _questReward = new();

        [JsonIgnore]
        public Dictionary<TypeReward, uint> questReward => _questReward.Dictionary;

        [field: SerializeField, ReadOnly]
        public bool isProgress { get; set; }

        [field: SerializeField, ReadOnly]
        public bool isDone { get; set; }

        [field: SerializeField, ReadOnly]
        public bool canTake { get; set; }

        public string guid { get => GUID; set => GUID = value; }
    }
}
