using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CrumbleEngine.Simulation.Elements;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation;

public class World : Debugging
{
    public static readonly Vector2 Gravity = new(0, 9.8f);
    public static World Instance { get; private set; }

    private readonly Texture2D _texture;
    private readonly Chunk[,] _chunks;
    private int _chunkAccessCounter = 0;

    public World(IVector2 screenSize)
    {
        Instance = this;
        _texture = new Texture2D(GameRoot.Instance.GraphicsDevice, 400, 400);
        _chunks = new Chunk[8,8]; // HACK: This is an arbitrary number
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                _chunks[x, y] = new Chunk(new IVector2(x, y));
            }
        }
    }

    public void SetElement(IVector2 pos, Element element)
    {
        Chunk chunk = GetChunk(pos);
        chunk?.SetElement(pos, element);
        chunk?.UpdateNextFrame();
    }

    public Element GetElement(int x, int y)
    {
        return GetElement(new IVector2(x, y));
    }

    public bool SwapElement(IVector2 a, IVector2 b)
    {
        Chunk chunkA = GetChunk(a);
        Chunk chunkB = GetChunk(b);
        if (chunkA == null || chunkB == null) return false;
        
        Element temp = chunkA.GetElement(a);
        
        chunkA.SetElement(a, chunkB.GetElement(b));
        chunkB.SetElement(b, temp);

        void UpdateNeighbourChunk(IVector2 pos)
        {
            int x = pos.X % Chunk.Size;
            int y = pos.Y % Chunk.Size;
            if (x == 0 || x == Chunk.Size - 1) 
                GetChunk(pos + new IVector2(x == 0 ? -1 : 1, 0))?.UpdateNextFrame();
            if (y == 0 || y == Chunk.Size - 1) 
                GetChunk(pos + new IVector2(0, y == 0 ? -1 : 1))?.UpdateNextFrame();
        }

        UpdateNeighbourChunk(a);
        return true;
    }

    public Element GetElement(IVector2 pos)
    {
        return GetChunk(pos, false)?.GetElement(pos) ?? Element.GetElement(ElementTypes.Void);
    }

    private Chunk GetChunk(IVector2 pos, bool createChunk = true)
    {
        _chunkAccessCounter++;
        // IVector2 chunkPos = pos / new IVector2(Chunk.Size);
        IVector2 chunkPos = new IVector2(
            pos.X < 0 ? (pos.X / Chunk.Size) - 1 : pos.X / Chunk.Size,
            pos.Y < 0 ? (pos.Y / Chunk.Size) - 1 : pos.Y / Chunk.Size
        );

        if (chunkPos.X is >= 0 and < 8 && chunkPos.Y is >= 0 and < 8)
            return _chunks[chunkPos.X, chunkPos.Y];
        
        // bool result = _chunks.TryGetValue(chunkPos, out Chunk chunk);
        // if (result) return chunk;
        // if (createChunk == false) return null;
        // if (pos.Y > 256 || pos.Y < 0 || pos.X < 0 || pos.X > 256) return null;
        //
        // chunk = new Chunk(chunkPos);
        // _chunks.Add(chunkPos, chunk);

        return null;
    }

    // private int offsetX = 0;
    // private int offsetY = 0;
    
    public void Update(GameTime gameTime)
    {
        _chunkAccessCounter = 0;
        // _chunks.AsParallel().ForAll(x => x.Value.ApplyAndResetUpdateFlags());
        foreach (Chunk chunk in _chunks)
            chunk.ApplyAndResetUpdateFlags();
        
        List<Chunk> updateChunksList = new List<Chunk>();

        // Iterate through the 2D array
        for (int x = 0; x < _chunks.GetLength(0); x++)
        {
            for (int y = 0; y < _chunks.GetLength(1); y++)
            {
                Chunk chunk = _chunks[x, y];
                if (chunk.ShouldUpdateThisFrame) // Assuming Chunk has this property
                {
                    updateChunksList.Add(chunk);
                }
            }
        }

        int startPosX = Random.Shared.Next(0, 2);
        int endPosX = startPosX == 0 ? 2 : -1;
        int addX = startPosX == 0 ? 1 : -1;

        for (int offsetY = -1; offsetY < 1; offsetY += 1)
        {
            for (int offsetX = startPosX; offsetX != endPosX; offsetX += addX)
            {
                foreach (Chunk chunk in updateChunksList)
                    chunk.Update(gameTime, this, offsetX, offsetY);
            }
        }
        
        // foreach (KeyValuePair<IVector2, Chunk> kvp in updateChunks)
        //     kvp.Value.Update(gameTime, this, offsetX, offsetY);
        //
        // offsetX++;
        // if (offsetX >= 2)
        // {
        //     offsetX = 0;
        //     offsetY++;
        //     if (offsetY >= 2)
        //         offsetY = 0;
        // }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 cameraLocation)
    {
        Color[] colors = new Color[_texture.Bounds.Width * _texture.Bounds.Height];
        
        Dictionary<IVector2, Chunk> renderChunks = new();
        Rectangle cameraRect = new Rectangle((int)cameraLocation.X, (int)cameraLocation.Y, _texture.Bounds.Width, _texture.Bounds.Height);

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                IVector2 pos = new(x, y);
                Rectangle chunkRect = new Rectangle(pos.X * Chunk.Size, pos.Y * Chunk.Size, Chunk.Size, Chunk.Size);
                if (cameraRect.Intersects(chunkRect))
                    renderChunks.Add(pos, _chunks[x, y]);
            }
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
                            ? (Color.Green)
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
        spriteBatch.Draw(_texture, Vector2.Zero, Color.White);

        // spriteBatch.DrawRectangle(offsetX -50, offsetY -50, 2, 2, Color.Red);
        
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

    protected override void Debug()
    {
        ImGui.Begin("General stats");
        ImGui.Text($"Chunk access: {_chunkAccessCounter}");
        ImGui.End();
    }

    public IVector2 GetChunkPosition(IVector2 position)
    {
        return position / new IVector2(Chunk.Size, Chunk.Size);
    }
}
