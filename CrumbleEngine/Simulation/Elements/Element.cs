using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    public bool Updated { get; private set; }
    private readonly HashSet<BaseComponent> _components;
    public IVector2? NextPos { get; private set; }
    
    private IVector2 _position;
    
    private Element(ElementTypes type, Color color)
    {
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

    public bool Update(ref GameTime gameTime, ref SimulationMatrix simMatrix, IVector2 position)
    {
        // if (Updated) return true;
        _position = position;
        
        Updated = true;
        bool any = false;
        foreach (BaseComponent component in _components)
            if (component.Update(ref gameTime, ref simMatrix, position))
                any = true;

        return any;
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
                // element = new(elementTypes, new Color(Random.Shared.NextSingle(), Random.Shared.NextSingle(), Random.Shared.NextSingle()));
                element = new(elementTypes, Color.Yellow);
                element.AddComponent(new GravityComponent());
                return element;
            
            case ElementTypes.Stone:
                element = new(elementTypes, Color.Gray);
                return element;
            
            case ElementTypes.Red:
                element = new(elementTypes, Color.Red);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Green:
                element = new(elementTypes, Color.Green);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Blue:
                element = new(elementTypes, Color.Blue);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Yellow:
                element = new(elementTypes, Color.Yellow);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Pink:
                element = new(elementTypes, Color.Pink);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Purple:
                element = new(elementTypes, Color.Purple);
                element.AddComponent(new GravityComponent());
                return element;

            case ElementTypes.Cyan:
                element = new(elementTypes, Color.Cyan);
                element.AddComponent(new GravityComponent());
                return element;
            
            case ElementTypes.MonoGameOrange:
                element = new(elementTypes, Color.MonoGameOrange);
                element.AddComponent(new GravityComponent());
                return element;
            
            default:
                return null;
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

        ImGui.Text($"Position: ${_position}");
        
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
    Red,
    Green,
    Blue,
    Yellow,
    Pink,
    Purple,
    Cyan,
    MonoGameOrange
}