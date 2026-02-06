using System;
using Godot;

public partial class PlayerCharacter : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

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
			velocity.Y = JumpVelocity;
			_isJumping = false;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.

		if (!Mathf.IsZeroApprox(_horizontalMovement))
		{
			velocity.X = _horizontalMovement * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		// "Käytä" luettu syöte.
		_horizontalMovement = 0;

		Velocity = velocity;
		MoveAndSlide();
	}
}
