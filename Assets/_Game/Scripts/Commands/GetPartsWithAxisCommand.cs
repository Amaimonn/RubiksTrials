using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GetPartsWithAxisCommand : IGetPartsCommand
{
    private Vector3 _axis;

    public GetPartsWithAxisCommand(Vector3 axis)
    {
        _axis = axis;
    }

    public IEnumerable<CubePart> GetParts(IEnumerable<CubePart> allParts)
    {
        IEnumerable<CubePart> definedParts = null;
        if (allParts != null && allParts.Count() != 0)
        {
            PartDirection partDirection = allParts.First().PartDirectionFromAxis(_axis); //CubePart.PartDirectionFromAxis(c.GetLocalPartDirection(axis)
            definedParts = allParts.Where(c => (partDirection & c.Direction) != 0);
        }

        return definedParts;
    }
}