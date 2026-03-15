# Plugin.Maui.HexGrid

Plugin.Maui.HexGrid is a .NET MAUI control library for rendering adaptive honeycomb layouts with flat-top hexagons using GraphicsView and IDrawable.

## Current Scope

- Target frameworks: net10.0-android and net10.0-ios
- Rendering: Microsoft.Maui.Graphics via GraphicsView + IDrawable
- Architecture: MVVM-friendly ItemsSource, SelectedItem, and tap command support
- Layout: adaptive sizing with staggered rows
- Performance direction: single GraphicsView surface with viewport-aware rendering and cached hex geometry

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
    SelectedItem="{Binding SelectedItem, Mode=TwoWay}"
    HexTappedCommand="{Binding HexTappedCommand}" />
```

## Public API Direction

The current implementation exposes these key properties:

- ItemsSource
- SelectedItem
- PreviewTextPath
- CommandParameterPath
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

Preview text is truncated to a maximum of 3 visible characters.

## Repository Layout

- src: library source and source solution
- samples: MAUI sample app and sample solution
- .github/workflows: CI and package workflow scaffolding

## Status

This is the initial implementation pass. The control surface, layout engine, hit testing, and sample host are in place. The next likely steps are geometry-focused tests, more layout customization, and deeper virtualization tuning under large datasets.
