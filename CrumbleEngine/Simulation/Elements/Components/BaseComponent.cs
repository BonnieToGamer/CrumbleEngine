using System;
using Microsoft.Xna.Framework;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation.Elements.Components;

public abstract class BaseComponent
{
    protected Element _element;
    protected bool _updated = false;


    public void SetParent(Element parent) => _element = parent;

    public abstract void Init();
    public abstract bool Update(ref GameTime gameTime, ref World world, IVector2 position, ElementNeighborhoodPlacement placement);
    public bool ResetUpdate() => _updated = false;
    
    protected bool TryMoveOrInteract(IVector2 targetPosition, ref World world, IVector2 position)
    {
        Element targetElement = world.GetElement(targetPosition);

        if (InteractionRules.TryGetInteraction(_element, targetElement,
                out Func<Element, Element, World, IVector2, IVector2, bool> interaction))
        {
            return interaction(_element, targetElement, world, position, targetPosition);
        }

        if (targetElement.Type != ElementTypes.Void) return false;
        return world.SwapElement(position, targetPosition);
    }
}
