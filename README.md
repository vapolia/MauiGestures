[![NuGet](https://img.shields.io/nuget/v/Vapolia.MauiGesture.svg?style=for-the-badge)](https://www.nuget.org/packages/Vapolia.MauiGesture/)  
[![NuGet](https://img.shields.io/nuget/vpre/Vapolia.MauiGesture.svg?style=for-the-badge)](https://www.nuget.org/packages/Vapolia.MauiGesture/)  
![Nuget](https://img.shields.io/nuget/dt/Vapolia.MauiGesture)

# Supported Platforms

iOS, Android, Windows, Mac

# Maui Gesture Effects

Add "advanced" gestures to Maui. Available on all views.
Most gesture commands include the event position.  
Combine this feature with `UserInteraction.Menu()` (from [this nuget](https://github.com/softlion/UserInteraction/)) to display a standart menu at the position of the finger. Useful especially for tablets. See the demo app in this repo on how to do it.

```xaml
    <Label
       Text="Click here"
       ui:Gesture.TapCommand="{Binding OpenLinkCommand}"
       ui:Gesture.CommandParameter="{Binding .}" />
```
`CommandParameter` is optional.

Or in code:
```csharp
    var label = new Label();
    Gesture.SetTapCommand(label, new Command(() => { /*your code*/ }));
```

# Quick start
Add the above nuget package to your Maui project   
then add this line to your maui app builder:

```c#
using MauiGestures;
...
builder.UseAdvancedGestures();
``` 

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

Only commands are supported (PR welcome for events). No .NET events. So you must use the MVVM pattern.

Swipe commands are not supported on Windows because of a curious bug (event not received). If you find it, notify me!
PinchCommand is not supported (yet) on Windows. PR welcome.

If your command is not receiving events, make sure that:
- you used the correct handler. Ie: the `LongPressPointCommand` should be `new Command<PointEventArgs>(args => ...)`
- you set `IsEnabled="True"` and `InputTransparent="False"` on the element

Windows requires the fall creator update.  


![Alt](https://repobeats.axiom.co/api/embed/8b815aadebdd267fc06d925b4c7482bed6b7b715.svg "Repobeats analytics image")
