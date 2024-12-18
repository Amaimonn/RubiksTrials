using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CubePart : MonoBehaviour
{
    public PartColors Colors { get => _colors; }
    public PartDirection Direction;
    public bool IsBusy;
    public Vector3 InitialDirection => _initialDirection;
    public Vector3 DirectionX => transform.right.normalized * InitialDirection.x;
    public Vector3 DirectionY => transform.up.normalized * InitialDirection.y;
    public Vector3 DirectionZ => transform.forward.normalized * InitialDirection.z;
    public Vector3 Center => _renderer.bounds.center;

    [SerializeField] private PartColors _colors;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Vector3 _initialDirection;


    // private void OnValidate()
    // {
    //     // _renderer = GetComponent<Renderer>();
    //     UpdateDirection(new Vector3(0, 8.8f, 0));
    //     var initAxis = AxisFromPartDirection(Direction);
    //         // initAxis.x *= _directionCorrection.x;
    //         // initAxis.y *= _directionCorrection.y;
    //         // initAxis.z *= _directionCorrection.z;
    //     _initialDirection = initAxis;

    // }

    public void UpdateDirection(Vector3 relativeTo)
    {
        var axis = (Center - relativeTo).normalized;
        Direction = PartDirectionFromAxis(axis);
    }

    public void RoundPosition()
    {
        var newPosition = transform.position;

        newPosition.x = Mathf.Round(newPosition.x * 10f) / 10f;
        newPosition.y = Mathf.Round(newPosition.y * 10f) / 10f;
        newPosition.z = Mathf.Round(newPosition.z * 10f) / 10f;

        transform.position = newPosition;
    }

    public void RotateCubePart(Vector3 relativeTo, Vector3 axis, float angle)
    {
        transform.RotateAround(relativeTo, axis, angle);
    }

    public PartDirection PartDirectionFromAxis(Vector3 axis)
    {
        byte newDirection = 0;

        if (axis.x > 0.1f)
            newDirection += (byte)PartDirection.East;
        else if (axis.x < -0.1f)
            newDirection += (byte)PartDirection.West;

        if (axis.y > 0.1f)
            newDirection += (byte)PartDirection.Top;
        else if (axis.y < -0.1f)
            newDirection += (byte)PartDirection.Bottom;
        
        if (axis.z > 0.1f)
            newDirection += (byte)PartDirection.North;
        else if (axis.z < -0.1f)
            newDirection += (byte)PartDirection.South;

        return (PartDirection)newDirection;
    }

    public Vector3 AxisFromPartDirection(PartDirection partDirection)
    {
        Vector3 axis = Vector3.zero;
        if (partDirection.HasFlag(PartDirection.East))
            axis.x = 1.0f;
        else if (partDirection.HasFlag(PartDirection.West))
            axis.x = -1.0f;

        if (partDirection.HasFlag(PartDirection.Top))
            axis.y = 1.0f;
        else if (partDirection.HasFlag(PartDirection.Bottom))
            axis.y = -1.0f;

        if (partDirection.HasFlag(PartDirection.North))
            axis.z = 1.0f;
        else if (partDirection.HasFlag(PartDirection.South))
            axis.z = -1.0f;
        return axis;
    }

    // какой face из трёх смотрит в направлении partDirection
    public Vector3 CompareDirections(PartDirection partDirection)
    {
        Vector3 result = Vector3.zero;
        switch (partDirection)
        {
            case PartDirection.East:
            {
                if (DirectionX.x > 0.1f)
                    result += Vector3.right;
                else if (DirectionY.x > 0.1f)
                    result += Vector3.up; 
                else if (DirectionZ.x > 0.1f)
                    result += Vector3.forward;
                break;
            }

            case PartDirection.West:
            {
                if (DirectionX.x < -0.1f)
                    result += Vector3.right;
                else if (DirectionY.x < -0.1f)
                    result += Vector3.up; 
                else if (DirectionZ.x < -0.1f)
                    result += Vector3.forward;
                break;
            }

            case PartDirection.North:
            {
                if (DirectionX.z > 0.1f)
                    result += Vector3.right;
                else if (DirectionY.z > 0.1f)
                    result += Vector3.up; 
                else if (DirectionZ.z > 0.1f)
                    result += Vector3.forward;
                break;
            }

            case PartDirection.South:
            {
                if (DirectionX.z < -0.1f)
                    result += Vector3.right;
                else if (DirectionY.z < -0.1f)
                    result += Vector3.up; 
                else if (DirectionZ.z < -0.1f)
                    result += Vector3.forward;
                break;
            }

            case PartDirection.Top:
            {
                if (DirectionX.y > 0.1f)
                    result += Vector3.right;
                else if (DirectionY.y > 0.1f)
                    result += Vector3.up; 
                else if (DirectionZ.y > 0.1f)
                    result += Vector3.forward;
                break;
            }

            case PartDirection.Bottom:
            {
                if (DirectionX.y < -0.1f)
                    result += Vector3.right;
                else if (DirectionY.y < -0.1f)
                    result += Vector3.up; 
                else if (DirectionZ.y < -0.1f)
                    result += Vector3.forward;
                break;
            }
        }
        return result;
    }

    public void SetVisibility(bool isVisible)
    {
        _renderer.enabled = isVisible;
        float alphaChannel = isVisible ? 1.0f : 0.0f;
        _renderer.material.SetColor("_Color", new Color (_renderer.material.color.r, _renderer.material.color.g, 
            _renderer.material.color.b, alphaChannel));
    }

#region MonoBehaviour
    private void Awake()
    {
        IsBusy = false;
    }
#endregion

    private void OnDrawGizmos()
    {
        Debug.DrawLine(Center, Center + DirectionX * 10.0f, Color.red);
        Debug.DrawLine(Center, Center + DirectionY * 10.0f, Color.green);
        Debug.DrawLine(Center, Center + DirectionZ * 10.0f, Color.blue);
    }
}

