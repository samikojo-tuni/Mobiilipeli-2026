using System;
using Godot;

public partial class UIHealthBar : HBoxContainer
{
	[Export] private PackedScene _heartScene = null;
	private Control[] _hearts = null;

	public void Setup(Health health)
	{
		// MaxHealth defines the max. amount of hearts.
		_hearts = new Control[health.MaxHealth];

		for (int i = 0; i < _hearts.Length; ++i)
		{
			Control heart = _heartScene.Instantiate<Control>();
			_hearts[i] = heart;
			AddChild(heart);
		}

		health.HealthChanged += OnHealthChanged;
	}

	public void Dispose(Health health)
	{
		health.HealthChanged -= OnHealthChanged;
	}

	private void OnHealthChanged(int previousHealth, int currentHealth)
	{
		for (int i = 0; i < _hearts.Length; i++)
		{
			_hearts[i].Visible = i < currentHealth;
		}
	}
}
