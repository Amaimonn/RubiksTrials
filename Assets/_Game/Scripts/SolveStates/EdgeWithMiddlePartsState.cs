using System;

namespace Assets._Game.Scripts.SolveStates
{
    public class EdgeWithMiddlePartsState : SolveState
    {
        private readonly CrossState _crossState = new ();
        public override bool Compare(CommonRubiksCube cube)
        {
            foreach (PartColors color in Enum.GetValues(typeof(PartColors)))
            {
                if (!_crossState.Compare(cube, color))
                    continue;

                if (cube.CheckEdgeIsSolved(color))
                    return true;
            }

            return false;
        }

        public override SolveState GetNextState()
        {
            return new AllEdgesExceptOneState();
        }
    }
}