using System;
using Godot;

namespace GA.Common.UI;

public partial class VirtualKnob : Sprite2D
{
	[Export] private TouchScreenButton _joystickButton = null;
	[Export] private float _maxLength = 50.0f;
	[Export] private float _deadZone = 5.0f;
	[Export] private float _releaseSpeed = 10.0f;

	public bool IsPressing
	{
		get;
		private set;
	} = false;

	public VirtualJoystick Parent { get; private set; } = null;

	// Parent's position is its top-left corner. We want its center.
	public Vector2 ParentPosition => Parent.GetGlobalPosition();

	public override void _Ready()
	{
		if (_joystickButton == null)
		{
			GD.PrintErr("Joystick Button not assigned in Knob!");
		}

		Parent = GetParent<VirtualJoystick>();
		if (Parent == null)
		{
			GD.PrintErr("Knob's parent is not a Joystick!");
		}
	}

	public override void _EnterTree()
	{
		base._EnterTree();

		if (_joystickButton != null)
		{
			_joystickButton.Pressed += OnButtonDown;
			_joystickButton.Released += OnButtonUp;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		if (_joystickButton != null)
		{
			_joystickButton.Pressed -= OnButtonDown;
			_joystickButton.Released -= OnButtonUp;
		}
	}

	private void OnButtonDown()
	{
		IsPressing = true;
	}

	private void OnButtonUp()
	{
		IsPressing = false;
	}

	public override void _Process(double delta)
	{
		if (IsPressing)
		{
			Vector2 mousePosition = GetGlobalMousePosition();

			float distance = mousePosition.DistanceTo(ParentPosition);
			if (distance <= _maxLength)
			{
				GlobalPosition = mousePosition;
			}
			else
			{
				float angle = ParentPosition.AngleToPoint(mousePosition);
				float x = ParentPosition.X + Mathf.Cos(angle) * _maxLength;
				float y = ParentPosition.Y + Mathf.Sin(angle) * _maxLength;
				GlobalPosition = new Vector2(x, y);
			}

			Parent.RelativePosition = CalculatePosition();
		}
		else
		{
			GlobalPosition = GlobalPosition.Lerp(ParentPosition, (float)delta * _releaseSpeed);
			Parent.RelativePosition = Vector2.Zero;
		}
	}

	private Vector2 CalculatePosition()
	{
		Vector2 offset = GlobalPosition - ParentPosition;
		if (offset.Length() < _deadZone)
		{
			return Vector2.Zero;
		}

		Vector2 normalizedPosition = Vector2.Zero;
		normalizedPosition.X = (GlobalPosition.X - ParentPosition.X) / _maxLength;
		normalizedPosition.Y = (GlobalPosition.Y - ParentPosition.Y) / _maxLength;
		return normalizedPosition;
	}
}
