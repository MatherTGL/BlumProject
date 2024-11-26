using System.Collections.Generic;
using UnityEngine;

namespace GameAssets.Meta.MiniGame
{
    public interface IMiniGameView
    {
        GameObject prefab { get; }


//TODO абстрагировать ISaperModel 
        void Start(ref List<SpecificCell> cells, ISaperModel saperModel);

        void SetTimerText(double countdown);

        void Finish();
    }
}
