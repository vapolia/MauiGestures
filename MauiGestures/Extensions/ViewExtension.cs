
namespace MauiGestures.Extensions;

/// <summary>
/// Extensions for the View class.
/// </summary>
public static class ViewExtensions
{
    #region Methods
    /// <summary>
    /// Gets or creates the GestureRoutingEffect for the view.
    /// </summary>
    /// <param name="view"></param>
    /// <returns></returns>
    public static GestureRoutingEffect GetOrCreateGestureEffect(this View view)
    {
        var effect = (GestureRoutingEffect?)view.Effects.FirstOrDefault(e => e is GestureRoutingEffect);
        if (effect == null)
        {
            effect = new GestureRoutingEffect();
            view.Effects.Add(effect);
        }
        return effect;
    }

    #endregion Methods
}

/// <summary>
/// Routing effect for gestures.
/// </summary>
public class GestureRoutingEffect : RoutingEffect
{
    /// <summary>
    /// Constructor for GestureRoutingEffect.
    /// </summary>
    public GestureRoutingEffect()
    {
    }
}