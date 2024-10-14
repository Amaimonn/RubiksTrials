using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets._Game.Scripts.SolveStates
{
    public class AllEdgesExceptOneState : SolveState
    {
        public override bool Compare(CommonRubiksCube cube)
        {
            foreach (PartColors partColor in Enum.GetValues(typeof(PartColors)))
            {
                if (CheckExceptOneIsSolved(cube, partColor))
                    return true;
            }

            return false;
        }

        public bool Compare(CommonRubiksCube cube, PartColors partColor)
        {
            return CheckExceptOneIsSolved(cube, partColor);
        }

        public override SolveState GetNextState()
        {
            return new LastEdgeWithPlacedCross();
        }

        private bool CheckExceptOneIsSolved(CommonRubiksCube cube, PartColors oppositePartColor)
        {
            var neighborColors = cube.CubeParts
                .Where(part => part.Colors
                .HasFlag(oppositePartColor))
                .Select(edge => edge.Colors)
                .Aggregate((a, b) => a | b);
            // все части, не принадлежащие оставшейся грани
            var checkParts = cube.CubeParts.Where(part => neighborColors.HasFlag(part.Colors));
            IEnumerable<CubePart> partsWithCommonColor;
            foreach (PartColors partColor in Enum.GetValues(typeof(PartColors)))
            {
                if (!neighborColors.HasFlag(partColor))
                { 
                    continue; 
                }

                partsWithCommonColor = checkParts.Where(part => part.Colors.HasFlag(partColor));
                if (!cube.CheckPartsHasEqualDirections(partsWithCommonColor))
                {
                    return false;
                }
            }
            // StartCoroutine(FlickerAnimation(checkParts));
            return true;
        }
    }
}