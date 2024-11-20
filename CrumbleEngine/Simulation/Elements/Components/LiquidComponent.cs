using System;
using Microsoft.Xna.Framework;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation.Elements.Components;

public class LiquidComponent : BaseComponent
{
    private GravityComponent _gravity;
    private bool _movingLeft;
    private Timer _moveTimer;
    private bool _shouldMove;

    public override void Init()
    {
        _shouldMove = true;
        _moveTimer = new Timer(4f);
        _movingLeft = Random.Shared.Next(0, 2) == 0;
        _gravity = _element.GetComponent<GravityComponent>();
    }

    public override bool Update(ref GameTime gameTime, ref World world, IVector2 position,
        ElementNeighborhoodPlacement placement)
    {
        if (_updated) return true;

        if (_gravity.Update(ref gameTime, ref world, position, placement))
        {
            _updated = true;
            _shouldMove = true;
            _moveTimer.Reset();
            return true;
        }

        if (_shouldMove == false) return false;

        if (_moveTimer.Update((float)gameTime.ElapsedGameTime.TotalSeconds))
        {
            _shouldMove = false;
            return false;
        }

        switch (placement)
        {
            case ElementNeighborhoodPlacement.TopLeft when _movingLeft == false:
            {
                if (TryMoveOrInteract(position + new IVector2(1, 0), ref world, position))
                {
                    _updated = true;
                    return true;
                }

                _movingLeft = true;
                break;
            }
            case ElementNeighborhoodPlacement.TopRight when _movingLeft:
            {
                if (TryMoveOrInteract(position + new IVector2(-1, 0), ref world, position))
                {
                    _updated = true;
                    return true;
                }
            
                _movingLeft = false;
                break;
            }
        }


        return false;

        
    }
}