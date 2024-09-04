using System;
using Microsoft.Xna.Framework;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation.Elements.Components;

public class GravityComponent : BaseComponent
{
    private Vector2 _velocity;
    private readonly Vector2 _terminalVelocity = new Vector2(16f, 16f);

    public GravityComponent()
    {
        _velocity = Vector2.Zero;
    }

    public override bool Update(GameTime gameTime, SimulationMatrix simMatrix, IVector2 position)
    {
        // if (_element.ReadFlag)
            // Console.WriteLine($"{position}");

        Vector2 newVelocity = _velocity + Vector2.One * (float)gameTime.ElapsedGameTime.TotalSeconds;
        IVector2 endPosition = position + new IVector2((int)Math.Round(newVelocity.X), (int)Math.Round(newVelocity.Y));
        
        if (endPosition == position && _velocity == newVelocity) 
            return false;
        
        _velocity = new Vector2(
            MathHelper.Clamp(newVelocity.X, -_terminalVelocity.X, _terminalVelocity.X),
            MathHelper.Clamp(newVelocity.Y, -_terminalVelocity.Y, _terminalVelocity.Y)
        );
        
        int currentX = position.X;
        int currentY = position.Y + 1;
        
        if (_element.ReadFlag)
            Console.WriteLine("");
        
        for (; currentY <= endPosition.Y; currentY++)
        {
            if (simMatrix.GetChunk(new (currentX, currentY)).IsCellEmpty(new(currentX, currentY)))
            {
                simMatrix.SwapElement(position, new(currentX, currentY));
                position = new IVector2(currentX, currentY);
            }
        
            else if (simMatrix.GetChunk(new (currentX - 1, currentY)).IsCellEmpty(new(currentX - 1, currentY)))
            {
                currentX -= 1;
                simMatrix.SwapElement(position, new(currentX, currentY));
                position = new IVector2(currentX, currentY);
            }
            
            else if (simMatrix.GetChunk(new (currentX + 1, currentY)).IsCellEmpty(new(currentX + 1, currentY)))
            {
                currentX += 1;
                simMatrix.SwapElement(position, new(currentX, currentY));
                position = new IVector2(currentX, currentY);
            }
            else
                break;

            _element.SetNextPos(new(currentX, currentY));

        }

        return true;
    }
}
