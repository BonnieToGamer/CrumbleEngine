using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrumbleEngine.Simulation.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation;

public class SimulationMatrix
{
    public static readonly Vector2 Gravity = new(0, 9.8f);
    public static SimulationMatrix Instance { get; private set; }

    private readonly Texture2D _texture;
    private readonly Dictionary<IVector2, Chunk> _chunks;

    public SimulationMatrix(IVector2 screenSize)
    {
        Instance = this;
        _texture = new Texture2D(GameRoot.Instance.GraphicsDevice, screenSize.X, screenSize.Y);
        _chunks = new();
    }

    public void SetElement(IVector2 pos, Element element)
    {
        Chunk chunk = GetChunk(pos);
        chunk.SetElement(pos, element);
        chunk.UpdateNextFrame();
    }

    public Element GetElement(int x, int y)
    {
        return GetElement(new IVector2(x, y));
    }

    public void SwapElement(IVector2 a, IVector2 b)
    {
        GetChunk(a).MoveElement(a, b);
    }

    public Element GetElement(IVector2 pos)
    {
        return GetChunk(pos)?.GetElement(pos) ?? Element.GetElement(ElementTypes.Void);
    }

    public Chunk GetChunk(IVector2 pos)
    {
        // IVector2 chunkPos = pos / new IVector2(Chunk.Size);
        IVector2 chunkPos = new IVector2(
            pos.X < 0 ? (pos.X / Chunk.Size) - 1 : pos.X / Chunk.Size,
            pos.Y < 0 ? (pos.Y / Chunk.Size) - 1 : pos.Y / Chunk.Size
        );
        
        bool result = _chunks.TryGetValue(chunkPos, out Chunk chunk);
        if (result) return chunk;
        
        chunk = new Chunk(chunkPos);
        _chunks.Add(chunkPos, chunk);

        return chunk;
    }

    public void Update(GameTime gameTime)
    {
        _chunks.AsParallel().ForAll(x => x.Value.ApplyAndResetUpdateFlags());
        Dictionary<IVector2, Chunk> updateChunks = _chunks.Where(x => x.Value.ShouldUpdateThisFrame).ToDictionary(x => x.Key, x => x.Value);

        for (int yPattern = 0; yPattern < 2; yPattern++)
        {
            for (int xGroup = 0; xGroup < 2; xGroup++)
            {
                UpdateCycle(gameTime, updateChunks, xGroup, yPattern);
            }
        }

        foreach (Chunk chunk in updateChunks.Values)
        {
            chunk.ProcessChanges(this);
        }
    }

    private void UpdateCycle(GameTime gameTime, Dictionary<IVector2, Chunk> updateChunks, int xGroup, int yPattern)
    {
        Dictionary<IVector2, Chunk> cycleChunks = updateChunks
            .Where(x => x.Key.X % 2 == xGroup && x.Key.Y % 2 == yPattern)
            .ToDictionary(x => x.Key, x => x.Value);

        Task[] tasks = new Task[cycleChunks.Count];

        foreach (Chunk chunk in cycleChunks.Values)
        {
            chunk.Update(gameTime, this);
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 cameraLocation)
    {
        Color[] colors = new Color[_texture.Bounds.Width * _texture.Bounds.Height];
        
        Dictionary<IVector2, Chunk> renderChunks = new();
        Rectangle cameraRect = new Rectangle((int)cameraLocation.X, (int)cameraLocation.Y, _texture.Bounds.Width, _texture.Bounds.Height);
        
        foreach (IVector2 pos in _chunks.Keys)
        {
            Rectangle chunkRect = new Rectangle(pos.X * Chunk.Size, pos.Y * Chunk.Size, Chunk.Size, Chunk.Size);
            if (cameraRect.Intersects(chunkRect))
                renderChunks.Add(pos, _chunks[pos]);
        }
        
        foreach (IVector2 pos in renderChunks.Keys)
        {
            Chunk chunk = renderChunks[pos];
        
            for (int y = 0; y < Chunk.Size; y++)
            {
                for (int x = 0; x < Chunk.Size; x++)
                {
                    IVector2 elementPos = new IVector2(x, y) + pos * Chunk.Size;
                    Rectangle elementRect = new Rectangle(elementPos.X, elementPos.Y, 1, 1);
                    if (elementRect.Intersects(cameraRect) == false)
                        continue;
                    
                    Element element = GetElement(elementPos);
                    if (element == null || element.Type == ElementTypes.Void)
                    {
                        colors[elementPos.X + elementPos.Y * _texture.Bounds.Width] = chunk.ShouldUpdateNextFrame
                            ? (Color.Blue)
                            : Color.Red;
                    }
                    else
                    {
                        colors[elementPos.X + elementPos.Y * _texture.Bounds.Width] = element.RenderColor;
                    }
                }
            }
        }
        
        _texture.SetData(colors);
        spriteBatch.Draw(_texture, new Vector2(-50, -50), Color.White);

        // for (int y = 0; y < _texture.Bounds.Height; y++)
        // {
        //     for (int x = 0; x < _texture.Bounds.Width; x++)
        //     {
        //         colors[x + y * _texture.Bounds.Width] = _matrix[x, y].RenderColor;
        //     }
        // }

        // _texture.SetData(colors);
        // spriteBatch.Draw(_texture, location - (new Vector2(_texture.Bounds.Width, _texture.Bounds.Height) / 2f), Color.White);
    }

    public IVector2 GetChunkPosition(IVector2 position)
    {
        return position / new IVector2(Chunk.Size, Chunk.Size);
    }
}
