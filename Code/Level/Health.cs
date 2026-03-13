using System;
using Godot;

public partial class Health : Node
{
	// Signal which is emitted whenever the health changes.
	[Signal] public delegate void HealthChangedEventHandler(int previousHealth, int currentHealth);
	[Export] private int _maxHealth = 3;
	[Export] private int _initialHealth = 3;

	private int _currentHealth = 0;

	public int CurrentHealth
	{
		get { return _currentHealth; }
		set
		{
			int previous = _currentHealth;
			_currentHealth = Mathf.Clamp(value, 0, _maxHealth);
			// Emits the signal about the health change.
			EmitSignal(SignalName.HealthChanged, previous, _currentHealth);
		}
	}

	public bool IsImmortal
	{
		get;
		set;
	} = false;

	public int MaxHealth
	{
		get => _maxHealth;
	}

	public bool IsAlive
	{
		get { return CurrentHealth > 0; }
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Reset();
	}

	/// <summary>
	/// Resets health points back to their initial values.
	/// </summary>
	public void Reset()
	{
		CurrentHealth = _initialHealth;
	}

	/// <summary>
	/// Damages the character by <paramref name="amount"/>
	/// </summary>
	/// <param name="amount">The damage dealt to the character. The value can't be negative.</param>
	/// <returns>True, if the damage was succesfully delivered. False otherwise.</returns>
	public bool TakeDamage(int amount)
	{
		if (amount < 0)
		{
			GD.PushError("Negative amount is not allowed when taking damage.");
			return false;
		}

		CurrentHealth -= amount;

		return true;
	}

	/// <summary>
	/// Heals the character by <paramref name="amount"/>
	/// </summary>
	/// <param name="amount">The amount of health added to the character. Negative values are not allowed.</param>
	/// <returns>True, if healing was successful. False otherwise.</returns>
	public bool Heal(int amount)
	{
		if (amount < 0)
		{
			GD.PushError("Negative amount is not allowed when healing.");
			return false;
		}

		CurrentHealth += amount;
		return true;
	}
}
