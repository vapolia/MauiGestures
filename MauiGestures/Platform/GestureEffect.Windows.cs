using System.ComponentModel;
using Microsoft.Maui.Controls.Platform;
using Microsoft.UI.Xaml.Input;
using MauiGestures.GestureArgs;
using Microsoft.UI.Input;
using GestureRecognizer = Microsoft.UI.Input.GestureRecognizer;

namespace MauiGestures.Platform
{
    internal partial class GestureEffect : PlatformEffect
    {
        #region Fields
        private readonly GestureRecognizer _detector;
        private bool isHolding;
        private bool isPotentialSwipe;
        private readonly int swipeThresholdInPoints = 30;
        private readonly long swipeDurationInMs = 150;
        private Point? dragStartPoint;
        private DateTime pointerPressedTime;

        #endregion Fields

        #region Constructors
        public GestureEffect()
        {
            _detector = new()
            {
                GestureSettings =
                   GestureSettings.Tap
                   | GestureSettings.RightTap
                   | GestureSettings.Drag
                   | GestureSettings.DoubleTap
                   | GestureSettings.Hold
                   | GestureSettings.HoldWithMouse
            };

            _detector.Dragging += (sender, args) =>
            {
                var currentPoint = new Point(args.Position.X, args.Position.Y);

                if (args.DraggingState == DraggingState.Started)
                {
                    isPotentialSwipe = true;
                    dragStartPoint = currentPoint;
                    return;
                }

                if (args.DraggingState == DraggingState.Continuing)
                {
                    var gestureStatus = GestureStatus.Running;
                    var parameters = new PanArgs(currentPoint, gestureStatus);
                    TriggerCommand(panCommand, commandParameter);
                    TriggerCommand(panPointCommand, parameters);
                    TriggerEvent(PanEvent, parameters);
                    return;
                }

                if (args.DraggingState == DraggingState.Completed && dragStartPoint.HasValue && isPotentialSwipe)
                {
                    var swipeDuration = DateTime.Now - pointerPressedTime;
                    if (swipeDuration.TotalMilliseconds <= swipeDurationInMs)
                    {
                        var deltaX = currentPoint.X - dragStartPoint.Value.X;
                        var deltaY = currentPoint.Y - dragStartPoint.Value.Y;
                        var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                        if (distance >= swipeThresholdInPoints)
                        {
                            var swipeDirection = GetSwipeDirection(deltaX, deltaY);
                            var swipeArgs = new SwipeArgs(swipeDirection, distance, currentPoint);
                            TriggerCommand(swipeCommand, swipeArgs);
                            TriggerEvent(SwipeEvent, swipeArgs);
                        }
                    }
                }

                if (args.DraggingState == DraggingState.Completed)
                {
                    ResetStates();
                }
            };

            _detector.Tapped += (sender, args) =>
            {
                if (args.TapCount == 1)
                {
                    TriggerCommand(tapCommand, commandParameter);
                    var pointArgs = new PointArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                    TriggerCommand(tapPointCommand, pointArgs);
                    TriggerEvent(TapEvent, pointArgs);
                }
                else if (args.TapCount == 2)
                {
                    TriggerCommand(doubleTapCommand, commandParameter);
                    var pointArgs = new PointArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                    TriggerCommand(doubleTapPointCommand, pointArgs);
                    TriggerEvent(DoubleTapEvent, pointArgs);
                }
            };

            _detector.RightTapped += (sender, args) =>
            {
                if (!isHolding)
                {
                    TriggerCommand(rightTabCommand, commandParameter);
                    var pointArgs = new PointArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                    TriggerCommand(rightTapPointCommand, pointArgs);
                    TriggerEvent(RightTapEvent, pointArgs);
                }
            };

            _detector.Holding += (sender, args) =>
            {
                if (args.HoldingState == HoldingState.Started)
                {
                    isHolding = true;
                    TriggerCommand(longPressCommand, commandParameter);
                    var pointArgs = new PointArgs(new Point(args.Position.X, args.Position.Y), Element, Element.BindingContext);
                    TriggerCommand(longPressPointCommand, pointArgs);
                    TriggerEvent(LongPressEvent, pointArgs);
                }
            };
        }

        #endregion Constructors

        #region Properties

        #endregion Properties

        #region Methods
        protected override partial void OnAttached()
        {
            var control = Control ?? Container;

            if (control is Microsoft.UI.Xaml.UIElement uiElement)
            {
                uiElement.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY | ManipulationModes.TranslateInertia | ManipulationModes.All;
            }

            control.PointerMoved += ControlOnPointerMoved;
            control.PointerPressed += ControlOnPointerPressed;
            control.PointerReleased += ControlOnPointerReleased;
            control.PointerCanceled += ControlOnPointerCanceled;
            control.PointerCaptureLost += ControlOnPointerCanceled;
            control.PointerExited += ControlOnPointerExited;
            control.PointerEntered += ControlOnPointerEntered;

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
            control.PointerExited -= ControlOnPointerExited;
            control.PointerEntered -= ControlOnPointerEntered;
        }

        private void ControlOnPointerPressed(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            _detector.CompleteGesture();
            _detector.ProcessDownEvent(pointerRoutedEventArgs.GetCurrentPoint(Control ?? Container));
            pointerPressedTime = DateTime.Now;
            pointerRoutedEventArgs.Handled = true;
        }

        private void ControlOnPointerExited(object sender, PointerRoutedEventArgs args)
        {
            args.Handled = true;
        }

        private void ControlOnPointerEntered(object sender, PointerRoutedEventArgs args)
        {
            args.Handled = true;
        }

        private void ControlOnPointerMoved(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            _detector.ProcessMoveEvents(returnAllPointsOnWindows ?
                pointerRoutedEventArgs.GetIntermediatePoints(Control ?? Container)
                : new List<PointerPoint> { pointerRoutedEventArgs.GetCurrentPoint(Control ?? Container) });
            pointerRoutedEventArgs.Handled = true;
        }

        private void ControlOnPointerCanceled(object sender, PointerRoutedEventArgs args)
        {
            _detector.CompleteGesture();
            ResetStates();
            args.Handled = true;
        }

        private void ControlOnPointerReleased(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            _detector.ProcessUpEvent(pointerRoutedEventArgs.GetCurrentPoint(Control ?? Container));
            ResetStates();
            pointerRoutedEventArgs.Handled = true;
        }

        private void ResetStates()
        {
            dragStartPoint = null;
            isPotentialSwipe = false;
            isHolding = false;
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
}