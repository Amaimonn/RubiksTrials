
namespace Assets._Game.Scripts.SolveStates
{
    public abstract class SolveState
    {
        public abstract bool Compare(CommonRubiksCube cube);
        public abstract SolveState GetNextState();
    }
}