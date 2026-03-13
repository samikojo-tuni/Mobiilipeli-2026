using System;
using Godot;

namespace GA.Common.UI;

public partial class VirtualJoystick : Node2D
{
	[Export] private StringName _leftAction = "ui_left";
	[Export] private StringName _rightAction = "ui_right";
	[Export] private StringName _upAction = "ui_up";
	[Export] private StringName _downAction = "ui_down";
	[Export] private bool _useOnlyInBuild = false;

	public Vector2 RelativePosition { get; set; } = Vector2.Zero;

	public override void _Process(double delta)
	{
		if (_useOnlyInBuild && OS.HasFeature("editor"))
		{
			return;
		}

		HandleActionRelease();
		HandleActionPress();
	}

	private void HandleActionPress()
	{
		if (_useOnlyInBuild && OS.HasFeature("editor"))
		{
			return;
		}

		if (InputMap.HasAction(_leftAction) && RelativePosition.X < 0)
		{
			Input.ActionPress(_leftAction, -RelativePosition.X);
		}
		else if (InputMap.HasAction(_rightAction) && RelativePosition.X > 0)
		{
			Input.ActionPress(_rightAction, RelativePosition.X);
		}
		if (InputMap.HasAction(_upAction) && RelativePosition.Y < 0)
		{
			Input.ActionPress(_upAction, -RelativePosition.Y);
		}
		else if (InputMap.HasAction(_downAction) && RelativePosition.Y > 0)
		{
			Input.ActionPress(_downAction, RelativePosition.Y);
		}
	}


	private void HandleActionRelease()
	{
		if (_useOnlyInBuild && OS.HasFeature("editor"))
		{
			return;
		}

		if (InputMap.HasAction(_leftAction) && RelativePosition.X >= 0 && Input.IsActionPressed(_leftAction))
		{
			Input.ActionRelease(_leftAction);
		}
		if (InputMap.HasAction(_rightAction) && RelativePosition.X <= 0 && Input.IsActionPressed(_rightAction))
		{
			Input.ActionRelease(_rightAction);
		}
		if (InputMap.HasAction(_upAction) && RelativePosition.Y >= 0 && Input.IsActionPressed(_upAction))
		{
			Input.ActionRelease(_upAction);
		}
		if (InputMap.HasAction(_downAction) && RelativePosition.Y <= 0 && Input.IsActionPressed(_downAction))
		{
			Input.ActionRelease(_downAction);
		}
	}
}
