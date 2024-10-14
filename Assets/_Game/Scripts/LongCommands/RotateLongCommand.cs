using System;
using System.Collections;
using UnityEngine;

public class RotateLongCommand : RotationParameters, ILongCommand
{
    private readonly CommonRubiksCube _rubiksCube;
    private readonly float _speed;
    private readonly Action _onBeforeExecuteOrUndo;
    private readonly Action _onAfterExecuteOrUndo;

    public RotateLongCommand(CommonRubiksCube rubiksCube, float speed, Vector3 relativeTo,
        Vector3 axis, float angle, Action onBeforeExecuteOrUndo = null, Action onAfterExecuteOrUndo = null) : 
        base(relativeTo, axis, angle)
    {
        _rubiksCube = rubiksCube;
        _speed = speed;
        _onBeforeExecuteOrUndo = onBeforeExecuteOrUndo;
        _onAfterExecuteOrUndo = onAfterExecuteOrUndo;
    }

    public IEnumerator Execute()
    {
        _onBeforeExecuteOrUndo?.Invoke();
        yield return RotateCube(Math.Sign(_angle));
        _onAfterExecuteOrUndo?.Invoke();
    }

    public IEnumerator Undo()
    {
        _onBeforeExecuteOrUndo?.Invoke();
        yield return RotateCube(-Math.Sign(_angle));
        _onAfterExecuteOrUndo?.Invoke();
    }

    private IEnumerator RotateCube(int sign)
    {
        var remainAngle = Math.Abs(_angle);
        float lerpAngle;

        while (remainAngle > 0.0f)
        {
            lerpAngle = Mathf.Min(remainAngle, Time.deltaTime * _speed);
            _rubiksCube.transform.RotateAround(_relativeTo, _axis, sign * lerpAngle);
            remainAngle -= lerpAngle;
            yield return null;
        }
        
        _rubiksCube.UpdatePartsDirection();
    }
}
