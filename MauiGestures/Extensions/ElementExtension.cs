using MauiGestures.GestureArgs;
using System.Drawing;
using Point = Microsoft.Maui.Graphics.Point;

#if IOS || MACCATALYST
using UIKit;
#endif

namespace MauiGestures.Extensions;

/// <summary>
/// Extensions for VisualElement
/// </summary>
public static class ElementExtensions
{
    #region Methods
    /// <summary>
    /// Returns the absolute bounds of the element
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static RectangleF GetAbsoluteBoundsF(this PointArgs args)
    {
        var bounds = (args.Element as VisualElement).GetAbsoluteBounds();
        return new((float)bounds.X, (float)bounds.Y, (float)bounds.Width, (float)bounds.Height);
    }

    /// <summary>
    /// Gets the absolute bounds of the element
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Rect GetAbsoluteBounds(this PointArgs args)
        => (args.Element as VisualElement).GetAbsoluteBounds();

    /// <summary>
    /// Gets the absolute bounds of the element
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static Rect GetAbsoluteBounds(this VisualElement? element)
    {
        if (element == null)
            return Rect.Zero;

        var location = element.GetCoordinates();
        return new(location.X, location.Y, element.Width, element.Height);
    }

    /// <summary>
    /// Gets the coordinates of the element
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static Point GetCoordinates(this PointArgs args)
        => (args.Element as VisualElement).GetCoordinates();

    /// <summary>
    /// Gets the coordinates of the element
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static Point GetCoordinates(this VisualElement? element)
    {
        if (element == null)
            return Point.Zero;

        #if ANDROID
            var nativeView = element.Handler?.PlatformView as Android.Views.View;
            if (nativeView == null)
                return Point.Zero;
            var density = nativeView.Context!.Resources!.DisplayMetrics!.Density;

            var locationWindow = new int[2];
            nativeView.GetLocationInWindow(locationWindow);
            var locationOfRootWindow = new int[2];
            nativeView.RootView?.FindViewById(Android.Resource.Id.Content)?.GetLocationInWindow(locationOfRootWindow);

            return new(locationWindow[0] / density, (locationWindow[1] - locationOfRootWindow[1]) / density);

        #elif IOS || MACCATALYST
            if (element.Handler?.PlatformView is not UIView nativeView)
                return Point.Zero;
            var rect = nativeView.Superview.ConvertPointToView(nativeView.Frame.Location, null);
            return new(rect.X, rect.Y);
        #elif WINDOWS
            return Point.Zero;
        #else
            throw new NotSupportedException("Not supported on this platform");
        #endif
    }

    #endregion Methods
}
