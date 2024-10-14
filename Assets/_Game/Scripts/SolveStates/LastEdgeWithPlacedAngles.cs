using System;
using System.Linq;
using EnumPro;

namespace Assets._Game.Scripts.SolveStates
{
    public class LastEdgeWithPlacedAngles : SolveState
    {
        private readonly LastEdgeWithAccurateCross _lastEdgeWithAccurateCross = new();
        public override bool Compare(CommonRubiksCube cube)
        {
            foreach (PartColors partColor in Enum.GetValues(typeof(PartColors)))
            {
                if (!_lastEdgeWithAccurateCross.Compare(cube, partColor))
                    continue;
                if (CheckLastAnglesArePlaced(cube, partColor))
                    return true;
            }
            return false;
        }

        public override SolveState GetNextState()
        {
            return new FullSolvedState();
        }

        private bool CheckLastAnglesArePlaced(CommonRubiksCube cube, PartColors oppositePartColor)
        {
            //TODO: проверить, что все углы возле креста лежат в своих плоскостях
            var neighborColors = cube.CubeParts
                .Where(part => part.Colors.HasFlag(oppositePartColor))
                .Select(edge => edge.Colors)
                .Aggregate((a, b) => a | b);
            PartColors crossColor = ~neighborColors & ((PartColors[])Enum.GetValues(typeof(PartColors))).Aggregate((a, b) => a | b);

            var angleCrossParts = cube.CubeParts.Where(part => part.Colors.HasFlag(crossColor) && part.Colors.GetFlagsCount() == 3);
            
            var allExceptCross = cube.CubeParts.Where(part => !(part.Colors.HasFlag(crossColor) && part.Colors.GetFlagsCount() < 3));
            PartDirection commonDirection;
            foreach (PartColors partColor in Enum.GetValues(typeof(PartColors)))
            {
                if (partColor == oppositePartColor)
                {
                    continue;
                }
                // общая плоскость у частей грани, имеющей цвет partColor, за исключением частей креста
                commonDirection = allExceptCross
                    .Where(part => part.Colors.HasFlag(partColor))
                    .Select(part => part.Direction)
                    .Aggregate((a, b) => a & b);
                if (commonDirection == 0)
                {
                    return false;
                }
            }
            // StartCoroutine(FlickerAnimation(allExceptCross));
            return true;
        }
    }
}