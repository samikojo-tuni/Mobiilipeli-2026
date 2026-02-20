using System;
using Godot;

public partial class PlayerCharacter : CharacterBody2D
{
	[Export]
	private float _speed = 300.0f;

	[Export]
	private float _jumpVelocity = -400.0f;

	private float _horizontalMovement = 0;
	private bool _isJumping = false;

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed(InputConfig.InputJump) && IsOnFloor())
		{
			_isJumping = true;
		}
	}

	public override void _Process(double delta)
	{
		_horizontalMovement = Input.GetAxis(InputConfig.InputLeft, InputConfig.InputRight);
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
}
