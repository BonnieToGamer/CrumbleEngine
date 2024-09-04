using MonoGame.Utilities;

namespace CrumbleEngine.Simulation;

public struct ElementChange(Chunk sourceChunk, IVector2 position, IVector2 newPosition)
{
    public Chunk sourceChunk = sourceChunk;
    public IVector2 position = position;
    public IVector2 newPosition = newPosition;
}