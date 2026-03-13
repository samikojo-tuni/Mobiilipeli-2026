using System;
using Godot;

public partial class LevelController : Node2D
{
	// TODO: Täällä tulee olemaan tasoon liittyvää logiikkaa.

	[Export] private PlayerCharacter _player = null;
	[Export] private UIHealthBar _healthBar = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Register the level to the Game Manager. This will become the active level.
		GameManager.Instance.RegisterLevel(this);

		if (_healthBar != null && _player != null)
		{
			_healthBar.Setup(_player.Health);
		}
	}

	public override void _ExitTree()
	{
		if (_healthBar != null && _player != null)
		{
			_healthBar.Dispose(_player.Health);
		}
	}
}
