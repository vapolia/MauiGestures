using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Microsoft.Maui.Controls.Platform;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using GestureRecognizer = Microsoft.UI.Input.GestureRecognizer;

namespace MauiGestures;

internal partial class GestureBehavior
{
    GestureRecognizer? detector;

    Windows.Foundation.Point swipeStartPosition;
    //int swipeThresholdInPoints = 40;

    PointerPoint? currentPointer1;      // First touch point
    PointerPoint? currentPointer2;      // Second touch point
    bool pinching;
    bool panning;
    (Point Point1, Point Point2) startingPinchPoints;
    (Point Point1, Point Point2) currentPinchPoints;

    GestureRecognizer CreateGestures(View view)
    {
        var recognizer = new GestureRecognizer
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

        recognizer.Tapped += (sender, args) =>
        {
            if (args.TapCount == 1)
            {
                TriggerCommand(tapCommand, commandParameter);
                var pointArgs = new PointEventArgs(new Point(args.Position.X, args.Position.Y), view, view.BindingContext);
                TriggerCommand(tapPointCommand, pointArgs);
            }
            else if (args.TapCount == 2) 
            {
                TriggerCommand(doubleTapCommand, commandParameter);
                var pointArgs = new PointEventArgs(new Point(args.Position.X, args.Position.Y), view, view.BindingContext);
                TriggerCommand(doubleTapPointCommand, pointArgs);
            }
        };

        recognizer.Holding += (sender, args) =>
        {
            if (args.HoldingState == HoldingState.Started) 
            {
                TriggerCommand(longPressCommand, commandParameter);
                var pointArgs = new PointEventArgs(new Point(args.Position.X, args.Position.Y), view, view.BindingContext);
                TriggerCommand(longPressPointCommand, pointArgs);
            }
        };

        recognizer.ManipulationStarted += (sender, args) =>
        {
            // Need to wait till ManipulationUpdated to see if we're pinching or panning before we commit to a gesture
        };

        recognizer.ManipulationUpdated += (sender, args) =>
        {
            if (!pinching && currentPointer1 != null &&  currentPointer2 != null)
            {
                startingPinchPoints = (new Point(currentPointer1.Position.X, currentPointer1.Position.Y),
                                       new Point(currentPointer2.Position.X, currentPointer2.Position.Y));

                var parameters = new PinchEventArgs(GestureStatus.Started, startingPinchPoints, startingPinchPoints);
                TriggerCommand(pinchCommand, parameters);

                pinching = true;
            }
            else if (pinching)
            {
                if (currentPointer1 != null &&  currentPointer2 != null)
                {
                    currentPinchPoints = (new Point(currentPointer1.Position.X, currentPointer1.Position.Y), 
                                          new Point(currentPointer2.Position.X, currentPointer2.Position.Y));

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

        recognizer.ManipulationCompleted += (sender, args) =>
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

        recognizer.CrossSliding += (sender, args) =>
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

        return recognizer;
    }

    private void TriggerCommand(ICommand? command, object? parameter)
    {
        if(command?.CanExecute(parameter) == true)
            command.Execute(parameter);
    }

    protected override void OnAttachedTo(View view, Microsoft.UI.Xaml.FrameworkElement platformView)
    {
        detector = CreateGestures(view);
        
        platformView.PointerPressed += ControlOnPointerPressed;
        platformView.PointerMoved += ControlOnPointerMoved;
        platformView.PointerReleased += ControlOnPointerReleased;
        platformView.PointerCanceled += ControlOnPointerCanceled;
        platformView.PointerCaptureLost += ControlOnPointerCanceled;
        //control.ManipulationInertiaStarting += ControlOnManipulationInertiaStarting;
        //control.PointerExited += ControlOnPointerCanceled;

        OnAttached(view);
        OnViewPropertyChanged(view, new PropertyChangedEventArgs(String.Empty));
    }

    protected override void OnDetachedFrom(View view, Microsoft.UI.Xaml.FrameworkElement platformView)
    {
        OnDetach(view);
        
        platformView.PointerPressed -= ControlOnPointerPressed;
        platformView.PointerMoved -= ControlOnPointerMoved;
        platformView.PointerReleased -= ControlOnPointerReleased;
        platformView.PointerCanceled -= ControlOnPointerCanceled;
        platformView.PointerCaptureLost -= ControlOnPointerCanceled;
        //control.ManipulationInertiaStarting -= ControlOnManipulationInertiaStarting;
        //control.PointerExited -= ControlOnPointerCanceled;

        detector = null;
    }

    //private void ControlOnManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
    //{
    //    detector.ProcessInertia();
    //    e.Handled = true;
    //}

    private void ControlOnPointerPressed(object sender, PointerRoutedEventArgs args)
    {
        var platformView = (UIElement)sender;
        var point = args.GetCurrentPoint(platformView);
        detector?.ProcessDownEvent(point);
        platformView.CapturePointer(args.Pointer);
        args.Handled = true;
    }

    private void ControlOnPointerMoved(object sender, PointerRoutedEventArgs args)
    {
        var platformView = (UIElement)sender;
        var points = processIntermediatePoints ? args.GetIntermediatePoints(platformView) : [args.GetCurrentPoint(platformView)];

        if (currentPointer1 == null || points[0].Timestamp > currentPointer1.Timestamp)
        {
            // Set the first pointer if it either hasn't been set or is out-of-date (old timestamp)
            currentPointer1 = points[0];
            currentPointer2 = null;
        }
        else
        {
            // If the new pointer's timestamp matches the first pointer's, then we know this is a second pointer
            if (points[0].Timestamp == currentPointer1.Timestamp)
                currentPointer2 = points[0];
        }

        try
        {
            detector?.ProcessMoveEvents(points);
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
        var platformView = (UIElement)sender;
        var point = args.GetCurrentPoint(platformView);
        detector?.ProcessUpEvent(point);
        platformView.ReleasePointerCapture(args.Pointer);
        args.Handled = true;
    }

    private void ControlOnPointerCanceled(object sender, PointerRoutedEventArgs args)
    {
        var platformView = (UIElement)sender;
        detector?.CompleteGesture();
        platformView.ReleasePointerCapture(args.Pointer);
        args.Handled = true;
    }
}

