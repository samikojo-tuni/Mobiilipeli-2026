using Godot;

namespace GA.Common.UI;

public partial class VirtualControl : Node2D
{
	private enum ViewportCorner
	{
		TopLeft,
		TopRight,
		BottomLeft,
		BottomRight
	}

	[Export] private ViewportCorner _anchorCorner = ViewportCorner.BottomLeft;
	[Export] private Vector2 _anchorOffset = new(24f, 24f);
	[Export] private bool _useOnlyInBuild = false;

	public bool UseOnlyInBuild => _useOnlyInBuild;

	private void UpdateAnchoredPosition()
	{
		Vector2 viewportSize = GetViewportRect().Size;
		Vector2 basePosition = _anchorCorner switch
		{
			ViewportCorner.TopLeft => Vector2.Zero,
			ViewportCorner.TopRight => new Vector2(viewportSize.X, 0f),
			ViewportCorner.BottomLeft => new Vector2(0f, viewportSize.Y),
			ViewportCorner.BottomRight => viewportSize,
			_ => Vector2.Zero
		};

		Vector2 cornerAdjustedOffset = _anchorCorner switch
		{
			ViewportCorner.TopLeft => new Vector2(_anchorOffset.X, _anchorOffset.Y),
			ViewportCorner.TopRight => new Vector2(-_anchorOffset.X, _anchorOffset.Y),
			ViewportCorner.BottomLeft => new Vector2(_anchorOffset.X, -_anchorOffset.Y),
			ViewportCorner.BottomRight => new Vector2(-_anchorOffset.X, -_anchorOffset.Y),
			_ => _anchorOffset
		};

		GlobalPosition = basePosition + cornerAdjustedOffset;
	}

	public override void _Process(double delta)
	{
		UpdateAnchoredPosition();
	}
}