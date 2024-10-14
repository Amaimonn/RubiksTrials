using System;

[Flags]
public enum PartDirection 
{
    Top = 1 << 4,
    Bottom = 1 << 5,
    North = 1 << 1,
    South = 1 << 0,
    West = 1 << 3,
    East = 1 << 2,
    None = 1 << 6,
    All = ~None
}

[Flags]
public enum PartColors 
{
   White = 1 << 0,
   Yellow = 1 << 1,
   Green = 1 << 2,
   Blue = 1 << 3,
   Orange = 1 << 4,
   Red = 1 << 5,
   // Black = 1 << 6
}

public enum SwipeDirection
{
   Up,
   Down,
   Left,
   Right,
   None
}

public enum RotateDirection
{
   Clockwise,
   CounterClockwise,
   None
}

public enum CubeTypes
{
   TwoByTwo,
   ThreeByThree
}