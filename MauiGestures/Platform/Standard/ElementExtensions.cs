using System.Drawing;
using Point = Microsoft.Maui.Graphics.Point;

#if IOS || MACCATALYST
using UIKit;
#endif

namespace MauiGestures;

public static class ElementExtensions
{
    public static RectangleF GetAbsoluteBoundsF(this PointEventArgs args)
    {
        var bounds = (args.Element as VisualElement).GetAbsoluteBounds();
        return new((float)bounds.X, (float)bounds.Y, (float)bounds.Width, (float)bounds.Height);
    }

    public static Rect GetAbsoluteBounds(this PointEventArgs args)
        => (args.Element as VisualElement).GetAbsoluteBounds();
    
    public static Point GetCoordinates(this PointEventArgs args)
        => (args.Element as VisualElement).GetCoordinates();
    
    public static Rect GetAbsoluteBounds(this VisualElement? element)
    {
        if(element == null)
            return Rect.Zero;
            
        var location = GetCoordinates(element);
        return new(location.X, location.Y, element.Width, element.Height);
    }

    public static Point GetCoordinates(this VisualElement? element)
    {
        if(element == null)
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
            
        return new (locationWindow[0] / density, (locationWindow[1]-locationOfRootWindow[1]) / density);
#elif IOS || MACCATALYST
        if (element.Handler?.PlatformView is not UIView nativeView)
            return Point.Zero;
        var rect = nativeView.Superview.ConvertPointToView(nativeView.Frame.Location, null);
        return new (rect.X, rect.Y);
#elif WINDOWS
        return Point.Zero;
#else
        throw new NotSupportedException("Not supported on this platform");
#endif
    }
}