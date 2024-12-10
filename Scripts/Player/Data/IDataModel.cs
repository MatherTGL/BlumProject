using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameAssets.Meta.Quests;
using GameAssets.Player.Data.Config;
using UnityEngine;

namespace GameAssets.Player.Data
{
    public interface IDataModel
    {
        float coins { get; protected set; }

        ushort tickets { get; protected set; }

        Action<BaseQuest.TypeQuest, BaseQuest.JobSubtype> UpdatedData { get; set; }


        void InitAsync();

        void Update();

        void AddCoins(float amount)
        {
            coins += amount;
            Debug.Log($"coins: {coins}");
            Update();
        }

        bool IsSpendCoins(float amount, bool spend)
        {
            if (coins >= amount)
            {
                if (spend)
                {
                    coins -= amount;
                    Update();
                }
                return true;
            }

            return false;
        }

        bool IsSpendTickets(ushort amount, bool spend)
        {
            if (tickets >= amount)
            {
                if (spend)
                {
                    tickets -= amount;
                    Debug.Log($"tickets: {tickets}");
                    Update();
                }
                return true;
            }

            return false;
        }

        void AddTickets(ushort amount)
        {
            tickets += amount;
            Update();
        }

        UniTask<float> GetCoinsAsync();

        UniTask<ConfigDataPlayer> GetConfigAsync();
    }
}
