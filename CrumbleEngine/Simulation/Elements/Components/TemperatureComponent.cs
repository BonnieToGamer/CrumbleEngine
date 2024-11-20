using Microsoft.Xna.Framework;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation.Elements.Components;

public class TemperatureComponent : BaseComponent
{
    private float _temperature;

    public TemperatureComponent(float temperature = 23f)
    {
        _temperature = temperature;
    }
    
    public override void Init() {}

    public void AddTemperature(float temp)
    {
        _temperature = temp;
    }

    public float GetTemperature() => _temperature;

    public override bool Update(ref GameTime gameTime, ref World world, IVector2 position, ElementNeighborhoodPlacement placement)
    {
        return false;
    }
}