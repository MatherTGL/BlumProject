using System.Collections.Generic;
using PlayFab.ClientModels;

namespace GameAssets.Meta.Referrals
{
    public interface IReferralsModel
    {
        List<FriendInfo> GetReferralList();
    }
}
