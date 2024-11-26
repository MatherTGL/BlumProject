using System.Collections.Generic;
using System.Linq;
using GameAssets.General.Server;
using PlayFab.ClientModels;
using UnityEngine;

namespace GameAssets.Meta.Referrals
{
    public sealed class ReferralsModel : IReferralsModel
    {
        private List<FriendInfo> _tempFriends = new();


        private async void GetServerData(List<FriendInfo> userReferrals)
        {
            _tempFriends = await PlayfabController.GetFriendsListAsync();
            Debug.Log($"{userReferrals.Count} user referrals");

            SortReferralsList(ref userReferrals);
        }

        private void SortReferralsList(ref List<FriendInfo> userReferrals)
        {
            userReferrals = userReferrals.OrderByDescending(item => item.Username).ToList();
        }

        private List<FriendInfo> GetSortedReferrals()
        {
            GetServerData(_tempFriends);
            return _tempFriends;
        }

        List<FriendInfo> IReferralsModel.GetReferralList() => GetSortedReferrals();
    }
}
