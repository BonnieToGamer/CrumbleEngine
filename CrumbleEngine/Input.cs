using Microsoft.Xna.Framework.Input;
using Apos.Input;

namespace CrumbleEngine;

public class Input
{
    public static readonly ICondition CloseApplication = 
        new AnyCondition(
            new KeyboardCondition(Keys.Escape)
        );
}
