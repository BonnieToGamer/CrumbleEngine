using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrumbleEngine.Simulation.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collections;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation;

public class Chunk
{
    public bool ShouldUpdateThisFrame { get; private set; }
    public bool ShouldUpdateNextFrame { get; private set; }
    public IVector2 Position { get; private set; }
    public static readonly int Size = 32;
    
    private Element[,] elements;
    private List<ElementChange> changes;
    private Rectangle workingDirtyRect;
    private Rectangle dirtyRect;
    private readonly IVector2 dirtyRectMargin = new(4, 4);

    public Chunk(IVector2 position)
    {
        ShouldUpdateThisFrame = true;
        ShouldUpdateNextFrame = true;
        elements = new Element[Size, Size];
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                elements[x, y] = Element.GetElement(ElementTypes.Void);
            }
        }
        
        dirtyRect = new Rectangle(0, 0, Size, Size);
        Position = position;
        changes = new();
    }

    public void ApplyAndResetUpdateFlags()
    {
        ShouldUpdateThisFrame = ShouldUpdateNextFrame;
        ShouldUpdateNextFrame = false;
        
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                elements[x, y].ResetNextPos();
            }
        }
    }

    public void ActivateNextFrame()
    {
        ShouldUpdateNextFrame = true;
    }

    public void MoveElement(IVector2 from, IVector2 to)
    {
        // All coordinates will be stored as world positions.
        // Every time I want local coords. I have to convert.
        changes.Add(new(this, from, to));
    }

    public Element GetElement(IVector2 pos)
    {
        return elements[
            MathUtils.NegativeMod(pos.X, Size), 
            MathUtils.NegativeMod(pos.Y, Size)
        ];
    }

    public void SetElement(IVector2 pos, Element element)
    {
        if (element == null)
            Console.Write("a");
        elements[
            MathUtils.NegativeMod(pos.X, Size), 
            MathUtils.NegativeMod(pos.Y, Size)
        ] = element;
        
        UpdateDirtyRect(pos);
        ShouldUpdateNextFrame = true;
    }

    public bool InBounds(IVector2 pos)
    {
        IVector2 chunkPos = Position * Size;
        
        return 
            pos.X >= chunkPos.X        && pos.Y >= chunkPos.Y &&
            pos.X <  chunkPos.X + Size && pos.Y <  chunkPos.Y + Size;
    }
    
    public bool IsCellEmpty(IVector2 pos)
    {
        Element element = GetElement(pos);
        return element.Type == ElementTypes.Void || element.NextPos != null;
    }
    
    private void UpdateDirtyRect(IVector2 pos)
    {
        workingDirtyRect.X      = Math.Clamp(Math.Min(pos.X - dirtyRectMargin.X, workingDirtyRect.X), 0, Size);
        workingDirtyRect.Y      = Math.Clamp(Math.Min(pos.Y - dirtyRectMargin.Y, workingDirtyRect.Y), 0, Size);
        workingDirtyRect.Width  = Math.Clamp(Math.Max(pos.X + dirtyRectMargin.X, workingDirtyRect.Width), 0, Size);
        workingDirtyRect.Height = Math.Clamp(Math.Max(pos.Y + dirtyRectMargin.Y, workingDirtyRect.Height), 0, Size);
    }

    public void Update(GameTime gameTime, SimulationMatrix simMatrix)
    {
        if (ShouldUpdateThisFrame == false) return;
        
        bool elementsUpdated = false;
        for (int y = dirtyRect.Y; y < dirtyRect.Height; y++)
        {
            for (int x = dirtyRect.X; x < dirtyRect.Width; x++)
            {
                bool moved = elements[x, y].Update(ref gameTime, ref simMatrix, new IVector2(x, y) + Position * Size);
                if (moved) elementsUpdated = true;
            }
        }

        ShouldUpdateNextFrame = elementsUpdated;
        
        // dirtyRect = workingDirtyRect;
        // workingDirtyRect = new(Size, Size, -1, -1);
    }
    
    public void ProcessChanges(SimulationMatrix simMatrix)
    {
        for (int i = 0; i < changes.Count; i++)
        {
            if (IsCellEmpty(changes[i].newPosition)) continue;
            
            changes[i] = changes[^1];
            changes.RemoveAt(changes.Count - 1);
            i--;
        }

        if (changes.Count == 0)
            return;
        
        changes.Sort((a,b) 
            => (a.newPosition.X + a.newPosition.Y * Size).CompareTo(b.newPosition.X + b.newPosition.Y * Size)
        );
        
        changes.Add(new(null, new IVector2(-1, -1), new IVector2(-1, -1)));
        
        Random random = Random.Shared;

        int previousIndex = 0;
        for (int i = 0; i < changes.Count - 1; i++)
        {
            if (changes[i + 1].newPosition == changes[i].newPosition) continue;

            int randomIndex = previousIndex + random.Next(i - previousIndex + 1);

            ElementChange change = changes[randomIndex];

            Chunk originChunk = change.sourceChunk;
            Chunk newChunk = simMatrix.GetChunk(change.newPosition);
            Element originElement = originChunk.GetElement(change.position);
            Element newElement = newChunk.GetElement(change.newPosition);

            originChunk.SetElement(change.position, newElement);
            newChunk.SetElement(change.newPosition, originElement);

            previousIndex = i + 1;

        }
        
        changes.Clear();
    }

    public void UpdateNextFrame()
    {
        ShouldUpdateNextFrame = true;
    }
}
