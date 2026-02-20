using System;
using Godot;

/// <summary>
/// This Node follows the target node on the screen.
/// </summary>
public partial class FollowCamera : Camera2D
{
	[Export] private Node2D _target = null;
	[Export] private float _speed = 5f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_target == null)
		{
			GD.PushWarning("FollowCamera: Target is not set!");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_target == null)
		{
			// There's no target, no can do!
			return;
		}

		if (ProcessCallback == Camera2DProcessCallback.Idle)
		{
			UpdatePosition((float)delta);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_target == null)
		{
			// There's no target, no can do!
			return;
		}

		if (ProcessCallback == Camera2DProcessCallback.Physics)
		{
			UpdatePosition((float)delta);
		}
	}

	private void UpdatePosition(float deltaTime)
	{
		// Target's position in the game world in global coordinates.
		Vector2 targetPosition = _target.GlobalPosition;
		Vector2 currentPosition = GlobalPosition;
		Vector2 newPosition = currentPosition.Lerp(targetPosition, _speed * deltaTime);
		GlobalPosition = newPosition;
	}

	public void SetTarget(Node2D newTarget)
	{
		if (newTarget != null)
		{
			_target = newTarget;
		}
	}
}
