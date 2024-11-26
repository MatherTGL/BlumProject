using GameAssets.Meta.Items.ScriptableObjects;
using SerializableDictionary.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using static GameAssets.Meta.DailyReward.DailyRewardConfig;

namespace GameAssets.Meta.Quests
{
    public class BaseQuest : ItemInfo
    {
        public enum TypeQuest : byte
        {
            Collect, OpenLink
        }

        [field: SerializeField, EnumToggleButtons, LabelText("Type"), BoxGroup("Reward")]
        public TypeQuest typeQuest { get; set; }

        [field: SerializeField, BoxGroup("Parameters")]
        public SerializableDictionary<TypeReward, uint> reward { get; private set; } = new();
    }
}
