using System;
using System.Collections.Generic;
using System.Linq;
using CrumbleEngine.Simulation.Elements.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation.Elements;

public class Element
{
    public ElementTypes Type { get; private set; }
    public Color RenderColor { get; private set; }
    public bool Updated { get; private set; }
    private readonly HashSet<BaseComponent> _components;
    public bool ReadFlag { get; private set; }
    public IVector2? NextPos { get; private set; }

    private Element(ElementTypes type, Color color, bool ReadFlag = false)
    {
        this.ReadFlag = ReadFlag;
        Type = type;
        RenderColor = Randomise(color);
        _components = new();
    }

    private Color Randomise(Color color)
    {
        HslColor hslColor = color.ToHsl();
        float hue = hslColor.H;
        float saturation = hslColor.S + Random.Shared.NextSingle() * 0.2f - 0.2f;
        float lightness  = hslColor.L + Random.Shared.NextSingle() * 0.04f - 0.04f;

        hslColor = new HslColor(hue, saturation, lightness);

        return hslColor.ToRgb();
    }

    public BaseComponent GetComponent<T>() where T : BaseComponent
        => _components.First(component => component is T);

    public bool AddComponent(BaseComponent component)
    {
        bool success = _components.Add(component);
        if (success)
            component.SetParent(this);
        
        return success;
    }

    public void ResetUpdateFlag()
    {
        NextPos = null;
        Updated = false;
    }

    public void SetNextPos(IVector2 pos) => NextPos = pos;
    public void ResetNextPos() => NextPos = null;

    public bool Update(GameTime gameTime, SimulationMatrix simMatrix, IVector2 position)
    {
        // if (Updated) return true;
        Updated = true;
        
        return _components.Any(component => component.Update(gameTime, simMatrix, position));
    }

    public static Element GetElement(ElementTypes elementTypes, bool readFlag = false)
    {
        Element element;
        switch (elementTypes)
        {
            case ElementTypes.Void:
                element = new(elementTypes, Color.Transparent);
                return element;
            case ElementTypes.Sand:
                element = new(elementTypes, Color.Yellow, readFlag);
                element.AddComponent(new GravityComponent());
                return element;
            
            case ElementTypes.Stone:
                element = new(elementTypes, Color.Gray, readFlag);
                return element;
            
            case ElementTypes.Red:
                element = new(elementTypes, Color.Red, readFlag);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Green:
                element = new(elementTypes, Color.Green, readFlag);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Blue:
                element = new(elementTypes, Color.Blue, readFlag);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Yellow:
                element = new(elementTypes, Color.Yellow, readFlag);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Pink:
                element = new(elementTypes, Color.Pink, readFlag);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Purple:
                element = new(elementTypes, Color.Purple, readFlag);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Cyan:
                element = new(elementTypes, Color.Cyan, readFlag);
                element.AddComponent(new GravityComponent());
                return element;
            
            case ElementTypes.MonoGameOrange:
                element = new(elementTypes, Color.MonoGameOrange, readFlag);
                element.AddComponent(new GravityComponent());
                return element;
            
            default:
                return null;
        }
    }
}

public enum ElementTypes
{
    Void,
    Sand,
    Stone,
    Red,
    Green,
    Blue,
    Yellow,
    Pink,
    Purple,
    Cyan,
    MonoGameOrange
}