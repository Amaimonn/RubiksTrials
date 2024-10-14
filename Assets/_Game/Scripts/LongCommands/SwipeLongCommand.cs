using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwipeLongCommand : RotationParameters, ILongCommand
{
    private readonly IEnumerable<CubePart> _cubeParts;
    private readonly float _speed;
    private readonly Action _onBeforeExecuteOrUndo;
    private readonly Action _onAfterExecuteOrUndo;

    public SwipeLongCommand(IEnumerable<CubePart> cubeParts, float speed, Vector3 relativeTo,
        Vector3 axis, float angle, Action onBeforeExecuteOrUndo = null, Action onAfterExecuteOrUndo = null) : 
        base( relativeTo, axis, angle)
    {
        _cubeParts = cubeParts;
        _speed = speed;
        _onBeforeExecuteOrUndo = onBeforeExecuteOrUndo;
        _onAfterExecuteOrUndo = onAfterExecuteOrUndo;
    }

    public IEnumerator Execute()
    {
        _onBeforeExecuteOrUndo?.Invoke();
        yield return DirectionalRotation(Math.Sign(_angle));
        _onAfterExecuteOrUndo?.Invoke();
    }

    public IEnumerator Undo()
    {
        _onBeforeExecuteOrUndo?.Invoke();
        yield return DirectionalRotation(-Math.Sign(_angle));
        _onAfterExecuteOrUndo?.Invoke();
    }

    private IEnumerator DirectionalRotation(int sign)
    {
        var remainAngle = Math.Abs(_angle);
        float lerpAngle;

        while (remainAngle > 0.0f)
        {
            lerpAngle = Mathf.Min(remainAngle, Time.deltaTime * _speed); // 80.0f - speed
            foreach (var cubePart in _cubeParts)
            {
                cubePart.RotateCubePart(_relativeTo, _axis, sign * lerpAngle);
            }
            remainAngle -= lerpAngle;
            yield return null;
        }
        
        foreach (var cubePart in _cubeParts)
        {
            // cubePart.RotateCubePart(_relativeTo, _axis, sign * remainAngle);
            cubePart.UpdateDirection(_relativeTo);
        }
    }
}
