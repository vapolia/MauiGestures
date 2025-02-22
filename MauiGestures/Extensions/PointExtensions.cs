using System;

namespace MauiGestures.Extensions;

internal static class PointExtensions
{
    #region Methods
    /// <summary>
    /// Adds two points together.
    /// </summary>
    /// <param name="pt1"></param>
    /// <param name="pt2"></param>
    /// <returns></returns>
    public static Point Add(this Point pt1, Point pt2)
        => new Point(pt1.X + pt2.X, pt1.Y + pt2.Y);

    /// <summary>
    /// Substracts two points.
    /// </summary>
    /// <param name="pt1"></param>
    /// <param name="pt2"></param>
    /// <returns></returns>
    public static Point Substract(this Point pt1, Point pt2)
        => new Point(pt1.X - pt2.X, pt1.Y - pt2.Y);

    /// <summary>
    /// Calculates the angle of a line with the horizontal axis.
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public static double AngleWithHorizontal(this (Point Point1, Point Point2) line)
    {
        var vector = line.Point2.Substract(line.Point1);
        return Math.Atan2(vector.Y, vector.X);
    }

    /// <summary>
    /// Divides a point by a value.
    /// </summary>
    /// <param name="pt"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Point Divide(this Point pt, double value)
        => new Point(pt.X / value, pt.Y / value);

    /// <summary>
    /// Calculates the distance between two points.
    /// </summary>
    /// <param name="pt1"></param>
    /// <param name="pt2"></param>
    /// <returns></returns>
    internal static double Distance2(this Point pt1, Point pt2)
    {
        var dx = pt2.X - pt1.X;
        var dy = pt2.Y - pt1.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    #endregion Methods
}
