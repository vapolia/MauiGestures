using System.ComponentModel;
using System.Runtime.CompilerServices;
using MauiGestures.GestureArgs;
using MauiGestures.Commands;

namespace MauiGestures.Example
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string? _gestureText;
        public string GestureText
        {
            get => _gestureText ?? String.Empty;
            set
            {
                _gestureText = value;
                OnPropertyChanged();
            }
        }

        private Color _boxColor = Colors.Blue;
        public Color BoxColor
        {
            get => _boxColor;
            set
            {
                _boxColor = value;
                OnPropertyChanged();
            }
        }

        private string? _gestureInfo;
        public string GestureInfo
        {
            get => _gestureInfo ?? String.Empty;
            set
            {
                _gestureInfo = value;
                OnPropertyChanged();
            }
        }


        private bool _isAdvancedMode;
        public bool IsAdvancedMode
        {
            get => _isAdvancedMode;
            set
            {
                _isAdvancedMode = value;
                OnPropertyChanged();
            }
        }

        public ICommand? TapCommand { get; private set; }
        public ICommand<PointArgs>? TapPointCommand { get; private set; }
        public ICommand? RightTapCommand { get; private set; }
        public ICommand<PointArgs>? RightTapPointCommand { get; private set; }
        public ICommand? DoubleTapCommand { get; private set; }
        public ICommand<PointArgs>? DoubleTapPointCommand { get; private set; }
        public ICommand? LongPressCommand { get; private set; }
        public ICommand<PointArgs>? LongPressPointCommand { get; private set; }
        public ICommand? PanCommand { get; private set; }
        public ICommand<PanArgs>? PanPointCommand { get; private set; }
        public ICommand? PinchCommand { get; private set; }
        public ICommand<PinchArgs>? PinchPointCommand { get; private set; }
        public ICommand<SwipeArgs>? SwipeCommand { get; private set; }
    
        public System.Windows.Input.ICommand? ResetCommand { get; private set; }
        public System.Windows.Input.ICommand? ToggleModeCommand { get; private set; }

        public MainViewModel()
        {
            IsAdvancedMode = false;
            GestureText = "Interact with me!";
            GestureInfo = "Basic mode shows the gesture type only.";

            TapCommand = new GestureCommand(() => UpdateGesture("Tapped!"), () => !IsAdvancedMode);
            RightTapCommand = new GestureCommand(() => UpdateGesture("Right Tapped!"), () => !IsAdvancedMode);
            DoubleTapCommand = new GestureCommand(() => UpdateGesture("Double Tapped!"), () => !IsAdvancedMode);
            LongPressCommand = new GestureCommand(() => UpdateGesture("Long Pressed!"), () => !IsAdvancedMode);
            PanCommand = new GestureCommand(() => UpdateGesture("Panned!"), () => !IsAdvancedMode);
            PinchCommand = new GestureCommand(() => UpdateGesture("Pinched!"), () => !IsAdvancedMode);

            TapPointCommand = new GestureCommand<PointArgs>(args => UpdateGesture($"Tapped at X:{args.Point.X:F2}, Y:{args.Point.Y:F2}"), args => IsAdvancedMode);
            RightTapPointCommand = new GestureCommand<PointArgs>(args => UpdateGesture($"Right Tapped at X:{args.Point.X:F2}, Y:{args.Point.Y:F2}"), args => IsAdvancedMode);
            DoubleTapPointCommand = new GestureCommand<PointArgs>(args => UpdateGesture($"Double Tapped at X:{args.Point.X:F2}, Y:{args.Point.Y:F2}"), args => IsAdvancedMode);
            LongPressPointCommand = new GestureCommand<PointArgs>(args => UpdateGesture($"Long Pressed at X:{args.Point.X:F2}, Y:{args.Point.Y:F2}"), args => IsAdvancedMode);
            PanPointCommand = new GestureCommand<PanArgs>(args => UpdateGesture($"Panned at X:{args.Point.X:F2}, Y:{args.Point.Y:F2}"), args => IsAdvancedMode);
            PinchPointCommand = new GestureCommand<PinchArgs>(args => UpdateGesture($"Pinched {args.Scale}"), args => IsAdvancedMode);

            SwipeCommand = new GestureCommand<SwipeArgs>(args => UpdateGesture($"Swiped {args.Direction} to position X:{args.Position.X:F2}, Y:{args.Position.Y:F2}"),args => IsAdvancedMode);

            ResetCommand = new Command(Reset);
            ToggleModeCommand = new Command(ToggleMode);
        }

        private void UpdateGesture(string gesture)
        {
            GestureText = gesture;
            BoxColor = Color.FromRgb(Random.Shared.NextDouble(), Random.Shared.NextDouble(), Random.Shared.NextDouble());
        }

        private void Reset()
        {
            GestureText = "Interact with me!";
            BoxColor = Colors.Blue;
        }

        private void ToggleMode()
        {
            IsAdvancedMode = !IsAdvancedMode;
            GestureInfo = IsAdvancedMode ? "Advanced Mode" : "Basic Mode";

            if (IsAdvancedMode)
            {
                GestureInfo = "Advanced mode allows you to see the exact position of the gesture.";
            }
            else
            {
                GestureInfo = "Basic mode shows the gesture type only.";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}