[![NuGet](https://img.shields.io/nuget/v/Vapolia.MauiGesture.svg?style=for-the-badge)](https://www.nuget.org/packages/Vapolia.MauiGesture/)  
[![NuGet](https://img.shields.io/nuget/vpre/Vapolia.MauiGesture.svg?style=for-the-badge)](https://www.nuget.org/packages/Vapolia.MauiGesture/)  
![Nuget](https://img.shields.io/nuget/dt/Vapolia.MauiGesture)

# Supported Platforms

iOS, MacOS, Android, Windows 

# Maui Gesture Effects

Add "advanced" gestures to Maui. Available on all views.
Most gesture commands include the event position.  
Combine this feature with `UserInteraction.Menu()` (from [this nuget](https://github.com/softlion/UserInteraction/)) to display a standart menu at the position of the finger. Useful especially for tablets. See the demo app in this repo on how to do it.

```xaml
    <Label
       Text="Click here"
       ui:Gesture.TapCommand="{Binding OpenLinkCommand}"
       ui:Gesture.CommandParameter="{Binding .}" />
       
    <!-- Or using events -->
    <Label
       Text="Click here"
       ui:Gesture.Tap="OnLabelTapped" />
```
`CommandParameter` is optional.

Or in code:
```csharp
    var label = new Label();
    Gesture.SetTapCommand(label, new Command(() => { /*your code*/ }));

    // Or using events
    Gesture.SetTapEvent(label, (sender, e) => { /*your code*/ });
```

# Quick start
Add the above nuget package to your Maui project   
Gestures are now automatically enabled when you use gesture properties - no additional setup required!

The view on which the gesture is applied should have the property `InputTransparent="False"` which activates user interaction on it. If the view is still not receiving tap events, try adding a background color. That forces Maui to wrap some controls in an invisible container.

# Examples

Add Gesture.TapCommand on any supported xaml view:
```xaml
        <StackLayout ui:Gesture.TapCommand="{Binding OpenLinkCommand}">
            <Label Text="1.Tap this to open an url"  />
        </StackLayout>
```
Declare the corresponding namespace:
```xaml
    <ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             ...
             xmlns:ui="clr-namespace:MauiGestures;assembly=MauiGestures">
```
And in the viewmodel:
   ```csharp     
    public Command OpenLinkCommand => new Command(() =>
    {
        //do something
    });
```
# Supported Gestures

## Commands

 *  `TapCommand (ICommand or Command<YourClass>)` if `CommandParameter` is set (see below)
 *  `DoubleTapCommand (ICommand) or Command<YourClass>` if `CommandParameter` is set (see below)
 *  `PanCommand (ICommand) or Command<YourClass>` if `CommandParameter` is set (see below)
 *  `LongPressCommand (ICommand) or Command<YourClass>` if `CommandParameter` is set (see below)
 *  `TapPointCommand (ICommand or Command<PointEventArgs>)`
 *  `DoubleTapPoinCommand (ICommand or Command<PointEventArgs>)`
 *  `PanPointCommand (ICommand or Command<PanEventArgs>)`
 *  `LongPressPointCommand (ICommand or Command<PointEventArgs>)`
 *  `SwipeLeftCommand (ICommand) or Command<YourClass>` if `CommandParameter` is set (see below)
 *  `SwipeRightCommand (ICommand) or Command<YourClass>` if `CommandParameter` is set (see below)
 *  `SwipeTopCommand (ICommand) or Command<YourClass>` if `CommandParameter` is set (see below)
 *  `SwipeBottomCommand (ICommand) or Command<YourClass>` if `CommandParameter` is set (see below)
 *  `PinchCommand (Command<PinchEventArgs>)` where `PinchEventArg` contains `StartingPoints`, `CurrentPoints`, `Center`, `Scale`, `RotationRadians`, `RotationDegrees`, `Status`

## Events

In addition to commands, you can use events:

 *  `TapEvent (EventHandler)`
 *  `DoubleTapEvent (EventHandler)`
 *  `LongPressEvent (EventHandler)`
 *  `PanEvent (EventHandler)`
 *  `PinchEvent (EventHandler<PinchEventArgs>)`
 *  `SwipeLeftEvent (EventHandler)`
 *  `SwipeRightEvent (EventHandler)`
 *  `SwipeTopEvent (EventHandler)`
 *  `SwipeBottomEvent (EventHandler)`
 *  `TapPointEvent (EventHandler<PointEventArgs>)`
 *  `DoubleTapPointEvent (EventHandler<PointEventArgs>)`
 *  `LongPressPointEvent (EventHandler<PointEventArgs>)`
 *  `PanPointEvent (EventHandler<PanEventArgs>)`

Events are triggered in addition to commands, so you can use both simultaneously if needed.

`PointEventArgs` contains the absolute tap position relative to the view, the instance of the control triggering the command, and the BindingContext associated with that control. With that feature, the gestures can easily be used on `CollectionView`'s items.
 
 Properties:
 
 * `IsPanImmediate` Set to true to receive the PanCommand or PanPointCommand event on touch down, instead of after a minimum move distance. Default to `false`.

## Using Command Parameters

Important note:  
You can not set a binding in the main command's parameter. Even if it is accepted and no error is displayed, the resulting parameter will always be null. That's a maui limiation.  
Instead, you should use the MauiGesture's `CommandParameter` attached property:

If you define the `CommandParameter` property, some gestures will callback the command with this parameter's value.  
Example:

```c#
<ContentPage x:Name="ThePage" ...>
    <CollectionView ...>
        <CollectionView.ItemTemplate>
            <DataTemplate>
                    <Grid
                      ui:Gesture.TapCommand="{Binding BindingContext.MyItemTappedCommand, Source={x:Reference ThePage}}"
                      ui:Gesture.CommandParameter="{Binding .}">
                        <Label Text="{Binding SomeText}" />
                    </Grid>
            </DataTemplate>
        </CollectionView.ItemTemplate>
    </CollectionView>
</ContentPage
```

Note that the above example can be simplified by using `TapPointCommand` instead of `TapCommand`. `TapPointCommand` already provides the BindingContext in its `PointEventArgs` parameter to your command.


 
# Examples

## Some commands in XAML

```xml
<VerticalStackLayout ui:Gesture.TapCommand="{Binding OpenCommand}" IsEnabled="True">
    <Label Text="1.Tap this text to open an url" />
</VerticalStackLayout>

<VerticalStackLayout ui:Gesture.DoubleTapPointCommand="{Binding OpenPointCommand}" IsEnabled="True">
    <Label Text="2.Double tap this text to open an url" />
</VerticalStackLayout>

<BoxView
    ui:Gesture.PanPointCommand="{Binding PanPointCommand}"
    HeightRequest="200" WidthRequest="300"
    InputTransparent="False"
    IsEnabled="True"
     />
```

In the viewmodel:

```csharp
public ICommand OpenCommand => new Command(async () =>
{
   //...
});

public ICommand OpenPointCommand => new Command<PointEventArgs>(args =>
{
    var point = args.Point;
    PanX = point.X;
    PanY = point.Y;
    //...
});

public ICommand PanPointCommand => new Command<PanEventArgs>(args =>
{
    var point = args.Point;
    PanX = point.X;
    PanY = point.Y;
    //...
});

```

## Using Events in Code-Behind

You can also use events instead of commands, which is useful when not using MVVM or when you prefer event-driven programming:

```csharp
public partial class MyPage : ContentPage
{
    public MyPage()
    {
        InitializeComponent();

        // Set up gesture events
        Gesture.SetTapEvent(myLabel, OnLabelTapped);
        Gesture.SetTapPointEvent(myLabel, OnLabelTappedWithPoint);
        Gesture.SetPanPointEvent(myBoxView, OnBoxViewPanned);
        Gesture.SetPinchEvent(myImage, OnImagePinched);
        Gesture.SetSwipeLeftEvent(myView, OnSwipeLeft);
        Gesture.SetLongPressEvent(myButton, OnLongPress);
    }

    private void OnLabelTapped(object sender, EventArgs e)
    {
        // Handle tap
        DisplayAlert("Tap", "Label was tapped!", "OK");
    }

    private void OnLabelTappedWithPoint(object sender, PointEventArgs e)
    {
        // Handle tap with position information
        DisplayAlert("Tap", $"Tapped at {e.Point.X}, {e.Point.Y}", "OK");
    }

    private void OnBoxViewPanned(object sender, PanEventArgs e)
    {
        // Handle pan gesture
        if (e.Status == GestureStatus.Running)
        {
            // Update UI based on pan position
            myBoxView.TranslationX = e.Point.X;
            myBoxView.TranslationY = e.Point.Y;
        }
    }

    private void OnImagePinched(object sender, PinchEventArgs e)
    {
        // Handle pinch gesture
        if (e.Status == GestureStatus.Running)
        {
            myImage.Scale = e.Scale;
            myImage.Rotation = e.RotationDegrees;
        }
    }

    private void OnSwipeLeft(object sender, EventArgs e)
    {
        // Handle swipe left
        DisplayAlert("Swipe", "Swiped left!", "OK");
    }

    private void OnLongPress(object sender, EventArgs e)
    {
        // Handle long press
        DisplayAlert("Long Press", "Button was long pressed!", "OK");
    }
}
```

## Using Events in XAML (with code-behind)

You can also set up events directly in XAML using the attached event properties:

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:ui="clr-namespace:MauiGestures;assembly=MauiGestures"
             x:Class="MyApp.MainPage">

    <StackLayout>
        <Label Text="Tap me!"
               ui:Gesture.Tap="OnLabelTapped" />

        <Label Text="Double tap me!"
               ui:Gesture.DoubleTap="OnLabelDoubleTapped" />

        <Label Text="Long press me!"
               ui:Gesture.LongPress="OnLabelLongPressed" />

        <BoxView BackgroundColor="Blue"
                 HeightRequest="100"
                 ui:Gesture.PanPoint="OnBoxViewPanned" />

        <Image Source="myimage.png"
               ui:Gesture.Pinch="OnImagePinched" />

        <StackLayout ui:Gesture.SwipeLeft="OnSwipeLeft"
                     ui:Gesture.SwipeRight="OnSwipeRight"
                     BackgroundColor="Yellow"
                     HeightRequest="50">
            <Label Text="Swipe left or right on this area" />
        </StackLayout>
    </StackLayout>

</ContentPage>
```

And in the code-behind file (MainPage.xaml.cs):

```csharp
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnLabelTapped(object sender, EventArgs e)
    {
        DisplayAlert("Tap", "Label was tapped!", "OK");
    }

    private void OnLabelDoubleTapped(object sender, EventArgs e)
    {
        DisplayAlert("Double Tap", "Label was double tapped!", "OK");
    }

    private void OnLabelLongPressed(object sender, EventArgs e)
    {
        DisplayAlert("Long Press", "Label was long pressed!", "OK");
    }

    private void OnBoxViewPanned(object sender, PanEventArgs e)
    {
        if (e.Status == GestureStatus.Running)
        {
            // Update UI based on pan position
            ((BoxView)sender).TranslationX = e.Point.X;
            ((BoxView)sender).TranslationY = e.Point.Y;
        }
    }

    private void OnImagePinched(object sender, PinchEventArgs e)
    {
        if (e.Status == GestureStatus.Running)
        {
            var image = (Image)sender;
            image.Scale = e.Scale;
            image.Rotation = e.RotationDegrees;
        }
    }

    private void OnSwipeLeft(object sender, EventArgs e)
    {
        DisplayAlert("Swipe", "Swiped left!", "OK");
    }

    private void OnSwipeRight(object sender, EventArgs e)
    {
        DisplayAlert("Swipe", "Swiped right!", "OK");
    }
}
```

Note: Events are triggered in addition to commands, so you can use both simultaneously if needed.

### Available Event Properties for XAML

For XAML usage, use these attached event properties:

- `ui:Gesture.Tap` - Simple tap event
- `ui:Gesture.DoubleTap` - Double tap event
- `ui:Gesture.LongPress` - Long press event
- `ui:Gesture.Pan` - Pan gesture event (simple)
- `ui:Gesture.Pinch` - Pinch gesture event
- `ui:Gesture.SwipeLeft` - Swipe left event
- `ui:Gesture.SwipeRight` - Swipe right event
- `ui:Gesture.SwipeTop` - Swipe up event
- `ui:Gesture.SwipeBottom` - Swipe down event
- `ui:Gesture.TapPoint` - Tap with position information
- `ui:Gesture.DoubleTapPoint` - Double tap with position information
- `ui:Gesture.LongPressPoint` - Long press with position information
- `ui:Gesture.PanPoint` - Pan with position and status information

## Exemple on a Grid containing an horizontal slider (set value on tap)

```csharp
//Tap anywhere to set value
Gesture.SetTapPointCommand(this, new Command<PointEventArgs>(args =>
{
    var pt = args.Point;
    var delta = (pt.X - Padding.Left) / (Width - Padding.Left - Padding.Right);
    if(delta<0 || delta>1)
        return;
    Value = (int)Math.Round((Maximum - Minimum) * delta);
}));
```
        

# Limitations

Swipe commands are not supported on Windows because of a curious bug (event not received). If you find it, notify me!
PinchCommand is not supported (yet) on Windows. PR welcome.

If your command is not receiving events, make sure that:
- you used the correct handler. Ie: the `LongPressPointCommand` should be `new Command<PointEventArgs>(args => ...)`
- you set `IsEnabled="True"` and `InputTransparent="False"` on the element (and InputTransparent="True" on all its children)

Windows requires the fall creator update.  


![Alt](https://repobeats.axiom.co/api/embed/8b815aadebdd267fc06d925b4c7482bed6b7b715.svg "Repobeats analytics image")
