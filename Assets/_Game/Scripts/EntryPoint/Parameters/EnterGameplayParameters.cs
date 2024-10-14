public class EnterGameplayParameters
{
    public int CubeIndex { get; }
    public int LevelIndex { get; }

    public EnterGameplayParameters(int cubeIndex, int levelIndex)
    {
        CubeIndex = cubeIndex;
        LevelIndex = levelIndex;
    }
}