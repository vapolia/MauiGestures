using System.Windows.Input;
using MauiGestures;
using Vapolia.UserInteraction;
using Point = Microsoft.Maui.Graphics.Point;

namespace DemoApp.ViewModels;

public class MainPageViewModel(INavigation navigation) : BindableObject
{
    private Point pan, pinch;
    private GestureStatus? panStatus;
    private double rotation, scale;

    public Point Pan { get => pan; set { pan = value; OnPropertyChanged(); } }
    public GestureStatus? PanStatus { get => panStatus; set { panStatus = value; OnPropertyChanged(); } }
    public Point Pinch { get => pinch; set { pinch = value; OnPropertyChanged(); } }
    public double Rotation { get => rotation; set { rotation = value; OnPropertyChanged(); } }
    public double Scale { get => scale; set { scale = value; OnPropertyChanged(); } }

    public ICommand PanPointCommand => new Command<PanEventArgs>(args =>
    {
        var point = args.Point;
        Pan = point;
        PanStatus = args.Status;
    });
        
    public ICommand PinchCommand => new Command<PinchEventArgs>(args =>
    {
        Pinch = args.Center;
        Rotation = args.RotationDegrees;
        Scale = args.Scale;
    });

    public ICommand TextSwipedCommand => new Command(async () =>
    {
        var message = "Swipe gesture detected";
        await CommunityToolkit.Maui.Alerts.Toast.Make(message).Show();

        // await navigation.PushAsync(new ContentPage {
        //     Title = "Web",
        //     Content = new Grid {
        //         BackgroundColor = Yellow,
        //         Children = { new WebView { Source = new UrlWebViewSource { Url = "https://vapolia.fr" }, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill} }}});
    });
    
    public ICommand OnTapCommand => new Command(async () =>
    {
        var message = "Tap command received";
        await CommunityToolkit.Maui.Alerts.Toast.Make(message).Show();
        
        // await navigation.PushAsync(new ContentPage {
        //     Title = "Web",
        //     Content = new Grid {
        //         BackgroundColor = Yellow,
        //         Children = { new WebView { Source = new UrlWebViewSource { Url = "https://vapolia.fr" }, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill} }}});
    });

    public ICommand OpenDoubleTapPointCommand => new Command<PointEventArgs>(args => _ = OpenPointCommand(args, "Double tap"));
    public ICommand OpenLongPressPointCommand => new Command<PointEventArgs>(args => _ = OpenPointCommand(args, "Long press"));
    
        
    async Task OpenPointCommand(PointEventArgs args, string title)
    {
        Pan = args.Point;
        var absXy = args.GetCoordinates();
        var message = $"{title} received at position ({args.Point.X:0.0},{args.Point.Y:0.0}) relative to the element, and ({absXy.X:0.0},{absXy.Y:0.0}) relative to the root view";

        await CommunityToolkit.Maui.Alerts.Toast.Make(message).Show();
        
#if !WINDOWS
        await UserInteraction.Menu(default, position: args.GetAbsoluteBoundsF(), cancelButton: "Cancel", otherButtons: ["Do something"]);
#endif
    }
}