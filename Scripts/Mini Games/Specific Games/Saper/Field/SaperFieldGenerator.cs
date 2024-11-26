using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static GameAssets.Meta.MiniGame.Cell;
using Random = System.Random;

namespace GameAssets.Meta.MiniGame
{
    public sealed class SaperFieldGenerator
    {
        public List<SpecificCell> GetGeneratedField(List<Cell> cells)
        {
            List<SpecificCell> generatedFields = new();
            Random random = new();

            for (byte i = 0; i < cells.Count; i++)
            {
                for (ushort k = 0; k < cells[i].elementsCount; k++)
                {
                    generatedFields.Add(new SpecificCell()
                    {
                        image = cells[i].icon,
                        reward = cells[i].reward,
                        typeCell = cells[i].typeCell
                    });
                }
            }

            generatedFields = generatedFields.OrderBy(x => random.Next()).ToList();
            return generatedFields;
        }
    }

    public struct SpecificCell
    {
        public Sprite image;

        public TypeCell typeCell;

        public uint reward;
    }
}
