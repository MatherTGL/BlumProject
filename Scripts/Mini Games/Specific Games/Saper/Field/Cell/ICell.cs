using System;
using static GameAssets.Meta.MiniGame.Cell;

namespace GameAssets.Meta.MiniGame
{
    public interface ICell
    {
        Action<TypeCell, uint> Clicked { get; set; }


        void Init(SpecificCell specificCell);
    }
}
