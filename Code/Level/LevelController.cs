using System;
using Godot;

public partial class LevelController : Node2D
{
	// TODO: Täällä tulee olemaan tasoon liittyvää logiikkaa.

	[Export] private PlayerCharacter _player = null;
	[Export] private UIHealthBar _healthBar = null;

	private Vector2 _spawnPoint = Vector2.Zero;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Register the level to the Game Manager. This will become the active level.
		GameManager.Instance.RegisterLevel(this);

		if (_healthBar != null && _player != null)
		{
			_healthBar.Setup(_player.Health);
		}

		if (_player != null)
		{
			_spawnPoint = _player.GlobalPosition;
		}
	}

	public override void _ExitTree()
	{
		if (_healthBar != null && _player != null)
		{
			_healthBar.Dispose(_player.Health);
		}
	}

	public override void _Process(double delta)
	{
		if (OS.HasFeature("editor"))
		{
			if (Input.IsActionJustPressed("SetFI"))
			{
				GameManager.Instance.SetLocale("fi");
			}

			if (Input.IsActionJustPressed("SetEN"))
			{
				GameManager.Instance.SetLocale("en");
			}
		}
	}

	public void Respawn()
	{
		_player.GlobalPosition = _spawnPoint;
	}
}
