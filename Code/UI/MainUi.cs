using System;
using Godot;

public partial class MainUi : CanvasLayer
{
	[Export] private Label _scoreLabel = null;

	public override void _EnterTree()
	{
		GameManager.Instance.ScoreChanged += OnScoreChanged;
	}

	public override void _ExitTree()
	{
		GameManager.Instance.ScoreChanged -= OnScoreChanged;
	}

	public override void _Ready()
	{
		OnScoreChanged(GameManager.Instance.Score);
	}

	private void OnScoreChanged(int currentScore)
	{
		if (_scoreLabel != null)
		{
			_scoreLabel.Text = $"Score: {currentScore}";
		}
	}
}
