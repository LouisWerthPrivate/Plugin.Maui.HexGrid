# Plugin.Maui.HexGrid

Plugin.Maui.HexGrid is a .NET MAUI control library for rendering adaptive honeycomb layouts with pointy-top hexagons using GraphicsView and IDrawable.

## Current Scope

- Target frameworks: net10.0-android and net10.0-ios
- Rendering: Microsoft.Maui.Graphics via GraphicsView + IDrawable
- Architecture: MVVM-friendly ItemsSource, SelectedItem, and tap command support
- Layout: adaptive row-wrapping with staggered rows and orientation-aware relayout
- Styling: control-level fallback colors plus per-item color binding paths
- Performance direction: single GraphicsView surface with cached hex geometry

## Install

```bash
dotnet add package Plugin.Maui.HexGrid
```

## Usage

Register the control package during app startup:

```csharp
using Plugin.Maui.HexGrid;

builder.UseHexGrid();
```

Bind the HexGridView to your view model:

```xml
<hex:HexGridView
    ItemsSource="{Binding Items}"
    PreviewTextPath="Preview"
    FillColorPath="{Binding FillColorPath}"
    SelectedFillColorPath="{Binding SelectedFillColorPath}"
    StrokeColorPath="{Binding StrokeColorPath}"
    TextColorPath="{Binding TextColorPath}"
    FillColor="{Binding DefaultFillColor}"
    SelectedFillColor="{Binding DefaultSelectedFillColor}"
    StrokeColor="{Binding DefaultStrokeColor}"
    TextColor="{Binding DefaultTextColor}"
    SelectedItem="{Binding SelectedItem, Mode=TwoWay}"
    HexTappedCommand="{Binding HexTappedCommand}"
    MinHexSize="34"
    MaxHexSize="64"
    HexSize="30"
    HexSpacing="0"
    MinColumns="1"
    MaxColumns="0"
    StartWithOffsetRow="False"
    StrokeThickness="4"
    PreviewFontSize="13" />
```

## Screenshot

![Plugin.Maui.HexGrid sample](docs/images/Simulator%20Screenshot%20-%20iPhone%2017%20Pro%20Max%20-%202026-03-15%20at%2017.24.03.png)

## Public API

The current implementation exposes these key properties:

- ItemsSource
- SelectedItem
- PreviewTextPath
- CommandParameterPath
- FillColorPath
- SelectedFillColorPath
- StrokeColorPath
- TextColorPath
- HexTappedCommand
- HexSize
- MinHexSize
- MaxHexSize
- HexSpacing
- AdaptiveLayout
- MinColumns
- MaxColumns
- OverscanRows
- FillColor
- SelectedFillColor
- StrokeColor
- TextColor
- PreviewFontSize
- StrokeThickness
- StartWithOffsetRow

Item-level colors can be supplied through the color path properties, while the view-level color properties remain available as fallbacks.

The control automatically recalculates layout when its size changes, including device rotation.

## Repository Layout

- src: library source and source solution
- samples: MAUI sample app and sample solution
- .github/workflows: CI and package workflow scaffolding

## Status

The control surface, layout engine, hit testing, adaptive wrapping, and sample host are in place. The next likely steps are geometry-focused tests, additional item templating and styling hooks, and behavior validation under larger datasets.
