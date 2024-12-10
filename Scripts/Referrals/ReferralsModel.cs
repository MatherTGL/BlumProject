using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameAssets.General.Server;
using GameAssets.Meta.Quests;
using GameAssets.Scripts.Utils.DeepLinking;
using GameAssets.System.SaveSystem;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameAssets.Meta.Referrals
{
    public sealed class ReferralsModel : IReferralsModel
    {
        private List<FriendInfo> _friends = new();
        
        public Action<BaseQuest.TypeQuest, BaseQuest.JobSubtype> OnFriendsUpdated { get; set; }


        private async void GetServerData(List<FriendInfo> userReferrals)
        {
            var config = await LoadConfigAsync();
            await (config as ISaveable<ReferralsConfig>).AsyncLoad();
            
            _friends = await PlayfabController.GetFriendsListAsync();
            Debug.Log($"{userReferrals.Count} user referrals");

            if (_friends.Count > config.referralsCount)
            {
                Debug.Log("Добавлены новые рефералы!");
                OnFriendsUpdated?.Invoke(BaseQuest.TypeQuest.Collect, BaseQuest.JobSubtype.Friends);
            }
            
            config.referralsCount = _friends.Count;
            
            (config as ISaveable<ReferralsConfig>).Save();
            Addressables.Release(config);
            
            SortReferralsList(ref userReferrals);
        }

        private async UniTask<ReferralsConfig> LoadConfigAsync() 
            => await Addressables.LoadAssetAsync<ReferralsConfig>("ReferralConfig").Task;

        private void SortReferralsList(ref List<FriendInfo> userReferrals)
        {
            userReferrals = userReferrals.OrderByDescending(item => item.Username).ToList();
        }

        private List<FriendInfo> GetSortedReferrals()
        {
            GetServerData(_friends);
            return _friends;
        }

        private void CheckDeepLink()
        {
            if (DeepLinkChecker.HasDeepLinkArgument())
            {
                string friendID = DeepLinkChecker.GetDeepLinkArgument();
                Debug.Log($"friendID: {friendID}");
            }
            else
            {
                Debug.Log("Игрок зашел без реферальной ссылки");
            }
        }
        
        void IReferralsModel.Init() => CheckDeepLink();

        List<FriendInfo> IReferralsModel.GetReferralList() => GetSortedReferrals();
    }
}
