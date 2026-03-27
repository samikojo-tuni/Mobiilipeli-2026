using System;
using Godot;

public partial class Tutorial : Area2D
{
	[Export] private Control _tutorialText = null;

	public override void _EnterTree()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	public override void _ExitTree()
	{
		BodyEntered -= OnBodyEntered;
		BodyExited -= OnBodyExited;
	}

	public override void _Ready()
	{
		if (_tutorialText != null)
		{
			_tutorialText.Hide();
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is PlayerCharacter && _tutorialText != null)
		{
			_tutorialText.Show();
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body is PlayerCharacter && _tutorialText != null)
		{
			_tutorialText.Hide();
		}
	}
}
