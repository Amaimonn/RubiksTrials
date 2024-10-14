using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetPartsOnPlaneCommand : IGetPartsCommand
{
    private Plane _plane;

    public GetPartsOnPlaneCommand(Plane plane)
    {
        _plane = plane;
    }

    public IEnumerable<CubePart> GetParts(IEnumerable<CubePart> allParts)
    {
        var definedParts = allParts.Where(c => Mathf.Abs(_plane.GetDistanceToPoint(c.transform.position)) <= 0.5f);
        return definedParts;
    }
}