using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Platform;
using UIKit;

namespace MauiGestures;

internal partial class GestureBehavior
{
    private List<UIGestureRecognizer>? recognizers;
    private UIImmediatePanGestureRecognizer? panDetector;
    private UIImmediatePinchGestureRecognizer? pinchDetector;
    
    private (Point Origin0, Point Origin1) pinchOrigin, lastPinch;

    private List<UIGestureRecognizer> CreateGestures(View view, UIView platformView)
    {
        //if (!allSubviews)
        //    tapDetector.ShouldReceiveTouch = (s, args) => args.View != null && (args.View == view || view.Subviews.Any(v => v == args.View));
        //else
        //    tapDetector.ShouldReceiveTouch = (s, args) => true;

        var tapDetector = CreateTapRecognizer(view, platformView, () => (tapCommand,tapPointCommand), false);
        var doubleTapDetector = CreateTapRecognizer(view, platformView, () => (doubleTapCommand, doubleTapPointCommand), true);
        doubleTapDetector.NumberOfTapsRequired = 2;
        var longPressDetector = CreateLongPressRecognizer(view, platformView, () => (longPressCommand, longPressPointCommand));

        panDetector = CreatePanRecognizer(view, platformView, () => (panCommand, panPointCommand));
        pinchDetector = CreatePinchRecognizer(view, platformView, () => pinchCommand);

        var swipeLeftDetector = CreateSwipeRecognizer(view, () => swipeLeftCommand, UISwipeGestureRecognizerDirection.Left);
        var swipeRightDetector = CreateSwipeRecognizer(view, () => swipeRightCommand, UISwipeGestureRecognizerDirection.Right);
        var swipeUpDetector = CreateSwipeRecognizer(view, () => swipeTopCommand, UISwipeGestureRecognizerDirection.Up);
        var swipeDownDetector = CreateSwipeRecognizer(view, () => swipeBottomCommand, UISwipeGestureRecognizerDirection.Down);

        return 
        [
            tapDetector, doubleTapDetector, longPressDetector, panDetector, pinchDetector,
            swipeLeftDetector, swipeRightDetector, swipeUpDetector, swipeDownDetector
        ];
    }

    private PointEventArgs GetPointArgs(View view, UIView platformView, UIGestureRecognizer recognizer)
    {
        var control = platformView;
        var point = recognizer.LocationInView(control).ToPoint();
        var args = new PointEventArgs(point, view, view.BindingContext);
        return args;
    }

    private UITapGestureRecognizer CreateTapRecognizer(View view, UIView platformView, Func<(ICommand? Command,ICommand? PointCommand)> getCommand, bool isDoubleTap)
    {
        return new (recognizer =>
        {
            var (command, pointCommand) = getCommand();
            if (command != null || pointCommand != null)
            {
                if (command?.CanExecute(commandParameter) == true)
                    command.Execute(commandParameter);

                var args = GetPointArgs(view, platformView, recognizer);
                if(pointCommand?.CanExecute(args) == true)
                    pointCommand.Execute(args);

                // Trigger events
                if (isDoubleTap)
                {
                    OnDoubleTap(view, EventArgs.Empty);
                    if (pointCommand != null)
                        OnDoubleTapPoint(view, args);
                }
                else
                {
                    OnTap(view, EventArgs.Empty);
                    if (pointCommand != null)
                        OnTapPoint(view, args);
                }
            }
        })
        {
            Enabled = false,
            ShouldRecognizeSimultaneously = (_, _) => true,
            //ShouldReceiveTouch = (recognizer, touch) => true,
        };
    }

    private UILongPressGestureRecognizer CreateLongPressRecognizer(View view, UIView platformView, Func<(ICommand? Command, ICommand? PointCommand)> getCommand)
    {
        return new (recognizer =>
        {
            if (recognizer.State == UIGestureRecognizerState.Began)
            {
                var (command, pointCommand) = getCommand();
                if (command != null || pointCommand != null)
                {
                    if (command?.CanExecute(commandParameter) == true)
                        command.Execute(commandParameter);

                    var args = GetPointArgs(view, platformView, recognizer);
                    if (pointCommand?.CanExecute(args) == true)
                        pointCommand.Execute(args);

                    OnLongPress(view, EventArgs.Empty);
                    if (pointCommand != null)
                        OnLongPressPoint(view, args);
                }
            }
        })
        {
            Enabled = false,
            ShouldRecognizeSimultaneously = (_, _) => true,
            //ShouldReceiveTouch = (recognizer, touch) => true,
        };
    }


    private UISwipeGestureRecognizer CreateSwipeRecognizer(View view, Func<ICommand?> getCommand, UISwipeGestureRecognizerDirection direction)
    {
        return new (() =>
        {
            var handler = getCommand();
            if (handler?.CanExecute(commandParameter) == true)
                handler.Execute(commandParameter);

            // Trigger swipe events based on direction
            if (direction == UISwipeGestureRecognizerDirection.Left)
                OnSwipeLeft(view, EventArgs.Empty);
            else if (direction == UISwipeGestureRecognizerDirection.Right)
                OnSwipeRight(view, EventArgs.Empty);
            else if (direction == UISwipeGestureRecognizerDirection.Up)
                OnSwipeTop(view, EventArgs.Empty);
            else if (direction == UISwipeGestureRecognizerDirection.Down)
                OnSwipeBottom(view, EventArgs.Empty);
        })
        {
            Enabled = false,
            ShouldRecognizeSimultaneously = (_, _) => true,
            Direction = direction
        };
    }
        
    private UIImmediatePinchGestureRecognizer CreatePinchRecognizer(View view, UIView platformView, Func<ICommand?> getCommand)
    {
        return new (recognizer =>
        {
            var command = getCommand();
            if (command != null)
            {
                var control = platformView;

                if (recognizer is { NumberOfTouches: < 2, State: UIGestureRecognizerState.Changed }) 
                    return;

                if(recognizer.State == UIGestureRecognizerState.Began)
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
                    //UIGestureRecognizerState.Cancelled => GestureStatus.Canceled,
                    _ => GestureStatus.Canceled,
                };

                var parameters = new PinchEventArgs(status, (current0, current1), pinchOrigin);
                if (command.CanExecute(parameters))
                    command.Execute(parameters);

                OnPinch(view, parameters);
            }
        })
        {
            Enabled = false,
            ShouldRecognizeSimultaneously = (_, _) => true,
        };
    }

    private UIImmediatePanGestureRecognizer CreatePanRecognizer(View view, UIView platformView, Func<(ICommand? Command, ICommand? PointCommand)> getCommand)
    {
        return new UIImmediatePanGestureRecognizer(recognizer =>
        {
            var (command, pointCommand) = getCommand();
            if (command != null || pointCommand != null)
            {
                if (recognizer.NumberOfTouches > 1 && recognizer.State != UIGestureRecognizerState.Cancelled && recognizer.State != UIGestureRecognizerState.Ended)
                    return;
                    
                var control = platformView;
                var point = recognizer.LocationInView(control).ToPoint();
                    
                if (command?.CanExecute(commandParameter) == true)
                    command.Execute(commandParameter);

                if (pointCommand != null && recognizer.State != UIGestureRecognizerState.Began)
                {
                    //GestureStatus.Started has already been sent by ShouldBegin. Don't sent it twice.

                    var gestureStatus = recognizer.State switch
                    {
                        UIGestureRecognizerState.Began => GestureStatus.Started,
                        UIGestureRecognizerState.Changed => GestureStatus.Running,
                        UIGestureRecognizerState.Ended => GestureStatus.Completed,
                        //UIGestureRecognizerState.Cancelled => GestureStatus.Canceled,
                        _ => GestureStatus.Canceled,
                    };

                    var parameter = new PanEventArgs(gestureStatus, point);
                    if (pointCommand.CanExecute(parameter))
                        pointCommand.Execute(parameter);

                    OnPanPoint(view, parameter);
                }

                OnPan(view, EventArgs.Empty);
            }
        })
        {
            Enabled = false,
            ShouldRecognizeSimultaneously = (_, _) => true,
            MaximumNumberOfTouches = 1,
            ShouldBegin = recognizer =>
            {
                var (command, pointCommand) = getCommand();
                if (command != null)
                {
                    if (command.CanExecute(commandParameter))
                        command.Execute(commandParameter);
                    return true;
                }
                            
                if(pointCommand != null)
                {
                    var control = platformView;
                    var point = recognizer.LocationInView(control).ToPoint();

                    var parameter = new PanEventArgs(GestureStatus.Started, point);
                    if (pointCommand.CanExecute(parameter))
                        pointCommand.Execute(parameter);
                    if (!parameter.CancelGesture)
                    {
                        OnPanPoint(view, parameter);
                        return true;
                    }
                }

                return false;
            }
        };
    }

    protected override void OnAttachedTo(View view, UIView platformView)
    {
        recognizers = CreateGestures(view, platformView);
        foreach (var recognizer in recognizers)
        {
            platformView.AddGestureRecognizer(recognizer);
            recognizer.Enabled = true;
        }

        OnAttached(view);
        OnViewPropertyChanged(view, new PropertyChangedEventArgs(String.Empty));
    }

    protected override void OnDetachedFrom(View view, UIView platformView)
    {
        OnDetach(view);

        foreach (var recognizer in recognizers!)
        {
            recognizer.Enabled = false;
            platformView.RemoveGestureRecognizer(recognizer);
        }

        recognizers = null;
        panDetector = null;
        pinchDetector = null;
    }
}