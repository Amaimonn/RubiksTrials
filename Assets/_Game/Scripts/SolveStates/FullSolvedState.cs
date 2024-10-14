using UnityEngine;

namespace Assets._Game.Scripts.SolveStates
{
    public class FullSolvedState : SolveState
    {
        public override bool Compare(CommonRubiksCube cube)
        {
            return cube.CheckIsSolved();
        }

        public override SolveState GetNextState()
        {
            Debug.LogWarning("Can`t move from finish state");
            return this;
        }
    }
}