using System;
using Godot;

/// <summary>
/// A node which moves along the path.
/// </summary>
public partial class PathFollower : PathFollow2D
{
	// Unit: px/second
	[Export] private float _speed = 0;

	// The length of the path. A negative value indicates that it hasn't been computed yet.
	private float _pathLength = -1;
	private float _moveSpeed = 0;

	/// <summary>
	/// Direction of movement along the path. 1 for right, -1 for left.
	/// </summary>
	public int Direction
	{
		get;
		private set;
	} = 1;

	/// <summary>
	/// Indicates whether the PathFollower can move.
	/// </summary>
	[Export]
	public bool CanMove
	{
		get;
		set;
	} = true;

	public override void _Process(double delta)
	{
		if (!CanMove || !ValidatePath())
		{
			return;
		}

		// deltaRatio: Montako %-yksikköä polulla liikutaan tämän framen aikana.
		float deltaRatio = Direction * _moveSpeed * (float)delta;
		ProgressRatio = Mathf.Clamp(ProgressRatio + deltaRatio, 0, 1);

		if (ProgressRatio >= 1 || ProgressRatio <= 0)
		{
			Direction *= -1;
		}
	}

	private bool ValidatePath()
	{
		if (_pathLength >= 0)
		{
			return true;
		}

		Path2D path = GetParent<Path2D>();
		if (path == null || path.Curve == null)
		{
			return false;
		}

		_pathLength = path.Curve.GetBakedLength();
		_moveSpeed = _speed / _pathLength;

		GD.Print($"Path length: {_pathLength}");
		GD.Print($"Move Speed: {_moveSpeed}");

		return true;
	}
}
