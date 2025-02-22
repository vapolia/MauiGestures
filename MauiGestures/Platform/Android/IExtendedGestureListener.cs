using Android.Views;

namespace MauiGestures.Platform.Android;

/// <summary>
/// Gesture listener with an additional method.
/// </summary>
interface IExtendedGestureListener
{
    void OnUp(MotionEvent? e);
}
