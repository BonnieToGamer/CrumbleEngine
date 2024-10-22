using System;
using Microsoft.Xna.Framework;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation.Elements.Components;

public class GravityComponent : BaseComponent
{
    private Vector2 _velocity = Vector2.Zero;
    private readonly Vector2 _terminalVelocity = new Vector2(2f, 2f);

    public override bool Update(ref GameTime gameTime, ref SimulationMatrix simMatrix, IVector2 position)
    {
        /* calculate the new velocity.
         *  - velocity += gravity.
         * clamp the new velocity.
         *  - clamp(velocity, terminal velocity)
         * apply the new velocity.
         *  - position += velocity
         *      - check every element down
         */
        
        Vector2 newVelocity = _velocity + SimulationMatrix.Gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        _velocity = new(
            MathHelper.Clamp(newVelocity.X, -_terminalVelocity.X, _terminalVelocity.X),
            MathHelper.Clamp(newVelocity.Y, -_terminalVelocity.Y, _terminalVelocity.Y)
        );
        
        IVector2 endPosition = position + new IVector2((int)Math.Round(_velocity.X), (int)Math.Round(_velocity.Y));
        
        int currentX = position.X;
        int currentY = position.Y + 1;
        bool hasMoved = false;
        IVector2 currentPosition = position;
        
        for (; currentY <= endPosition.Y; currentY++)
        {
            if (TryMoveTo(simMatrix, ref currentPosition, 0 , 1))
            {
                hasMoved = true;
                continue;
            } // Move vertically

            int dir = Random.Shared.NextSingle() >= 0.5 ? 1 : -1;
            if (TryMoveTo(simMatrix, ref currentPosition, dir , 1))
            {
                hasMoved = true;
                continue;
            } // Move right
            if (TryMoveTo(simMatrix, ref currentPosition, -dir, 1)) 
            {
                hasMoved = true;
                continue;
            } // Move left

            // If no movement is possible, break the loop
            break;
        }
        
        // If currentY == position.Y + 1 we know that all movement was unsuccessful
        // since we define currentY to be position.Y + 1
        return hasMoved || _velocity != Vector2.Zero;
    }
    
    private bool TryMoveTo(SimulationMatrix simMatrix, ref IVector2 currentPosition, int deltaX, int deltaY)
    {
        int newX = currentPosition.X + deltaX;
        int newY = currentPosition.Y + deltaY;

        if (!simMatrix.GetChunk(new(newX, newY)).IsCellEmpty(new(newX, newY)))
            return false; // Movement failed

        simMatrix.SwapElement(currentPosition, new(newX, newY));
        currentPosition = new IVector2(newX, newY);
        _element.SetNextPos(currentPosition);
        
        return true; // Movement was successful
    }
}
