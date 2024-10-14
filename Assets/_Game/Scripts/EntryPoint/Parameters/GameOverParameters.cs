using System;

public class GameOverViewParameters
{
    public string Message { get; }
    public Action ExitCallback { get; }

    public GameOverViewParameters(string message, Action exitCallback)
    {
        Message = message;
        ExitCallback = exitCallback;
    }
}