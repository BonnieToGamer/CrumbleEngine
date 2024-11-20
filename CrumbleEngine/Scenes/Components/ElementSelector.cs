using System;
using MonoGame.Extended;
using Apos.Input;
using CrumbleEngine.Simulation.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrumbleEngine.Scenes.Components;

public class ElementSelector : BaseComponent
{
    public ElementTypes CurrentElementType { get; private set; }
    
    private readonly Camera<Vector2> _camera;
    private readonly Button[] _buttons;
    private Button _selectedButton;
    
    public ElementSelector(Camera<Vector2> camera)
    {
        _camera = camera;
        int enumSize = Enum.GetNames(typeof(ElementTypes)).Length;
        _buttons = new Button[enumSize];
        
        int i = 0;
        foreach (ElementTypes elementType in Enum.GetValues(typeof(ElementTypes)))
        {
            Color drawColor = Element.GetColor(elementType);
            if (drawColor == Color.Transparent) drawColor = Color.LightGray;
            _buttons[i] = new Button(new Rectangle(10 * i, 2, 8, 8), drawColor, Color.Orange, elementType, i == 0);
            _buttons[i].ButtonPressed += OnButtonPressed;
            i++;
        }

        _selectedButton = _buttons[0];
        CurrentElementType = ElementTypes.Void;
    }

    private void OnButtonPressed(Button sender)
    {
        if (sender == _selectedButton) return;
        
        _selectedButton.UnPress();
        _selectedButton = sender;

        CurrentElementType = sender.ElementType;
    }

    public override void Update(float delta)
    {
        foreach (Button button in _buttons)
        {
            button.Update(_camera);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        foreach (Button button in _buttons)
            button.Draw(spriteBatch, _camera.Position);
    }
}