using CrumbleEngine.Simulation.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace CrumbleEngine.Scenes.Components;

public class Button
{
    public readonly ElementTypes ElementType;
    
    private readonly Rectangle _rectangle;
    private readonly Texture2D _texture;
    private readonly Color _highlight;
    private bool _pressed;

    
    public delegate void ButtonEventHandler(Button sender);
    public event ButtonEventHandler ButtonPressed;
    
    public Button(Rectangle rect, Color color, Color highlight, ElementTypes elementType, bool pressed = false)
    {
        ElementType = elementType;
        _pressed = pressed;
        _rectangle = rect;
        _highlight = highlight;
        _texture = new Texture2D(GameRoot.Instance.GraphicsDevice, rect.Width, rect.Height);

        Color[] data = new Color[rect.Width * rect.Height];
        for (int i = 0; i < data.Length; i++)
            data[i] = color;
        
        _texture.SetData(data);
    }
    
    public void Update(Camera<Vector2> camera)
    {
        MouseState state = Mouse.GetState();
        Point mousePoint = state.Position;
        Vector2 mousePos = camera.ScreenToWorld(new Vector2(mousePoint.X, mousePoint.Y));
        Rectangle mouseRect = new((int)mousePos.X, (int)mousePos.Y, 1, 1);
        Rectangle actualRect = new(_rectangle.Location + camera.Position.ToPoint(), _rectangle.Size);

        if (mouseRect.Intersects(actualRect) && state.LeftButton == ButtonState.Pressed)
        {
            ButtonPressed?.Invoke(this);
            _pressed = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos)
    {
        Rectangle drawRect = new Rectangle(
            _rectangle.Location + new Point((int)cameraPos.X, (int)cameraPos.Y),
            _rectangle.Size
        );

        Vector2 position = _rectangle.Location.ToVector2() + cameraPos;
        
        spriteBatch.Draw(_texture, position, Color.White);
        if (_pressed) spriteBatch.DrawRectangle(position, _rectangle.Size, _highlight);
    }

    public void UnPress()
    {
        _pressed = false;
    }
}