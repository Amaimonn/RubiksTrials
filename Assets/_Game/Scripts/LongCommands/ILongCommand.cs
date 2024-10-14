using System.Collections;

public interface ILongCommand
{
    public IEnumerator Execute();
    public IEnumerator Undo();
}
