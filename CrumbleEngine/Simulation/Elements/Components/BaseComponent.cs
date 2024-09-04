using Microsoft.Xna.Framework;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation.Elements.Components;

public abstract class BaseComponent
{
    protected Element _element;

    public void SetParent(Element parent) => _element = parent;

    public abstract bool Update(GameTime gameTime, SimulationMatrix world, IVector2 position);
}
