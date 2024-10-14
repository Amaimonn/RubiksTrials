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
                    {
                        // показывает первую попавшуюся форму CrossEdge
                        // StartCoroutine(FlickerAnimation(GetEdgeCrossCubes(crossColor)));
                        return true;
                    }
            }

            // if (_crossState.TryGetCrossIsSolved(cube, out IEnumerable<PartColors> crossColors))
            // {
            //     foreach (var crossColor in crossColors)
            //     {
            //         // находим собранные грани с цветами, удовлетворяющими форме Cross
            //         // собранная грань + соответствие цвету грани с формой Cross дает состояние EdgeCross
            //         if (cube.CheckEdgeIsSolved(crossColor))
            //         {
            //             // показывает первую попавшуюся форму CrossEdge
            //             // StartCoroutine(FlickerAnimation(GetEdgeCrossCubes(crossColor)));
            //             return true;
            //         }
            //     }
            // }
            return false;
        }

        public override SolveState GetNextState()
        {
            return new AllEdgesExceptOneState();
        }
    }
}