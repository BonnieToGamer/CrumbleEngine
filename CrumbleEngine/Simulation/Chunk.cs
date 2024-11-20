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
    public IVector2 PositionScaled { get; private set; }
    public static readonly int Size = 32;
    
    private Element[,] elements;
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
        PositionScaled = position * Size;
    }

    public void ApplyAndResetUpdateFlags()
    {
        ShouldUpdateThisFrame = ShouldUpdateNextFrame;
        ShouldUpdateNextFrame = false;
        
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                elements[x, y].ResetUpdateFlag();
            }
        }
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
            return;
        
        elements[
            MathUtils.NegativeMod(pos.X, Size), 
            MathUtils.NegativeMod(pos.Y, Size)
        ] = element;
        
        // UpdateDirtyRect(pos);
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
        return element.Type == ElementTypes.Void;
    }
    
    private void UpdateDirtyRect(IVector2 pos)
    {
        workingDirtyRect.X      = Math.Clamp(Math.Min(pos.X - dirtyRectMargin.X, workingDirtyRect.X), 0, Size);
        workingDirtyRect.Y      = Math.Clamp(Math.Min(pos.Y - dirtyRectMargin.Y, workingDirtyRect.Y), 0, Size);
        workingDirtyRect.Width  = Math.Clamp(Math.Max(pos.X + dirtyRectMargin.X, workingDirtyRect.Width), 0, Size);
        workingDirtyRect.Height = Math.Clamp(Math.Max(pos.Y + dirtyRectMargin.Y, workingDirtyRect.Height), 0, Size);
    }

    public void Update(GameTime gameTime, World simMatrix, int offsetX, int offsetY)
    {
        if (ShouldUpdateThisFrame == false) return;
        
        bool elementsUpdated = ShouldUpdateNextFrame;

        for (int y = offsetY; y < Size; y += 2)
        {
            for (int x = offsetX; x < Size; x += 2)
            {
                IVector2 basePosition = new(PositionScaled.X + x, PositionScaled.Y + y);

                IVector2 topRight = new(basePosition.X + 1, basePosition.Y);
                IVector2 topLeft = new(basePosition.X, basePosition.Y);
                IVector2 bottomLeft = new(basePosition.X, basePosition.Y + 1);
                IVector2 bottomRight = new(basePosition.X + 1, basePosition.Y + 1);
                
                elementsUpdated = simMatrix.GetElement(topRight).Update(ref gameTime, ref simMatrix, topRight, ElementNeighborhoodPlacement.TopLeft) || elementsUpdated;
                elementsUpdated = simMatrix.GetElement(topLeft).Update(ref gameTime, ref simMatrix, topLeft, ElementNeighborhoodPlacement.TopRight) || elementsUpdated;
                elementsUpdated = simMatrix.GetElement(bottomLeft).Update(ref gameTime, ref simMatrix, bottomLeft, ElementNeighborhoodPlacement.BottomLeft) || elementsUpdated;
                elementsUpdated = simMatrix.GetElement(bottomRight).Update(ref gameTime, ref simMatrix, bottomRight, ElementNeighborhoodPlacement.BottomRight) || elementsUpdated;
            }
        }

        ShouldUpdateNextFrame = elementsUpdated;
        
        // dirtyRect = workingDirtyRect;
        // workingDirtyRect = new(Size, Size, -1, -1);
    }

    public void UpdateNextFrame()
    {
        ShouldUpdateNextFrame = true;
    }
}
