using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameAssets.Meta.MiniGame
{
    [RequireComponent(typeof(Image))]
    public sealed class SaperCell : MonoBehaviour, ICell
    {
        public Action<Cell.TypeCell, uint> Clicked { get; set; }

        private SpecificCell _specificCell;

        private bool _isOpen;


        void ICell.Init(SpecificCell specificCell)
        {
            //TODO установить данные клетке (image например)
            _specificCell = specificCell;
        }

        [Button("Open Cell"), BoxGroup("Control")]
        public void OpenCell()
        {
            if (_isOpen)
            {
                Debug.Log("Клетка уже открыта");
                return;
            }

            Debug.Log($"Игрок нажал на клетку {_specificCell.typeCell} / reward: {_specificCell.reward}");
            _isOpen = true;
            Clicked?.Invoke(_specificCell.typeCell, _specificCell.reward);
            Debug.Log("Clicked invoked!");
        }
    }
}
