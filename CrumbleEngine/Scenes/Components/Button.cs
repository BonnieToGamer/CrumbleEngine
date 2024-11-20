using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace CrumbleEngine.Scenes.Components;

public class Button
{
    private Rectangle _rectangle;
    private Texture2D _texture;
    private Color _highlight;
    private bool _pressed;
    
    public Button(Rectangle rect, Color color, Color highlight, bool pressed = false)
    {
        _pressed = pressed;
        _rectangle = rect;
        _highlight = highlight;
        _texture = new Texture2D(GameRoot.Instance.GraphicsDevice, rect.Width, rect.Height);

        Color[] data = new Color[rect.Width * rect.Height];
        for (int i = 0; i < data.Length; i++)
            data[i] = color;
        
        _texture.SetData(data);
    }
    
    public void Update(Camera camera)
    {
        MouseState state = Mouse.GetState();
        Point mousePoint = state.Position;
        Vector2 mousePos = camera.ScreenToWorld(new Vector2(mousePoint.X, mousePoint.Y));
        Rectangle mouseRect = new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1);

        if (mouseRect.Intersects(_rectangle) && state.LeftButton == ButtonState.Pressed)
        {
            
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, _rectangle, Color.White);
        if (_pressed) spriteBatch.DrawRectangle(_rectangle, _highlight);
    }
}