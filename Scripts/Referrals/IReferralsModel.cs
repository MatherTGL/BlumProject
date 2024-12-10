using System;
using System.Collections.Generic;
using GameAssets.Meta.Quests;
using PlayFab.ClientModels;

namespace GameAssets.Meta.Referrals
{
    public interface IReferralsModel
    {
        Action<BaseQuest.TypeQuest, BaseQuest.JobSubtype> OnFriendsUpdated { get; set; }
        
        void Init();
        
        List<FriendInfo> GetReferralList();
    }
}
