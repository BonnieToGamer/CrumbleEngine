namespace CrumbleEngine;

public delegate void DebugHandler();

public class Debugging
{
    public static event DebugHandler OnDebug;

    protected Debugging()
    {
        OnDebug += Debug;
    }
    
    protected virtual void Debug()
    {
        
    }

    public static void StartDebug()
    {
        OnDebug?.Invoke();
    }
}