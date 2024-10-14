using System;
using System.Collections.Generic;
using System.Linq;
using EnumPro;

namespace Assets._Game.Scripts.SolveStates
{
    public class CrossState : SolveState
    {
        public override bool Compare(CommonRubiksCube cube)
        {
            foreach (PartColors color in Enum.GetValues(typeof(PartColors)))
            {
                if (CheckCrossIsSolved(cube, color))
                    return true;
            }
            return false;
        }

        public override SolveState GetNextState()
        {
            return new EdgeWithMiddlePartsState();
        }

        public bool Compare(CommonRubiksCube cube, PartColors partColor)
        {
            return CheckCrossIsSolved(cube, partColor);
        }
        
        public bool TryGetCrossIsSolved(CommonRubiksCube cube, out IEnumerable<PartColors> partColorsWithCross)
        {
            var partColors = new List<PartColors>();
            foreach (PartColors color in Enum.GetValues(typeof(PartColors)))
            {
                if (CheckCrossIsSolved(cube, color))
                {
                    partColors.Add(color);
                }
            }
            partColorsWithCross = partColors;
            return partColors.Count() > 0;
        }

        private bool CheckCrossIsSolved(CommonRubiksCube cube, PartColors partColor)
        {   
            // все кубики цвета color, кроме угловых
            // oneCrossParts = new();
            var crossParts = cube.CubeParts.Where(part => part.Colors.HasFlag(partColor) && part.Colors.GetFlagsCount() < 3);
            // если части только угловые, то Cross конфигурации нет
            if (crossParts.Count() == 0)
                return true;
            // проверка на плюс стороны цвета color (без учета боковых)
            if (!cube.CheckPartsHasCommonDirection(crossParts))
            {
                return false;
            }
            // боковые (двухцветные) части плюса
            var twoColoredParts = crossParts.Where(part => part.Colors.GetFlagsCount() > 1);
            CubePart oneColoredPart;

            // проверка на соответствие цветов боковых частей
            bool isCross = true;
            // oneCrossParts.AddRange(crossParts);
            foreach (var twoColoredPart in twoColoredParts)
            {
                // серединка, того же цвета, что и боковая часть twoColoredPart
                oneColoredPart = cube.CubeParts
                    .Where(part => twoColoredPart.Colors.HasFlag(part.Colors) 
                        && !part.Colors.HasFlag(partColor) && part.Colors.GetFlagsCount() == 1)
                    .First();
                if (!cube.CheckPartsHasCommonDirection(twoColoredPart, oneColoredPart))
                {
                    isCross = false;
                    break;
                }
                //oneCrossParts.Add(oneColoredPart);
            }
            return isCross;
        }
    }
}