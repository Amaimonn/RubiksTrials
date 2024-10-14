using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCommand : RotationParameters, ICommand
{
    private readonly IEnumerable<CubePart> _cubeParts;

    public RotateCommand(IEnumerable<CubePart> cubeParts, float angle, Vector3 relativeTo, Vector3 axis) : 
        base(relativeTo, axis, angle)
    {
        _cubeParts = cubeParts;
    }

    public void Execute()
    {
        DirectionalRotation();
    }

    public void Undo()
    {
        DirectionalRotation(true);
    }

    public IEnumerator SmoothRotate(Action onFinished = null)
    {
        // var angle = _direction switch
        // {
        //     RotateDirection.Clockwise => 90.0f,
        //     RotateDirection.CounterClockwise => -90.0f,
        //     RotateDirection.None => 0,
        //     _ => 0
        // };
        var sign = _angle < 0? -1 : 1;
        var remainAngle = Math.Abs(_angle);

        while (remainAngle > 0.0f)
        {
            var angleLerp = Time.deltaTime * 80.0f; // 80.0f - speed
            foreach (var cubePart in _cubeParts)
            {
                //cubePart.Rotate(_direction);
                cubePart.RotateCubePart(_relativeTo, _axis, sign * angleLerp);
                // Debug.Log($"{cubePart} rotated {_direction}");
            }
            remainAngle -= angleLerp;
            yield return null;
        }
        
        foreach (var cubePart in _cubeParts)
        {
            cubePart.RotateCubePart(_relativeTo, _axis, sign * remainAngle);
            cubePart.UpdateDirection(_relativeTo);
        }

        onFinished?.Invoke();
    }

    private void DirectionalRotation(bool isReversed = false)
    {
        foreach (var cubePart in _cubeParts)
        {
            var angle = isReversed ? -_angle : _angle;

            cubePart.RotateCubePart(_relativeTo, _axis, angle);
            cubePart.UpdateDirection(_relativeTo);
        }
    }
}
