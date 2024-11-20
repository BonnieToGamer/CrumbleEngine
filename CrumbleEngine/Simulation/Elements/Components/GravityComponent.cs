using System;
using Microsoft.Xna.Framework;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation.Elements.Components;

public class GravityComponent : BaseComponent
{
    private int _gravity;

    public GravityComponent(int gravity = 1)
    {
        _gravity = gravity;
    }
    
    public override void Init() {}
    
    public override bool Update(ref GameTime gameTime, ref World world, IVector2 position, ElementNeighborhoodPlacement placement)
    {
        if (_updated) return true;

        if (placement is not (ElementNeighborhoodPlacement.TopLeft or ElementNeighborhoodPlacement.TopRight))
            return false;

        // Check below
        if (TryMoveOrInteract(position + new IVector2(0, _gravity), ref world, position))
        {
            _updated = true;
            return true;
        }

        // Check diagonals based on placement
        if (placement == ElementNeighborhoodPlacement.TopRight && 
            TryMoveOrInteract(position + new IVector2(-1, _gravity), ref world, position))
        {
            _updated = true;
            return true;
        }

        if (placement == ElementNeighborhoodPlacement.TopLeft && 
            TryMoveOrInteract(position + new IVector2(1, _gravity), ref world, position))
        {
            _updated = true;
            return true;
        }

        return false;
    }
}
