using Foundation;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;
using UIKit;

namespace MauiGestures.Platform.MaciOS;

internal class ExtendedUISwipeGestureRecognizer : UISwipeGestureRecognizer
{
    internal Point StartPoint { get; private set; }

    internal ExtendedUISwipeGestureRecognizer()
    {
    }

    /// <summary>
    /// Extension of the UISwipeGestureRecognizer to get the start point of the swipe.
    /// </summary>
    /// <param name="touches"></param>
    /// <param name="evt"></param>
    public override void TouchesBegan(NSSet touches, UIEvent evt)
    {
        base.TouchesBegan(touches, evt);

        if (touches.AnyObject is UITouch touch)
        {
            StartPoint = touch.LocationInView(View).ToPoint();
        }
    }
}