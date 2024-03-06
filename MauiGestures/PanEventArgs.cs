﻿namespace MauiGestures;

public class PanEventArgs(GestureStatus status, Point point)
{
    public GestureStatus Status { get; } = status;
    public Point Point { get; } = point;
    public bool CancelGesture { get; set; }
}