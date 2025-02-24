
namespace MauiGestures.GestureArgs;

/// <summary>
/// Arguments of a swipe gesture.
/// </summary>
public class SwipeArgs : EventArgs
{
    #region Constructors
    /// <summary>
    /// Constructor for SwipeArgs.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <param name="position"></param>
    public SwipeArgs(SwipeDirection direction, double distance = 0, Point position = new Point())
    {
        Direction = direction;
        Distance = distance;
        Position = position;
    }
    #endregion Constructors

    #region Properties
    /// <summary>
    /// Direction of the swipe gesture.
    /// </summary>
    public SwipeDirection Direction { get; }
    /// <summary>
    /// Distance of the swipe gesture.
    /// </summary>
    public double Distance { get; }
    /// <summary>
    /// Endposition of the swipe gesture.
    /// </summary>
    public Point Position { get; }

    #endregion Properties
}
