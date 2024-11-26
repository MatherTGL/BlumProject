using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameAssets.Meta.MiniGame
{
    [CreateAssetMenu(fileName = "SaperGameConf", menuName = "Configs/Games/Saper", order = 1)]
    public sealed class SaperConfig : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField, ToggleLeft, BoxGroup("Editor")]
        private bool _isEditCells = false;

        [SerializeField, ReadOnly, BoxGroup("Editor"), ShowIf("_isCellsError"), InfoBox("Overflowing cells or there are empty cells!", InfoMessageType.Error)]
        private bool _isCellsError;

        [SerializeField, ReadOnly, BoxGroup("Editor"), LabelText("Cells count")]
        private int _totalCells;

        [SerializeField, ReadOnly, BoxGroup("Editor"), LabelText("Field size")]
        private int _totalFieldSize;
#endif

        [field: SerializeField, Required, BoxGroup("Parameters/UI"), PropertyOrder(-2), PropertySpace(0, 10), AssetsOnly, PreviewField]
        public Sprite background { get; private set; }

        [field: SerializeField, Required, BoxGroup("Parameters/UI"), PropertyOrder(-3), AssetsOnly, PreviewField]
        public Sprite bombSprite { get; private set; }

        [field: SerializeField, Required, BoxGroup("Parameters/UI"), PropertyOrder(-4), AssetsOnly, PreviewField]
        public Sprite prizeSprite { get; private set; }

        [field: SerializeField, BoxGroup("Parameters"), PropertyOrder(-1)]
        public ushort ticketCost { get; private set; } = 1;

        [field: SerializeField, BoxGroup("Parameters"), Required, PropertyOrder(2), PropertySpace(10, 0), EnableIf("_isEditCells")]
        public List<Cell> cells { get; private set; } = new();

        [field: SerializeField, BoxGroup("Parameters"), PropertyOrder(0), EnableIf("_isEditCells")]
        public Vector2 fieldSize { get; private set; } = new Vector2(6, 6);

        [field: SerializeField, BoxGroup("Parameters"), SuffixLabel("sec"), LabelText("Play Time"), PropertyOrder(1)]
        public byte playTimeInSeconds { get; private set; } = 30;


#if UNITY_EDITOR
        private void OnValidate()
        {
            CalculateTotalCells();
            SetSpritesToCells();
        }

        private void SetSpritesToCells()
        {
            for (byte i = 0; i < cells.Count; i++)
            {
                var cell = cells[i];

                if (cells[i].typeCell == Cell.TypeCell.Bomb)
                    cell.icon = bombSprite;
                else
                    cell.icon = prizeSprite;

                cells[i] = cell;
            }
        }

        private void CalculateTotalCells()
        {
            _totalCells = cells.Sum(item => item.elementsCount);
            _totalFieldSize = (int)(fieldSize.x * fieldSize.y);

            if (_totalCells > _totalFieldSize)
            {
                Debug.LogError("There are more planned elements than the number of cells on the field.");
                _isCellsError = true;
            }
            else if (_totalCells == _totalFieldSize)
            {
                _isCellsError = false;
            }
            else
            {
                Debug.LogError("There are more planned elements than the number of cells on the field.");
                _isCellsError = true;
            }
        }
#endif
    }

    [Serializable]
    public struct Cell
    {
        public enum TypeCell : byte
        {
            Bomb, Prize
        }

        [EnumToggleButtons]
        public TypeCell typeCell;

        [ReadOnly]
        public Sprite icon;

        [EnableIf("@typeCell != TypeCell.Bomb")]
        public ushort reward;

        [MinValue(1)]
        public ushort elementsCount;
    }
}
