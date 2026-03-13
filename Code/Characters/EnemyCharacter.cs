using System;
using Godot;
using Godot.Collections;

public partial class EnemyCharacter : Area2D
{
	[Export] private float _bounceForce = -200f;
	[Export] private Array<Area2D> _damagerAreas = new();

	private AnimatedSprite2D _animatedSprite = null;
	private PathFollower _pathFollower = null;

	public Health Health
	{
		get;
		private set;
	}

	public override void _EnterTree()
	{
		BodyEntered += OnBodyEntered;

		foreach (Area2D item in _damagerAreas)
		{
			item.BodyEntered += OnDamagerBodyEntered;
		}
	}


	public override void _ExitTree()
	{
		BodyEntered -= OnBodyEntered;

		foreach (Area2D item in _damagerAreas)
		{
			item.BodyEntered -= OnDamagerBodyEntered;
		}
	}

	public override void _Ready()
	{
		Health = GetNode<Health>("Health");
		if (Health == null)
		{
			GD.PushError("EnemyCharacter has no Health Node!");
		}

		_animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (_animatedSprite == null)
		{
			GD.PushError("EnemyCharacter has no AnimatedSprite2D Node!");
		}

		_pathFollower = GetParent<PathFollower>();
		if (_pathFollower == null)
		{
			GD.PushError("EnemyCharacter has PathFollower parent!");
		}
	}

	public override void _Process(double delta)
	{
		_animatedSprite.FlipH = _pathFollower.Direction < 0;
	}

	private void OnBodyEntered(Node2D body)
	{
		// Vihollinen ottaa vahinkoa pelaajasta
		if (body is PlayerCharacter playerCharacter && !playerCharacter.Health.IsImmortal)
		{
			Vector2 playerVelocity = playerCharacter.Velocity;
			playerVelocity.Y = _bounceForce;
			playerCharacter.Velocity = playerVelocity;

			Health.TakeDamage(1);
			if (!Health.IsAlive)
			{
				Die();
			}
			else
			{
				// Toista vahinkoanimaatio
				_animatedSprite.Play("damage");
				_animatedSprite.AnimationFinished += OnAnimationFinished;
			}
		}
	}

	private void OnAnimationFinished()
	{
		_animatedSprite.AnimationFinished -= OnAnimationFinished;
		_animatedSprite.Play("default");
	}

	private void OnDamagerBodyEntered(Node2D body)
	{
		// Vihollinen tuottaa pelaajaan vahinkoa
		if (body is PlayerCharacter playerCharacter && !playerCharacter.Health.IsImmortal)
		{
			playerCharacter.Health.TakeDamage(1);
		}
	}

	private void Die()
	{
		// TODO: Play effects (audio, patricles)
		QueueFree();
	}
}
