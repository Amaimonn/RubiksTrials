using UnityEngine;

public abstract class RotationParameters
{
    protected Vector3 _relativeTo;
    protected Vector3 _axis;
    protected readonly float _angle;

    public RotationParameters(Vector3 relativeTo, Vector3 axis, float angle)
    {
        _relativeTo = relativeTo;
        _axis = axis;
        _angle = angle;
    }
}
