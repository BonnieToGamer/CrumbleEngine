namespace MonoGame.Utilities;

public class Timer
{
    public float Length { get; private set; }
    public float Elapsed { get; private set; }

    public Timer(float length)
    {
        Length = length;
        Elapsed = 0f;
    }
    
    
    /// <summary>
    /// Updates the timer with the given delta time.
    /// </summary>
    /// <param name="delta">The time elapsed since the last update.</param>
    /// <returns>True if the timer has reached its length, false otherwise.</returns>
    public bool Update(float delta)
    {
        Elapsed += delta;

        if (Elapsed < Length) return false;
        
        Elapsed = 0;
        return true;

    }

    public void Reset()
    {
        Elapsed = 0f;
    }
}