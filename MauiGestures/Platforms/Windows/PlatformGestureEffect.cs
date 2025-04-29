using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Microsoft.Maui.Controls.Platform;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;
using GestureRecognizer = Microsoft.UI.Input.GestureRecognizer;

namespace MauiGestures;

internal partial class PlatformGestureEffect : PlatformEffect
{
    readonly GestureRecognizer detector;
    Windows.Foundation.Point swipeStartPosition;
    //int swipeThresholdInPoints = 40;

    PointerPoint? _currentPointer1 = null;      // First touch point
    PointerPoint? _currentPointer2 = null;      // Second touch point
    bool pinching = false;
    bool panning = false;
    (Point Point1, Point Point2) startingPinchPoints;
    (Point Point1, Point Point2) currentPinchPoints;

    public PlatformGestureEffect()
    {
        detector = new()
        {
            GestureSettings = 
                GestureSettings.Tap
                //| GestureSettings.Drag 
                //| GestureSettings.ManipulationTranslateInertia
                | GestureSettings.DoubleTap
                | GestureSettings.Hold
                | GestureSettings.HoldWithMouse
                | GestureSettings.ManipulationTranslateX
                | GestureSettings.ManipulationTranslateY
                | GestureSettings.CrossSlide
                | GestureSettings.ManipulationScale,
            ShowGestureFeedback = false,
            CrossSlideHorizontally = true
            //AutoProcessInertia = true //default
        };

        //detector.Dragging += (sender, args) =>
        //{
        //    TriggerCommand(panCommand, commandParameter);
        //    var gestureStatus = args.DraggingState switch
        //    {
        //        DraggingState.Started => GestureStatus.Started,
        //        DraggingState.Continuing => GestureStatus.Running,
        //        DraggingState.Completed => GestureStatus.Completed,
        //        _ => GestureStatus.Canceled
        //    };
        //    var parameters = new PanEventArgs(gestureStatus, new Point(args.Position.X, args.Position.Y));
        //    TriggerCommand(panPointCommand, parameters);

        //    Trace.WriteLine("dragging");
        //};

        detector.Tapped += (sender, args) =>
        {
            if (args.TapCount == 1)
            {
                TriggerCommand(tapCommand, commandParameter);
                var pointArgs = new PointEventArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                TriggerCommand(tapPointCommand, pointArgs);
            }
            else if (args.TapCount == 2) 
            {
                TriggerCommand(doubleTapCommand, commandParameter);
                var pointArgs = new PointEventArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                TriggerCommand(doubleTapPointCommand, pointArgs);
            }
        };

        detector.Holding += (sender, args) =>
        {
            if (args.HoldingState == HoldingState.Started) 
            {
                TriggerCommand(longPressCommand, commandParameter);
                var pointArgs = new PointEventArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                TriggerCommand(longPressPointCommand, pointArgs);
            }
        };

        detector.ManipulationStarted += (sender, args) =>
        {
            // Need to wait till ManipulationUpdated to see if we're pinching or panning before we commit to a gesture
        };

        detector.ManipulationUpdated += (sender, args) =>
        {
            if (!pinching && _currentPointer1 != null &&  _currentPointer2 != null)
            {
                startingPinchPoints = (new Point(_currentPointer1.Position.X, _currentPointer1.Position.Y),
                                       new Point(_currentPointer2.Position.X, _currentPointer2.Position.Y));

                var parameters = new PinchEventArgs(GestureStatus.Started, startingPinchPoints, startingPinchPoints);
                TriggerCommand(pinchCommand, parameters);

                pinching = true;
            }
            else if (pinching)
            {
                if (_currentPointer1 != null &&  _currentPointer2 != null)
                {
                    currentPinchPoints = (new Point(_currentPointer1.Position.X, _currentPointer1.Position.Y), 
                                          new Point(_currentPointer2.Position.X, _currentPointer2.Position.Y));

                    var parameters = new PinchEventArgs(GestureStatus.Running, currentPinchPoints, startingPinchPoints);
                    TriggerCommand(pinchCommand, parameters);
                }
            }
            else   // panning
            {
                TriggerCommand(panCommand, commandParameter);
                var parameters = new PanEventArgs(panning ? GestureStatus.Running : GestureStatus.Started, new Point(args.Position.X, args.Position.Y));
                TriggerCommand(panPointCommand, parameters);

                panning = true;
            }
        };

        detector.ManipulationCompleted += (sender, args) =>
        {
            if (pinching)
            {
                var parameters = new PinchEventArgs(GestureStatus.Completed, currentPinchPoints, startingPinchPoints);
                TriggerCommand(pinchCommand, parameters);

                pinching = false;
            }
            else if (panning)
            {
                TriggerCommand(panCommand, commandParameter);
                var parameters = new PanEventArgs(GestureStatus.Completed, new Point(args.Position.X, args.Position.Y));
                TriggerCommand(panPointCommand, parameters);

                panning = false;
            }
        };

        //Never called. Don't know why.
        //detector.ManipulationInertiaStarting += (sender, args) =>
        //{
        //    var isHorizontalSwipe = Math.Abs(args.Delta.Translation.Y) < swipeThresholdInPoints;
        //    var isVerticalSwipe = Math.Abs(args.Delta.Translation.X) < swipeThresholdInPoints;
        //    if (isHorizontalSwipe || isVerticalSwipe)
        //    {
        //        if (isHorizontalSwipe)
        //        {
        //            var isLeftSwipe = args.Delta.Translation.X < 0;
        //            TriggerCommand(isLeftSwipe ? swipeLeftCommand : swipeRightCommand, commandParameter);
        //        }
        //        else
        //        {
        //            var isTopSwipe = args.Delta.Translation.Y < 0;
        //            TriggerCommand(isTopSwipe ? swipeTopCommand : swipeBottomCommand, commandParameter);
        //        }
        //    }
        //};

        detector.CrossSliding += (sender, args) =>
        {
            switch (args.CrossSlidingState)
            {
                case CrossSlidingState.Started:
                    swipeStartPosition = args.Position;
                    break;

                case CrossSlidingState.Completed:
                    var deltaX = args.Position.X - swipeStartPosition.X;
                    var deltaY = args.Position.Y -  swipeStartPosition.Y;

                    var isHorizontalSwipe = Math.Abs(deltaX) >= Math.Abs(deltaY);
                    if (isHorizontalSwipe)
                    {
                        var isLeftSwipe = deltaX < 0;
                        TriggerCommand(isLeftSwipe ? swipeLeftCommand : swipeRightCommand, commandParameter);
                    }
                    else
                    {
                        var isTopSwipe = deltaY < 0;
                        TriggerCommand(isTopSwipe ? swipeTopCommand : swipeBottomCommand, commandParameter);
                    }
                    break;
            }
        };
    }

    private void TriggerCommand(ICommand? command, object? parameter)
    {
        if(command?.CanExecute(parameter) == true)
            command.Execute(parameter);
    }

    protected override partial void OnAttached()
    {
        var control = Control ?? Container;

        control.PointerPressed += ControlOnPointerPressed;
        control.PointerMoved += ControlOnPointerMoved;
        control.PointerReleased += ControlOnPointerReleased;
        control.PointerCanceled += ControlOnPointerCanceled;
        control.PointerCaptureLost += ControlOnPointerCanceled;
        //control.ManipulationInertiaStarting += ControlOnManipulationInertiaStarting;
        //control.PointerExited += ControlOnPointerCanceled;

        OnElementPropertyChanged(new PropertyChangedEventArgs(String.Empty));
    }

    protected override partial void OnDetached()
    {
        var control = Control ?? Container;
        control.PointerPressed -= ControlOnPointerPressed;
        control.PointerMoved -= ControlOnPointerMoved;
        control.PointerReleased -= ControlOnPointerReleased;
        control.PointerCanceled -= ControlOnPointerCanceled;
        control.PointerCaptureLost -= ControlOnPointerCanceled;
        //control.ManipulationInertiaStarting -= ControlOnManipulationInertiaStarting;
        //control.PointerExited -= ControlOnPointerCanceled;
    }

    //private void ControlOnManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
    //{
    //    detector.ProcessInertia();
    //    e.Handled = true;
    //}

    private void ControlOnPointerPressed(object sender, PointerRoutedEventArgs args)
    {
        var point = args.GetCurrentPoint(Control ?? Container);
        detector.ProcessDownEvent(point);
        (Control ?? Container).CapturePointer(args.Pointer);
        args.Handled = true;
    }

    private void ControlOnPointerMoved(object sender, PointerRoutedEventArgs args)
    {
        var points = returnAllPointsOnWindows ? args.GetIntermediatePoints(Control ?? Container) : [args.GetCurrentPoint(Control ?? Container)];

        if (_currentPointer1 == null || points[0].Timestamp > _currentPointer1.Timestamp)
        {
            // Set the first pointer if it either hasn't been set or is out-of-date (old timestamp)
            _currentPointer1 = points[0];
            _currentPointer2 = null;
        }
        else
        {
            // If the new pointer's timestamp matches the first pointer's, then we know this is a second pointer
            if (points[0].Timestamp == _currentPointer1.Timestamp)
                _currentPointer2 = points[0];
        }

        try
        {
            detector.ProcessMoveEvents(points);
        }
        catch (COMException e)
        {
            if ((uint)e.HResult != 0x80400000)
                throw;

            // Occasionally, when multiple pointers are involved (i.e., in a pinch gesture), GetIntermediatePoints can 
            // "miss" some point data when it gets backlogged.  This is ok if it misses the exact same frame/timestamp for
            // ALL pointers, but usually it just misses one point for one pointer.  ProcessMoveEvents doesn't like that as
            // it assumes the frame IDs and timestamp you pass it for the first pointer will exactly match all the frame
            // IDs and timestamps you pass it for the second pointer, and it will throw a COMException with an HRESULT of
            // 0x80400000 when they don't match.
            // 
            // Trying to check and prune the pointer data for multiple pointers before you pass it to ProcessMoveEvents
            // is very complicated (and maybe impossible to do perfectly) because you have to try and manage it across
            // multiple calls to ControlOnPointerMoved (meaning you have to wait for a subsequent ControlOnPointerMoved
            // before you can safely call ProcessMoveEvents with the cleaned data).  You also have nothing but timestamp
            // values to aid you.
            //
            // Because of this, and the rarity of this occurring, I feel the best approach is just to eat the exception and 
            // move on.  That bit of pointer data will be lost but it's not a huge deal.
        }

        args.Handled = true;
    }

    private void ControlOnPointerReleased(object sender, PointerRoutedEventArgs args)
    {
        var point = args.GetCurrentPoint(Control ?? Container);
        detector.ProcessUpEvent(point);
        (Control ?? Container).ReleasePointerCapture(args.Pointer);
        args.Handled = true;
    }

    private void ControlOnPointerCanceled(object sender, PointerRoutedEventArgs args)
    {
        detector.CompleteGesture();
        (Control ?? Container).ReleasePointerCapture(args.Pointer);
        args.Handled = true;
    }
}

