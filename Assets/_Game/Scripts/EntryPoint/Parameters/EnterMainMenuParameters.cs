public class EnterMainMenuParameters
{
    public int CubeIndex { get; }
    public int LevelIndex { get; }

    public EnterMainMenuParameters(int cubeIndex, int levelIndex)
    {
        CubeIndex = cubeIndex;
        LevelIndex = levelIndex;
    }
}