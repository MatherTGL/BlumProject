using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GameAssets.Meta.TopPlayers
{
    public sealed class TopPlayerView : MonoBehaviour, ITopPlayer
    {
        [SerializeField, Required, BoxGroup("Parameters")]
        private TextMeshProUGUI _textName;

        [SerializeField, Required, BoxGroup("Parameters")]
        private TextMeshProUGUI _textScore;

        [SerializeField, Required, BoxGroup("Parameters")]
        private TextMeshProUGUI _textPlaceInTop;


        void ITopPlayer.Init(string name, int score, int placeInTop)
        {
            _textName.text = name;
            _textScore.text = score.ToString();
            _textPlaceInTop.text = (placeInTop + 1).ToString();
        }
    }
}
