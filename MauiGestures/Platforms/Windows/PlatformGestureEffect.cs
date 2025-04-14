using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls.Platform;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Input;
using GestureRecognizer = Microsoft.UI.Input.GestureRecognizer;
using Microsoft.UI.Xaml;

namespace MauiGestures;

internal partial class PlatformGestureEffect : PlatformEffect
{
    readonly GestureRecognizer detector;
    int swipeThresholdInPoints = 40;

    public PlatformGestureEffect()
    {
        detector = new()
        {
            GestureSettings = 
                GestureSettings.Tap 
                | GestureSettings.Drag 
                | GestureSettings.ManipulationTranslateInertia 
                | GestureSettings.DoubleTap
                | GestureSettings.Hold
                | GestureSettings.HoldWithMouse,
            ShowGestureFeedback = false,
            //CrossSlideHorizontally = true
            //AutoProcessInertia = true //default
        };

        detector.Dragging += (sender, args) => 
        {
            TriggerCommand(panCommand, commandParameter);
            var gestureStatus = args.DraggingState switch
            {
                DraggingState.Started => GestureStatus.Started,
                DraggingState.Continuing => GestureStatus.Running,
                DraggingState.Completed => GestureStatus.Completed,
                _ => GestureStatus.Canceled
            };
            var parameters = new PanEventArgs(gestureStatus, new Point(args.Position.X, args.Position.Y));
            TriggerCommand(panPointCommand, parameters);
        };

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
            if(args.HoldingState == HoldingState.Started) 
            {
                TriggerCommand(longPressCommand, commandParameter);
                var pointArgs = new PointEventArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                TriggerCommand(longPressPointCommand, pointArgs);
            }
        };

        //Never called. Don't know why.
        detector.ManipulationInertiaStarting += (sender, args) =>
        {
            var isHorizontalSwipe = Math.Abs(args.Delta.Translation.Y) < swipeThresholdInPoints;
            var isVerticalSwipe = Math.Abs(args.Delta.Translation.X) < swipeThresholdInPoints;
            if (isHorizontalSwipe || isVerticalSwipe)
            {
                if (isHorizontalSwipe)
                {
                    var isLeftSwipe = args.Delta.Translation.X < 0;
                    TriggerCommand(isLeftSwipe ? swipeLeftCommand : swipeRightCommand, commandParameter);
                }
                else
                {
                    var isTopSwipe = args.Delta.Translation.Y < 0;
                    TriggerCommand(isTopSwipe ? swipeTopCommand : swipeBottomCommand, commandParameter);
                }
            }
        };

        //detector.CrossSliding += (sender, args) =>
        //{
        //    args.CrossSlidingState == CrossSlidingState.Started
        //};
    }

    private void TriggerCommand(ICommand? command, object? parameter)
    {
        if(command?.CanExecute(parameter) == true)
            command.Execute(parameter);
    }

    protected override partial void OnAttached()
    {
        var control = Control ?? Container;

        control.PointerMoved += ControlOnPointerMoved;
        control.PointerPressed += ControlOnPointerPressed;
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
        control.PointerMoved -= ControlOnPointerMoved;
        control.PointerPressed -= ControlOnPointerPressed;
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

    private void ControlOnPointerPressed(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
    {
        var element = sender as UIElement;
        if (element != null)
            element.CapturePointer(pointerRoutedEventArgs.Pointer);

        detector.CompleteGesture();
        detector.ProcessDownEvent(pointerRoutedEventArgs.GetCurrentPoint(Control ?? Container));
        pointerRoutedEventArgs.Handled = true;
    }

    private void ControlOnPointerMoved(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
    {
        detector.ProcessMoveEvents(returnAllPointsOnWindows ? 
            pointerRoutedEventArgs.GetIntermediatePoints(Control ?? Container)
            : new List<PointerPoint> { pointerRoutedEventArgs.GetCurrentPoint(Control ?? Container) });
        pointerRoutedEventArgs.Handled = true;
    }

    private void ControlOnPointerCanceled(object sender, PointerRoutedEventArgs args)
    {
        detector.CompleteGesture();
        args.Handled = true;

        var element = sender as UIElement;
        if (element != null)
            element.ReleasePointerCapture(args.Pointer);
    }

    private void ControlOnPointerReleased(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
    {
        detector.ProcessUpEvent(pointerRoutedEventArgs.GetCurrentPoint(Control ?? Container));
        pointerRoutedEventArgs.Handled = true;

        var element = sender as UIElement;
        if (element != null)
            element.ReleasePointerCapture(pointerRoutedEventArgs.Pointer);
    }
}

