using System;
using Godot;

public partial class MainUi : CanvasLayer
{
	[Export] private Label _scoreLabel = null;
	[Export] private BaseButton _fiButton = null;
	[Export] private BaseButton _enButton = null;
	[Export] private BaseButton _closeButton = null;
	[Export] private Control _pauseMenu = null;
	[Export] private Slider _masterVolume = null;
	[Export] private Slider _musicVolume = null;
	[Export] private Slider _effectsVolume = null;

	private int _masterBusIndex = -1;
	private int _musicBusIndex = -1;
	private int _effectsBusIndex = -1;

	public override void _EnterTree()
	{
		GameManager.Instance.ScoreChanged += OnScoreChanged;
		_fiButton.Pressed += OnFiPressed;
		_enButton.Pressed += OnEnPressed;
		_closeButton.Pressed += ClosePause;

		_masterVolume.ValueChanged += OnMasterVolumeChanged;
		_musicVolume.ValueChanged += OnMusicVolumeChanged;
		_effectsVolume.ValueChanged += OnEffectsVolumeChanged;
	}

	public override void _ExitTree()
	{
		GameManager.Instance.ScoreChanged -= OnScoreChanged;
		_fiButton.Pressed -= OnFiPressed;
		_enButton.Pressed -= OnEnPressed;
		_closeButton.Pressed -= ClosePause;

		_masterVolume.ValueChanged -= OnMasterVolumeChanged;
		_musicVolume.ValueChanged -= OnMusicVolumeChanged;
		_effectsVolume.ValueChanged -= OnEffectsVolumeChanged;
	}

	public override void _Ready()
	{
		InitializeAudio();

		OnScoreChanged(GameManager.Instance.Score);
		ClosePause();
	}

	public override void _Notification(int what)
	{
		// Notifies about the language change
		if (what == NotificationTranslationChanged)
		{
			OnScoreChanged(GameManager.Instance.Score);
		}
	}

	public void OpenPause()
	{
		_pauseMenu.Show();
		GameManager.Instance.SceneTree.Paused = true;
	}

	public void ClosePause()
	{
		SaveSettings();
		GameManager.Instance.SceneTree.Paused = false;
		_pauseMenu.Hide();
	}

	private void SaveSettings()
	{
		ConfigFile settingsFile = new ConfigFile();

		string section = SettingsConfig.AUDIO_SECTION;

		settingsFile.SetValue(section, SettingsConfig.MASTER_BUS, _masterVolume.Value);
		settingsFile.SetValue(section, SettingsConfig.MUSIC_BUS, _musicVolume.Value);
		settingsFile.SetValue(section, SettingsConfig.EFFECTS_BUS, _effectsVolume.Value);

		section = SettingsConfig.LANGUAGE_SECTION;
		string currentLocale = TranslationServer.GetLocale();
		settingsFile.SetValue(section, SettingsConfig.LANGUAGE, currentLocale);

		settingsFile.Save(SettingsConfig.SETTINGS_PATH);
	}

	private void OnScoreChanged(int currentScore)
	{
		if (_scoreLabel != null)
		{
			string localizedScore = Tr("SCORE");
			_scoreLabel.Text = string.Format(localizedScore, currentScore);
		}
	}

	private void OnEnPressed()
	{
		GameManager.Instance.SetLocale("en");
	}

	private void OnFiPressed()
	{
		GameManager.Instance.SetLocale("fi");
	}

	private void OnEffectsVolumeChanged(double value)
	{
		SetVolumeToBus(_effectsBusIndex, (float)value);
	}

	private void OnMusicVolumeChanged(double value)
	{
		SetVolumeToBus(_musicBusIndex, (float)value);
	}

	private void OnMasterVolumeChanged(double value)
	{
		SetVolumeToBus(_masterBusIndex, (float)value);
	}

	private void SetVolumeToBus(int busIndex, float linearVolume)
	{
		// Linear volume is the value of volume in the range of [0,1].
		// Convert the volume to the decibel scale
		float dbVolume = Mathf.LinearToDb(linearVolume);

		AudioServer.SetBusVolumeDb(busIndex, dbVolume);
	}

	private void InitializeAudio()
	{
		_masterBusIndex = AudioServer.GetBusIndex(SettingsConfig.MASTER_BUS);
		_musicBusIndex = AudioServer.GetBusIndex(SettingsConfig.MUSIC_BUS);
		_effectsBusIndex = AudioServer.GetBusIndex(SettingsConfig.EFFECTS_BUS);

		SetVolume(_masterBusIndex, _masterVolume);
		SetVolume(_musicBusIndex, _musicVolume);
		SetVolume(_effectsBusIndex, _effectsVolume);
	}

	private void SetVolume(int busIndex, Slider volumeSlider)
	{
		float dbVolume = AudioServer.GetBusVolumeDb(busIndex);
		float linearVolume = Mathf.DbToLinear(dbVolume);
		volumeSlider.Value = linearVolume;
	}
}
