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

	public override void _Notification(int what)
	{
		// Notifies about the language change
		if (what == NotificationTranslationChanged)
		{
			OnScoreChanged(GameManager.Instance.Score);
		}
	}

	private void OnScoreChanged(int currentScore)
	{
		if (_scoreLabel != null)
		{
			string localizedScore = Tr("SCORE");
			_scoreLabel.Text = string.Format(localizedScore, currentScore);
		}
	}
}
