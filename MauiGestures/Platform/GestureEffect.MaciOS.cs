using Foundation;
using MauiGestures.Commands;
using MauiGestures.GestureArgs;
using MauiGestures.Platform.MaciOS;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using System.ComponentModel;
using UIKit;

namespace MauiGestures.Platform;

internal partial class GestureEffect : PlatformEffect
{
    #region Fields
    private readonly UIImmediatePanGestureRecognizer panDetector;
    private readonly UIImmediatePinchGestureRecognizer pinchDetector;
    private readonly List<UIGestureRecognizer> recognizers;
    private (Point Origin0, Point Origin1) pinchOrigin, lastPinch;

    private readonly int swipeThresholdInPointsMaciOS = 80;
    private readonly long swipeDurationInMsMaciOS = 200;
    private ExtendedUISwipeGestureRecognizer swipeLeftDetector;
    private ExtendedUISwipeGestureRecognizer swipeRightDetector;
    private ExtendedUISwipeGestureRecognizer swipeUpDetector;
    private ExtendedUISwipeGestureRecognizer swipeDownDetector;

    private UILongPressGestureRecognizer longPressDetector;
    private UITapGestureRecognizer tapDetector;
    private bool isLongPressing = false;
    private NSDate gestureStartTime;

    #endregion Fields

    #region Constructors
    public GestureEffect()
    {
        gestureStartTime = new NSDate();
        tapDetector = CreateTapRecognizer(() => (tapCommand, tapPointCommand));
        var doubleTapDetector = CreateTapRecognizer(() => (doubleTapCommand, doubleTapPointCommand));
        doubleTapDetector.NumberOfTapsRequired = 2;
        longPressDetector = CreateLongPressRecognizer(() => (longPressCommand, longPressPointCommand));

        panDetector = CreatePanRecognizer(() => (panCommand, panPointCommand));
        pinchDetector = CreatePinchRecognizer(() => (pinchCommand, pinchPointCommand));

        swipeLeftDetector = CreateSwipeRecognizer(UISwipeGestureRecognizerDirection.Left);
        swipeRightDetector = CreateSwipeRecognizer(UISwipeGestureRecognizerDirection.Right);
        swipeUpDetector = CreateSwipeRecognizer(UISwipeGestureRecognizerDirection.Up);
        swipeDownDetector = CreateSwipeRecognizer(UISwipeGestureRecognizerDirection.Down);

        recognizers = new List<UIGestureRecognizer>
        {
            tapDetector, doubleTapDetector, longPressDetector, panDetector, pinchDetector,
            swipeLeftDetector, swipeRightDetector, swipeUpDetector, swipeDownDetector
        };
    }

    #endregion Constructors

    #region Properties

    #endregion  Properties

    #region Methods
    private PointArgs GetPointArgs(UIGestureRecognizer recognizer)
    {
        var control = Control ?? Container;
        var point = recognizer.LocationInView(control).ToPoint();
        var args = new PointArgs(point, Element, Element.BindingContext);
        return args;
    }

    private UITapGestureRecognizer CreateTapRecognizer(Func<(ICommand? Command, ICommand<PointArgs>? PointCommand)> getCommand)
    {
        return new UITapGestureRecognizer(recognizer =>
        {
            if (!isLongPressing)
            {
                var (command, pointCommand) = getCommand();
                if (command != null || pointCommand != null)
                {
                    TriggerCommand(command, commandParameter);

                    var args = GetPointArgs(recognizer);
                    TriggerCommand(pointCommand, args);
                    TriggerEvent(TapEvent, args);
                }
            }
        })
        {
            Enabled = false,
            ShouldRecognizeSimultaneously = (_, _) => true,
        };
    }

    private UILongPressGestureRecognizer CreateLongPressRecognizer(Func<(ICommand? Command, ICommand<PointArgs>? PointCommand)> getCommand)
    {
        return new UILongPressGestureRecognizer(recognizer =>
        {
            if (recognizer.State == UIGestureRecognizerState.Began)
            {
                isLongPressing = true;
                var (command, pointCommand) = getCommand();
                if (command != null || pointCommand != null)
                {
                    TriggerCommand(command, commandParameter);

                    var args = GetPointArgs(recognizer);
                    TriggerCommand(pointCommand, args);
                    TriggerEvent(LongPressEvent, args);
                }
            }
            else if (recognizer.State == UIGestureRecognizerState.Ended || recognizer.State == UIGestureRecognizerState.Cancelled)
            {
                isLongPressing = false;
            }
        })
        {
            Enabled = false,
            ShouldRecognizeSimultaneously = (_, _) => true,
        };
    }

    private ExtendedUISwipeGestureRecognizer CreateSwipeRecognizer(UISwipeGestureRecognizerDirection direction)
    {
        var recognizer = new ExtendedUISwipeGestureRecognizer
        {
            NumberOfTouchesRequired = 1,
            Direction = direction,
            ShouldRecognizeSimultaneously = (gestureRecognizer, otherGestureRecognizer) => true
        };

        recognizer.AddTarget(() => HandleSwipe(recognizer));

        return recognizer;
    }

    private UIImmediatePinchGestureRecognizer CreatePinchRecognizer(Func<(ICommand? Command, ICommand<PinchArgs>? PointCommand)> getCommand)
    {
        return new UIImmediatePinchGestureRecognizer(recognizer =>
        {
            var (command, pointCommand) = getCommand();
            if (command != null || pointCommand != null)
            {
                var control = Control ?? Container;

                if (recognizer.NumberOfTouches < 2)
                {
                    if (recognizer.State == UIGestureRecognizerState.Changed)
                        return;
                }

                if (recognizer.State == UIGestureRecognizerState.Began)
                    lastPinch = (Point.Zero, Point.Zero);

                var current0 = lastPinch.Origin0;
                var current1 = lastPinch.Origin1;
                var lastCurrent0 = current0;
                var lastCurrent1 = current1;
                if (recognizer.NumberOfTouches >= 1)
                    current0 = lastCurrent0 = recognizer.LocationOfTouch(0, control).ToPoint();
                if (recognizer.NumberOfTouches >= 2)
                    current1 = lastCurrent1 = recognizer.LocationOfTouch(1, control).ToPoint();
                else if (recognizer.State == UIGestureRecognizerState.Began)
                    current1 = lastCurrent1 = current0;

                lastPinch = (lastCurrent0, lastCurrent1);
                if (recognizer.State == UIGestureRecognizerState.Began)
                    pinchOrigin = (current0, current1);

                var status = recognizer.State switch
                {
                    UIGestureRecognizerState.Began => GestureStatus.Started,
                    UIGestureRecognizerState.Changed => GestureStatus.Running,
                    UIGestureRecognizerState.Ended => GestureStatus.Completed,
                    UIGestureRecognizerState.Cancelled => GestureStatus.Canceled,
                    _ => GestureStatus.Canceled,
                };

                TriggerCommand(command, commandParameter);

                var parameters = new PinchArgs(status, (current0, current1), pinchOrigin);
                TriggerCommand(pointCommand, parameters);
                TriggerEvent(PinchEvent, parameters);
            }
        })
        {
            Enabled = false,
            ShouldRecognizeSimultaneously = (_, _) => true,
        };
    }

    private UIImmediatePanGestureRecognizer CreatePanRecognizer(Func<(ICommand? Command, ICommand<PanArgs>? PointCommand)> getCommand)
    {
        var recognizer = new UIImmediatePanGestureRecognizer(recognizer =>
        {
            var (command, pointCommand) = getCommand();
            var translation = new Point();
            var distance = new Double();
            var duration = new Double();

            if (command != null || pointCommand != null)
            {
                if (recognizer.NumberOfTouches > 1 && recognizer.State != UIGestureRecognizerState.Cancelled && recognizer.State != UIGestureRecognizerState.Ended)
                    return;

                var control = Control ?? Container;
                var currentPoint = recognizer.LocationInView(control).ToPoint();

                if (recognizer.State == UIGestureRecognizerState.Began)
                {
                    gestureStartTime = NSDate.Now;
                    return;
                }

                if (recognizer.State == UIGestureRecognizerState.Changed)
                {
                    translation = recognizer.TranslationInView(control).ToPoint();
                    distance = Math.Sqrt(translation.X * translation.X + translation.Y * translation.Y);
                    duration = (NSDate.Now.SecondsSinceReferenceDate - gestureStartTime.SecondsSinceReferenceDate) * 1000;

                    if (distance >= swipeThresholdInPointsMaciOS && duration >= swipeDurationInMsMaciOS)
                    {
                        var parameters = new PanArgs(currentPoint, GestureStatus.Running);
                        TriggerCommand(command, commandParameter);
                        TriggerCommand(pointCommand, parameters);
                        TriggerEvent(PanEvent, parameters);
                    }
                    return;
                }

                if (recognizer.State == UIGestureRecognizerState.Ended)
                {
                    translation = recognizer.TranslationInView(control).ToPoint();
                    distance = Math.Sqrt(translation.X * translation.X + translation.Y * translation.Y);
                    duration = (NSDate.Now.SecondsSinceReferenceDate - gestureStartTime.SecondsSinceReferenceDate) * 1000;

                    if (distance <= swipeThresholdInPointsMaciOS && duration <= swipeDurationInMsMaciOS)
                    {
                        var swipeDirection = GetSwipeDirection(translation.X, translation.Y);
                        var swipeArgs = new SwipeArgs(swipeDirection, distance, currentPoint);
                        TriggerCommand(swipeCommand, swipeArgs);
                        TriggerEvent(SwipeEvent, swipeArgs);        
                    }
                    else
                    {
                        var parameters = new PanArgs(currentPoint, GestureStatus.Completed);
                        TriggerCommand(command, commandParameter);
                        TriggerCommand(pointCommand, parameters);
                        TriggerEvent(PanEvent, parameters);
                    }
                }
            }
        })
        {
            Enabled = false,
            MaximumNumberOfTouches = 1,
            ShouldRecognizeSimultaneously = (_, _) => true,
        };
        return recognizer;
    }

    protected override partial void OnAttached()
    {
        var control = Control ?? Container;

        foreach (var recognizer in recognizers)
        {
            control.AddGestureRecognizer(recognizer);
            recognizer.Enabled = true;
        }

        OnElementPropertyChanged(new PropertyChangedEventArgs(String.Empty));
    }

    protected override partial void OnDetached()
    {
        var control = Control ?? Container;
        foreach (var recognizer in recognizers)
        {
            recognizer.Enabled = false;
            control.RemoveGestureRecognizer(recognizer);
        }
    }

    [Export("HandleSwipe")]
    private void HandleSwipe(UISwipeGestureRecognizer recognizer)
    {
        var control = Control ?? Container;
        var endPoint = recognizer.LocationInView(control).ToPoint();

        var swipeDirection = recognizer.Direction switch
        {
            UISwipeGestureRecognizerDirection.Right => SwipeDirection.Right,
            UISwipeGestureRecognizerDirection.Left => SwipeDirection.Left,
            UISwipeGestureRecognizerDirection.Up => SwipeDirection.Up,
            UISwipeGestureRecognizerDirection.Down => SwipeDirection.Down,
            _ => throw new ArgumentOutOfRangeException()
        };

        var startPoint = ((ExtendedUISwipeGestureRecognizer)recognizer).StartPoint;
        var distance = Math.Sqrt(Math.Pow(endPoint.X - startPoint.X, 2) + Math.Pow(endPoint.Y - startPoint.Y, 2));
        var swipeArgs = new SwipeArgs(swipeDirection, distance, endPoint);

        TriggerCommand(swipeCommand, swipeArgs);
        TriggerEvent(SwipeEvent, swipeArgs);
    }

    private SwipeDirection GetSwipeDirection(double deltaX, double deltaY)
    {
        var absX = Math.Abs(deltaX);
        var absY = Math.Abs(deltaY);

        if (absX > absY)
        {
            return deltaX > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        }
        else
        {
            return deltaY > 0 ? SwipeDirection.Down : SwipeDirection.Up;
        }
    }

    #endregion Methods
}