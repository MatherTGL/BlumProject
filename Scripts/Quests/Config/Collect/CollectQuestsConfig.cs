using Sirenix.OdinInspector;
using UnityEngine;

namespace GameAssets.Meta.Quests
{
    public class CollectQuestsConfig : BaseQuest
    {
        [field: SerializeField, BoxGroup("Quest"), MinValue(0)]
        public int collectAmount { get; private set; }
    }
}
