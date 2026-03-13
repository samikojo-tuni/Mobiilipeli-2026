using System;
using Godot;

namespace GA.Common.UI;

public partial class VirtualButton : Node2D
{
	[Export] private TouchScreenButton _button = null;
	[Export] private StringName _actionName = "ui_accept";
	[Export] private bool _useOnlyInBuild = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_button == null)
		{
			GD.PrintErr("Button not assigned in VirtualButton!");
		}
	}

	public override void _EnterTree()
	{
		base._EnterTree();

		if (_button != null)
		{
			_button.Pressed += OnButtonDown;
			_button.Released += OnButtonUp;
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		if (_button != null)
		{
			_button.Pressed -= OnButtonDown;
			_button.Released -= OnButtonUp;
		}
	}


	private void OnButtonUp()
	{
		if (_useOnlyInBuild && OS.HasFeature("editor"))
		{
			return;
		}

		if (InputMap.HasAction(_actionName))
		{
			Input.ActionRelease(_actionName);
		}
	}


	private void OnButtonDown()
	{
		if (_useOnlyInBuild && OS.HasFeature("editor"))
		{
			return;
		}

		if (InputMap.HasAction(_actionName))
		{
			Input.ActionPress(_actionName);
		}
	}
}
