namespace GameAssets.Meta.Referrals
{
    public static class ReferralsController
    {
        public static IReferralsModel Imodel { get; } = new ReferralsModel();
    }
}
