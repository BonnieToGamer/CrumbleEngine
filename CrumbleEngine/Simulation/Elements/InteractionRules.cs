using System;
using System.Collections.Generic;
using System.Linq;
using MonoGame.Utilities;

namespace CrumbleEngine.Simulation.Elements;

public class InteractionRules
{
    public static readonly List<InteractionRule> Rules = new()
    {
        new InteractionRule
        {
            SourceType = ElementTypes.Sand,
            TargetType = ElementTypes.Water,
            Interaction = (source, target, world, sourcePos, targetPos) =>
            {
                // Swap Sand and Water
                if (world.GetElement(targetPos - new IVector2(0, 1)).Type == ElementTypes.Void)
                {
                    bool result = false;
                    result |= world.SwapElement(targetPos, targetPos - new IVector2(0, 1));
                    result |= world.SwapElement(sourcePos, targetPos);
                    return result;
                }
                
                return world.SwapElement(sourcePos, targetPos);
            }
        }
    };
    
    public static bool TryGetInteraction(Element source, Element target, out Func<Element, Element, World, IVector2, IVector2, bool> interaction)
    {
        InteractionRule rule = Rules.FirstOrDefault(r => r.SourceType == source.Type && r.TargetType == target.Type);
        if (rule != null)
        {
            interaction = rule.Interaction;
            return true;
        }

        interaction = null;
        return false;
    }

}

public class InteractionRule
{
    public ElementTypes SourceType;
    public ElementTypes TargetType;
    public Func<Element, Element, World, IVector2, IVector2, bool> Interaction;
}
