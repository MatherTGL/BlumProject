using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GameAssets.Meta.MiniGame
{
    public sealed class SaperView : MonoBehaviour, IMiniGameView
    {
        private ISaperModel _IsaperModel = new SaperModel();

        [SerializeField, Required, BoxGroup("Parameters")]
        private SaperCell _cellPrefab;

        [ShowInInspector, ReadOnly, BoxGroup("Parameters")]
        private List<ICell> _cells = new();

        [SerializeField, Required, BoxGroup("Parameters")]
        private GameObject _root;

        [SerializeField, Required, BoxGroup("Parameters/UI")]
        private TextMeshProUGUI _textCountdown;

        GameObject IMiniGameView.prefab => gameObject;


        void IMiniGameView.Start(ref List<SpecificCell> cells, ISaperModel saperModel)
        {
            for (byte i = 0; i < cells.Count; i++)
            {
                _cells.Add(Instantiate(_cellPrefab, transform.position, Quaternion.identity, _root.transform));
                _cells[^1].Init(cells[i]);
                _cells[^1].Clicked += OnClickedCell;
            }
        }

        private void OnClickedCell(Cell.TypeCell typeCell, uint reward)
            => _IsaperModel.ClickedCell(typeCell, reward);

        void IMiniGameView.SetTimerText(double countdown)
        {
            _textCountdown.text = $"{countdown} sec";
        }

        //TODO отображать вьюшку итогов игры
        void IMiniGameView.Finish()
        {
            _cells.Clear();
            Destroy(gameObject);
        }
    }
}
