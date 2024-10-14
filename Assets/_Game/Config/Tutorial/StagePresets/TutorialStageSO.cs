using UnityEngine;
using System;

[CreateAssetMenu(fileName = "TutorialStageSO", menuName = "ScriptableObjects/Tutorial stage ")]
public class TutorialStageSO : ScriptableObject
{    
    private enum SolveStages
    {
        CrossState,
        EdgeWithMiddlePartsState,
        AllEdgesExceptOneState,
        LastEdgeWithPlacedCross,
        LastEdgeWithAccurateCross,
        LastEdgeWithPlacedAngles,
        FullSolvedState
    }

    public CommonRubiksCube RubiksCube => _rubiksCube;
    public Func<bool> CompleteCondition;

    [SerializeField] private CommonRubiksCube _rubiksCube;
    [SerializeField] private SolveStages _solveStage;
}
