using System;
using Godot;

public partial class Coin : Collectable
{
	[Export] private int _score = 10;
	[Export] private AudioStreamPlayer2D _audioClip = null;
	[Export] private AnimatedSprite2D _animatedSprite = null;
	[Export] private GpuParticles2D _particleEffect = null;

	private int _effectCounter = 0;

	protected override void Collect(PlayerCharacter playerCharacter)
	{
		GameManager.Instance.AddScore(_score);
	}

	protected override void Clear()
	{
		if (_animatedSprite != null)
		{
			// Hide the coin
			_animatedSprite.Hide();
		}

		if (_audioClip != null)
		{
			_effectCounter++;
			_audioClip.Finished += OnEffectFinished;
			_audioClip.Play();
		}

		if (_particleEffect != null)
		{
			_effectCounter++;
			_particleEffect.Finished += OnEffectFinished;
			_particleEffect.Emitting = true;
		}
	}

	private void OnEffectFinished()
	{
		_effectCounter--;

		if (_effectCounter <= 0)
		{
			if (_audioClip != null)
			{
				_audioClip.Finished -= OnEffectFinished;
			}

			if (_particleEffect != null)
			{
				_particleEffect.Finished -= OnEffectFinished;
			}

			QueueFree();
		}
	}
}