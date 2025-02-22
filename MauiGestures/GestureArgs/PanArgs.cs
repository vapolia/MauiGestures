
namespace MauiGestures.GestureArgs;

/// <summary>
/// Arguments of a pan gesture.
/// </summary>
/// <param name="point"></param>
/// <param name="status"></param>
public class PanArgs(Point point, GestureStatus status)
{
    #region Constructors

    #endregion Constructors

    #region Properties
    /// <summary>
    /// Status of the gesture.
    /// </summary>
    public GestureStatus Status { get; } = status;

    /// <summary>
    /// Point of the pan gesture.
    /// </summary>
    public Point Point { get; } = point;

    /// <summary>
    /// If true, the gesture is cancelled.
    /// </summary>
    public bool CancelGesture { get; set; }

    #endregion Properties
}