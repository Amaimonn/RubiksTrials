using System;
using System.Linq;
using EnumPro;

namespace Assets._Game.Scripts.SolveStates
{
    public class LastEdgeWithPlacedCross : SolveState
    {
        private readonly AllEdgesExceptOneState _allEdgesExceptOneState = new();
        public override bool Compare(CommonRubiksCube cube)
        {
            foreach (PartColors partColor in Enum.GetValues(typeof(PartColors)))
            {
                if (!_allEdgesExceptOneState.Compare(cube, partColor))
                    continue;

                if (CheckLastCrossIsPlaced(cube, partColor))
                    return true;
            }
            return false;
        }

        public bool Compare(CommonRubiksCube cube, PartColors partColor)
        {
            if (_allEdgesExceptOneState.Compare(cube, partColor))
                if (CheckLastCrossIsPlaced(cube, partColor))
                        return true;
            return false;
        }

        private bool CheckLastCrossIsPlaced(CommonRubiksCube cube, PartColors oppositePartColor)
        {
            var neighborColors = cube.CubeParts
                .Where(part => part.Colors.HasFlag(oppositePartColor))
                .Select(edge => edge.Colors)
                .Aggregate((a, b) => a | b);
            PartColors crossColor = ~neighborColors & ((PartColors[])Enum.GetValues(typeof(PartColors))).Aggregate((a, b) => a | b);
            var crossParts = cube.CubeParts.Where(part => part.Colors.HasFlag(crossColor) && part.Colors.GetFlagsCount() < 3);
            var allExceptCrossAngles = cube.CubeParts.Where(part => !(part.Colors.HasFlag(crossColor) && part.Colors.GetFlagsCount() == 3));
            PartDirection commonDirection;
            foreach (PartColors partColor in Enum.GetValues(typeof(PartColors)))
            {
                if (partColor == oppositePartColor)
                {
                    continue;
                }
                // общая плоскость у частей грани, имеющей цвет partColor, за исключением угловых частей последней грани
                commonDirection = allExceptCrossAngles
                    .Where(part => part.Colors.HasFlag(partColor))
                    .Select(part => part.Direction)
                    .Aggregate((a, b) => a & b);
                if (commonDirection == 0)
                {
                    return false;
                }
            }
            //StartCoroutine(FlickerAnimation(allExceptCrossAngles));
            return true;
        }

        public override SolveState GetNextState()
        {
            return new LastEdgeWithAccurateCross();
        }
    }
}