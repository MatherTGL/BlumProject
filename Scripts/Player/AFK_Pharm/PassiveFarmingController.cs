using System;

namespace GameAssets.Player.Pharm
{
    public sealed class PassiveFarmingController
    {
        public static PassiveFarmingController instance { get; } = new();

        public IPassiveModel Imodel { get; }


        private PassiveFarmingController()
            => Imodel = new PassiveFarmingModel(this);
    }
}
