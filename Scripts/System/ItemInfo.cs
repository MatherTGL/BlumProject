#if LEAN_LOCALE
using Lean.Localization;
#else
#endif
using System;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameAssets.Meta.Items.ScriptableObjects
{
    public class ItemInfo : ScriptableObject
    {
        [Tooltip("Автоматом = имя если пусто")]
        [JsonIgnore][field: SerializeField] public string NameKey { get; protected set; }
        [JsonIgnore][field: SerializeField] public string GUID { get; protected set; }

        [JsonIgnore]
        public string LocalizedName
        {
            get
            {
#if LEAN_LOCALE
                if (LeanLocalization.CurrentTranslations.ContainsKey(NameKey))
                    return (string)LeanLocalization.CurrentTranslations[NameKey].Data;
#else
                // if (LocalizationBase.Customlocales.ContainsKey(NameKey))
                //     return (string)LocalizationBase.Customlocales[NameKey];
#endif
                return NameKey;
            }
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(NameKey)) NameKey = name;

            if (string.IsNullOrEmpty(GUID))
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(this, out string guid, out long file))
                    GUID = guid;
        }
#endif
    }

    [Serializable]
    public class ItemInfoReference : AssetReferenceT<ItemInfo>
    {
        public ItemInfoReference(string guid) : base(guid)
        {
        }
    }
}