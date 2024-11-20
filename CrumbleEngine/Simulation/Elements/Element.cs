using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CrumbleEngine.Simulation.Elements.Components;
using ImGuiNET;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation.Elements;

public class Element : Debugging
{
    public bool ShouldDebug = false;
    public ElementTypes Type { get; private set; }
    public Color RenderColor { get; private set; }
    private readonly HashSet<BaseComponent> _components;
    
    private bool _updated;
    private IVector2 _position;
    
    private Element(ElementTypes type, Color color)
    {
        Type = type;
        RenderColor = Randomise(color);
        _components = new();
    }

    public void InitComponents()
    {
        foreach (BaseComponent component in _components)
        {
            component.Init();
        }
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

    public T GetComponent<T>() where T : BaseComponent
        => (T)_components.First(component => component is T);

    public bool AddComponent(BaseComponent component)
    {
        bool success = _components.Add(component);
        if (success)
            component.SetParent(this);
        
        return success;
    }

    public void ResetUpdateFlag()
    {
        _updated = false;
        foreach (BaseComponent component in _components)
            component.ResetUpdate();
    }

    public bool Update(ref GameTime gameTime, ref World world, IVector2 position, ElementNeighborhoodPlacement placement)
    {
        if (_updated) return false;
        _position = position;
        
        bool any = false;
        foreach (BaseComponent component in _components)
            if (component.Update(ref gameTime, ref world, position, placement))
                any = true;

        _updated = any;

        return any;
    }

    public static Element GetElement(ElementTypes elementType)
    {
        Element element;
        switch (elementType)
        {
            case ElementTypes.Void:
                element = new(elementType, GetColor(elementType));
                return element;
            case ElementTypes.Sand:
                element = new(elementType, GetColor(elementType));
                element.AddComponent(new GravityComponent());
                element.AddComponent(new TemperatureComponent());
                element.InitComponents();
                return element;
            
            case ElementTypes.Stone:
                element = new(elementType, GetColor(elementType));
                element.AddComponent(new TemperatureComponent());
                element.InitComponents();
                return element;
            
            case ElementTypes.Water:
                element = new Element(elementType, GetColor(elementType));
                element.AddComponent(new LiquidComponent());
                element.AddComponent(new GravityComponent());
                element.AddComponent(new TemperatureComponent());
                element.InitComponents();
                return element;
            
            case ElementTypes.Smoke:
                element = new Element(elementType, GetColor(elementType));
                element.AddComponent(new GravityComponent(-1));
                element.AddComponent(new LiquidComponent());
                element.AddComponent(new TemperatureComponent());
                element.InitComponents();
                return element;
            
            default:
                return null;
        }
    }

    public static Color GetColor(ElementTypes elementTypes)
    {
        switch (elementTypes)
        {
                case ElementTypes.Sand:
                    return Color.Yellow;
                case ElementTypes.Stone:
                    return Color.Gray;
                case ElementTypes.Water:
                    return Color.Blue;
                case ElementTypes.Smoke:
                    return Color.DarkGray;
            
                case ElementTypes.Void:
                default:
                    return Color.Transparent;
        }
    }
    
    protected override void Debug()
    {
        if (!ShouldDebug) return;
        ImGui.Begin("Watched element");
        
        string[] types = Enum.GetNames(typeof(ElementTypes));
        int currentIndex = (int)Type;
        
        if (ImGui.Combo("Type", ref currentIndex, types, types.Length))
            Type = (ElementTypes)currentIndex;

        ImGui.Text($"Position: {_position}");
        
        if (ImGui.Button("Remove watch"))
            ShouldDebug = false;
        
        ImGui.End();
    }
}

public enum ElementTypes
{
    Void,
    Sand,
    Stone,
    Water,
    Smoke
}

public enum ElementNeighborhoodPlacement
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}