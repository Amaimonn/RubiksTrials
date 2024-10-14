using System.Collections.Generic;

public interface IGetPartsCommand
{
    public IEnumerable<CubePart> GetParts(IEnumerable<CubePart> allParts);
}