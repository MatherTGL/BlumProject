using System;

namespace GameAssets.Player.Pharm
{
    public interface IPassiveModel
    {
        Action UpdatedData { get; set; }


        void StartPharm();
    }
}
