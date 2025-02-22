using MauiGestures.Extensions;

namespace MauiGestures.GestureArgs;

/// <summary>
/// Arguments of a pinch gesture.
/// </summary>
public class PinchArgs
{
    #region Constructors
    /// <summary>
    /// Constructor for PinchArgs.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="currentPoints"></param>
    /// <param name="startingPoints"></param>
    public PinchArgs(GestureStatus status, (Point Point1, Point Point2) currentPoints, (Point Point1, Point Point2) startingPoints)
    {
        Status = status;
        CurrentPoints = currentPoints;
        StartingPoints = startingPoints;

        Center = startingPoints.Point1.Add(startingPoints.Point2).Divide(2);

        var initialDistance = startingPoints.Point1.Distance2(startingPoints.Point2);
        var currentDistance = currentPoints.Point1.Distance2(currentPoints.Point2);
        Scale = initialDistance > double.Epsilon ? currentDistance / initialDistance : 1;

        RotationRadians = currentPoints.AngleWithHorizontal() - startingPoints.AngleWithHorizontal();
    }

    #endregion Constructors

    #region Properties
    /// <summary>
    /// Pinch gesture status.
    /// </summary>
    public GestureStatus Status { get; }

    /// <summary>
    /// Current points of the pinch gesture.
    /// </summary>
    public (Point Point1, Point Point2) CurrentPoints { get; }

    /// <summary>
    /// Starting points of the pinch gesture.
    /// </summary>
    public (Point Point1, Point Point2) StartingPoints { get; }

    /// <summary>
    /// Center point of the pinch gesture.
    /// </summary>
    public Point Center { get; }

    /// <summary>
    /// Scale of the pinch gesture.
    /// </summary>
    public double Scale { get; }

    /// <summary>
    /// Rotation of the pinch gesture in radians.
    /// </summary>
    public double RotationRadians { get; }

    /// <summary>
    /// Rotation of the pinch gesture in degrees.
    /// </summary>
    public double RotationDegrees => RotationRadians * 180 / Math.PI;

    #endregion Properties
}
