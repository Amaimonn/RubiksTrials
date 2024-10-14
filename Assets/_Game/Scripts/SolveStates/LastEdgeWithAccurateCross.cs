using System;
using System.Collections.Generic;
using System.Linq;
using EnumPro;

namespace Assets._Game.Scripts.SolveStates
{
    public class LastEdgeWithAccurateCross : SolveState
    {
        private readonly LastEdgeWithPlacedCross _lastEdgeWithPlacedCross = new();
        public override bool Compare(CommonRubiksCube cube)
        {
            foreach (PartColors partColor in Enum.GetValues(typeof(PartColors)))
            {
                if (!_lastEdgeWithPlacedCross.Compare(cube, partColor))
                    continue;
                if (CheckCrossIsAccurate(cube, partColor))
                    return true;
            }
            return false;
        }

        public override SolveState GetNextState()
        {
            return new LastEdgeWithPlacedAngles();
        }

        public bool Compare(CommonRubiksCube cube, PartColors oppositePartColor)
        {
            if (_lastEdgeWithPlacedCross.Compare(cube, oppositePartColor))
                if (CheckCrossIsAccurate(cube, oppositePartColor))
                    return true;
            return false;
        }
        
        private bool CheckCrossIsAccurate(CommonRubiksCube cube, PartColors oppositePartColor)
        {
            var neighborColors = cube.CubeParts
                .Where(part => part.Colors.HasFlag(oppositePartColor))
                .Select(edge => edge.Colors)
                .Aggregate((a, b) => a | b);
            PartColors crossColor = ~neighborColors & ((PartColors[])Enum.GetValues(typeof(PartColors))).Aggregate((a, b) => a | b);

            var crossParts = cube.CubeParts.Where(part => part.Colors.HasFlag(crossColor) && part.Colors.GetFlagsCount() < 3);
            var allExceptCrossAngles = cube.CubeParts.Where(part => !(part.Colors.HasFlag(crossColor) && part.Colors.GetFlagsCount() == 3));
            IEnumerable<CubePart> oneColorParts;
            foreach (PartColors partColor in Enum.GetValues(typeof(PartColors)))
            {
                if (partColor == oppositePartColor)
                {
                    continue;
                }
                
                oneColorParts = allExceptCrossAngles.Where(part => part.Colors.HasFlag(partColor));
                // проверка на совпадение цветов на грани за исключением углов возле креста
                // CheckPartsHasEqualDirections(oneColorParts);
                if (!oneColorParts
                    .All((part) => cube.CheckPartsHasCommonDirection(part, oneColorParts.Where(part => part.Colors.GetFlagsCount()==1).First())))
                {
                    return false;
                }
            }
            // StartCoroutine(FlickerAnimation(allExceptCrossAngles));
            return true;
        }
    }
}