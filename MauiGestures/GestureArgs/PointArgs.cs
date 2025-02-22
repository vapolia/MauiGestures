
namespace MauiGestures.GestureArgs;

/// <summary>
/// Arguments of a point gesture.
/// </summary>
public class PointArgs
{
    #region Constructors
    /// <summary>
    /// Constructor for PointArgs.
    /// </summary>
    /// <param name="point"></param>
    /// <param name="element"></param>
    /// <param name="bindingContext"></param>
    public PointArgs(Point point, Element element, object bindingContext)
    {
        Point = point;
        Element = element;
        BindingContext = bindingContext;
    }

    #endregion Constructors

    #region Properties
    /// <summary>
    /// Point
    /// </summary>
    public Point Point { get; }

    /// <summary>
    /// Element 
    /// </summary>
    public Element Element { get; }

    /// <summary>
    /// Binding
    /// </summary>
    public object BindingContext { get; }

    #endregion Properties
}
