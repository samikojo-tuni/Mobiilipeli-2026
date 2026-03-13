using System;
using Godot;

public partial class PlayerCharacter : CharacterBody2D
{
	[Export]
	private float _speed = 300.0f;

	[Export]
	private float _jumpVelocity = -400.0f;

	[Export] private double _damageTime = 1f;

	private float _horizontalMovement = 0;
	private bool _isJumping = false;
	private Timer _timer = null;
	private AnimatedSprite2D _animatedSprite = null;

	public Health Health
	{
		get;
		private set;
	}

	public override void _Ready()
	{
		Health = GetNode<Health>("Health");
		if (Health == null)
		{
			GD.PushError("Can't find the Health node");
		}
		else
		{
			Health.HealthChanged += OnHealthChanged;
		}

		_timer = GetNode<Timer>("Timer");
		if (_timer == null)
		{
			GD.PushError("Can't find the Timer node");
		}

		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (_animatedSprite == null)
		{
			GD.PushError("Can't find the AnimatedSprite2D node");
		}
	}

	public override void _Process(double delta)
	{
		_horizontalMovement = Input.GetAxis(InputConfig.InputLeft, InputConfig.InputRight);

		if (Input.IsActionJustPressed(InputConfig.InputJump) && IsOnFloor())
		{
			_isJumping = true;
		}

		UpdateAnimations();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (_isJumping)
		{
			velocity.Y = -1 * _jumpVelocity;
			_isJumping = false;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.

		if (!Mathf.IsZeroApprox(_horizontalMovement))
		{
			velocity.X = _horizontalMovement * _speed;
			_animatedSprite.FlipH = _horizontalMovement < 0;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
		}

		// "Käytä" luettu syöte.
		_horizontalMovement = 0;

		Velocity = velocity;
		MoveAndSlide();
	}

	private void OnHealthChanged(int previousHealth, int currentHealth)
	{
		if (currentHealth <= 0)
		{
			Die();
		}
		else if (currentHealth < previousHealth && _timer != null)
		{
			// The character lost health
			_timer.Start(_damageTime);
			_timer.Timeout += OnTimerTimeout;
			Health.IsImmortal = true;
		}
	}

	private void OnTimerTimeout()
	{
		_timer.Timeout -= OnTimerTimeout;
		Health.IsImmortal = false;
	}

	private void Die()
	{
		GD.Print("Player character died!");
		// TODO: Replace me with a proper implementation.
	}

	private void UpdateAnimations()
	{
		if (Health.IsImmortal)
		{
			_animatedSprite.Play("damage");
		}
		else if (!IsOnFloor())
		{
			_animatedSprite.Play("jump");
		}
		else if (Mathf.IsZeroApprox(Velocity.X))
		{
			_animatedSprite.Play("idle");
		}
		else
		{
			_animatedSprite.Play("move");
		}
	}
}
