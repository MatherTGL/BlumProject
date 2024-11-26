using System.Collections.Generic;
using System.Linq;
using GameAssets.Meta.Quests;
using UnityEngine;

namespace GameAssets.TestServer
{
    public static class TestServerController
    {
        public static readonly UserQuests userQuests = new();


        public static void BeginQuest(string guid)
        {
            if (userQuests.quests.Any(quest => quest.id == guid) == false)
            {
                userQuests.quests.Add(new UserQuest());
                userQuests.quests[^1].id = guid;
                userQuests.quests[^1].isInProgress = true;
                Debug.Log($"The quest has started successfully! GUID: {guid}");
                return;
            }

            Debug.Log($"The quest is already in progress! GUID: {guid}");
        }

        public static void TakeReward(string guid)
        {

        }

        public static void CompleteQuest(string guid)
        {
            if (userQuests.quests.Any(quest => quest.id == guid))
            {
                userQuests.quests[^1].isInProgress = false;
                userQuests.quests[^1].CanTake = false;
                userQuests.quests[^1].isDone = true;
                Debug.Log($"Quest successfully completed! GUID: {guid}");
            }
        }

        public static List<PlayerInTop> GetPlayerInTops()
        {
            return new List<PlayerInTop>()
            {
                new PlayerInTop(){
                    username = "Suchka",
                    coins = Random.Range(1, int.MaxValue)
                },
                new PlayerInTop(){
                    username = "Popa",
                    coins = Random.Range(1, int.MaxValue)
                },
                new PlayerInTop(){
                    username = "Kola",
                    coins = Random.Range(1, int.MaxValue)
                }
            };
        }

        public static List<UserReferral> GetReferrals()
        {
            return new List<UserReferral>()
            {
                new UserReferral(){
                    Id = (ulong)Random.Range(1, int.MaxValue),
                    Username = "Player",
                    income = (uint)Random.Range(1, uint.MaxValue)
                },
                new UserReferral(){
                    Id = (ulong)Random.Range(1, int.MaxValue),
                    Username = "Player1",
                    income = (uint)Random.Range(1, uint.MaxValue)
                },
                new UserReferral(){
                    Id = (ulong)Random.Range(1, int.MaxValue),
                    Username = "Player2",
                    income = (uint)Random.Range(1, uint.MaxValue)
                },
                new UserReferral(){
                    Id = (ulong)Random.Range(1, int.MaxValue),
                    Username = "Player3",
                    income = (uint)Random.Range(1, uint.MaxValue)
                },
                new UserReferral(){
                    Id = (ulong)Random.Range(1, int.MaxValue),
                    Username = "Player4",
                    income = (uint)Random.Range(1, uint.MaxValue)
                }
            };
        }
    }

    public sealed class PlayerInTop
    {
        public string username { get; set; }

        public int coins { get; set; }
    }

    public sealed class UserReferral
    {
        public ulong Id { get; set; }

        public uint income { get; set; }

        public string Username { get; set; }
    }

    public sealed class UserQuests
    {
        public List<UserQuest> quests { get; set; } = new();
    }

    public sealed class UserQuest
    {
        public string id { get; set; }

        public bool CanTake { get; set; }

        public bool isInProgress { get; set; }

        public bool isDone { get; set; }
    }
}
