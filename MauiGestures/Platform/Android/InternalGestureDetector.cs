using Android.Views;

namespace MauiGestures.Platform.Android;

internal sealed class InternalGestureDetector : GestureDetector.SimpleOnGestureListener, IExtendedGestureListener
{
    #region Fields
    private MotionEvent? pinchInitialDown;
    internal int SwipeThresholdInPoints { get; set; }
    internal long MaxSwipeDuration { get; set; }

    #endregion Fields

    #region Constructors

    #endregion Constructors

    #region Properties
    internal float Density { get; set; }

    #endregion Properties

    #region Events
    internal Action<MotionEvent?>? TapAction { get; set; }
    internal Action<MotionEvent?>? DoubleTapAction { get; set; }
    internal Action<MotionEvent>? SwipeLeftAction { get; set; }
    internal Action<MotionEvent>? SwipeRightAction { get; set; }
    internal Action<MotionEvent>? SwipeTopAction { get; set; }
    internal Action<MotionEvent>? SwipeBottomAction { get; set; }
    internal Func<MotionEvent, MotionEvent?, bool>? PanAction { get; set; }
    internal Action<MotionEvent, MotionEvent?>? PinchAction { get; set; }
    internal Action<MotionEvent?>? LongPressAction { get; set; }

    #endregion Events

    #region Methods

    public override bool OnDoubleTap(MotionEvent? e)
    {
        DoubleTapAction?.Invoke(e);
        return true;
    }

    public override bool OnSingleTapUp(MotionEvent? e)
    {
        TapAction?.Invoke(e);
        return true;
    }

    public override void OnLongPress(MotionEvent? e)
        => LongPressAction?.Invoke(e);

    public override bool OnDown(MotionEvent? e)
    {
        return true;
    }

    public void OnUp(MotionEvent? e)
    {
        if (e != null)
            PanAction?.Invoke(e, e);

        pinchInitialDown = null;
    }

    public override bool OnScroll(MotionEvent? initialDown, MotionEvent currentMove, float distanceX, float distanceY)
    {
        if (initialDown != null)
        {
            if (pinchInitialDown == null && initialDown.PointerCount == 1 && currentMove.PointerCount == 2)
                pinchInitialDown = MotionEvent.Obtain(currentMove);

            switch (currentMove.PointerCount)
            {
                case 1 when PanAction != null:
                    return PanAction.Invoke(initialDown, currentMove);
                case 2 when PinchAction != null && pinchInitialDown != null:
                    PinchAction.Invoke(pinchInitialDown, currentMove);
                    return true;
            }
        }
        return false;
    }

    public override bool OnFling(MotionEvent? e1, MotionEvent? e2, float velocityX, float velocityY)
    {
        if (e1 == null || e2 == null)
            return false;

        var dx = e2.RawX - e1.RawX;
        var dy = e2.RawY - e1.RawY;
        var duration = e2.EventTime - e1.EventTime;

        if (duration > MaxSwipeDuration)
            return false;

        if (Math.Abs(dx) > SwipeThresholdInPoints * Density)
        {
            if (dx > 0)
                SwipeRightAction?.Invoke(e2);
            else
                SwipeLeftAction?.Invoke(e2);
            return true;
        }
        else if (Math.Abs(dy) > SwipeThresholdInPoints * Density)
        {
            if (dy > 0)
                SwipeBottomAction?.Invoke(e2);
            else
                SwipeTopAction?.Invoke(e2);
            return true;
        }
        return false;
    }

    #endregion Methods
}
