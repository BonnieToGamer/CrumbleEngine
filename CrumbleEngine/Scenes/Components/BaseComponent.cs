using Microsoft.Xna.Framework.Graphics;

namespace CrumbleEngine.Scenes.Components;

public abstract class BaseComponent
{
    public abstract void Update(float delta);
    public abstract void Draw(SpriteBatch spriteBatch);
}