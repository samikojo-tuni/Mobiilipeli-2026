using System;
using Godot;

/// <summary>
/// GameManager huolehtii pelisessioon liittyvästä datasta.
/// </summary>
public partial class GameManager : Node
{
	#region Singleton
	// Staattinen autoproperty.
	// Get on public, jotta GameManageriin päästään käsiksi mistä vain.
	// Set private, jotta sitä ei voisi helposti ylikirjoittaa.
	public static GameManager Instance
	{
		get;
		private set;
	}

	public GameManager()
	{
		// Singleton takaa, että luokasta voidaan tehdä vain yksi olio kerrallaan.
		if (Instance == null)
		{
			// Ainoata oliota ei ole vielä määritetty. Olkoon tämä olio se.
			Instance = this;
		}
		else if (Instance != this)
		{
			// Singleton-olio on jo olemassa! Tuhotaan juuri luotu olio.
			QueueFree();
			return;
		}
	}
	#endregion

	[Signal] public delegate void ScoreChangedEventHandler(int currentScore);

	#region Game Data
	private int _score = 0;
	private SceneTree _sceneTree = null;

	// Automatically initializing property. Loads the reference to the
	// scene tree when it is needed for the first time.
	public SceneTree SceneTree
	{
		get
		{
			if (_sceneTree == null)
			{
				_sceneTree = GetTree();
			}
			return _sceneTree;
		}
	}

	public int Score
	{
		get { return _score; }
		set
		{
			// TODO: Mieti parempi maksimiarvo.
			_score = Mathf.Clamp(value, 0, Int32.MaxValue);
			GD.Print($"Pisteet nyt: {Score}");
			EmitSignal(SignalName.ScoreChanged, _score);
		}
	}

	public LevelController CurrentLevel
	{
		get;
		private set;
	}

	#endregion

	public override void _Ready()
	{
		LoadSettings();
	}

	/// <summary>
	/// Adds the <paramref name="amount"/> to the score.
	/// </summary>
	/// <param name="amount">The amount to add.</param>
	/// <returns><c>True</c>, if adding the amount was successful. <c>False</c> otherwise.</returns>
	public bool AddScore(int amount)
	{
		if (amount < 0)
		{
			// Ei tukea negatiivisille pisteille. Käytä vähennykseen SubtractScore() metodia.
			return false;
		}

		Score += amount;
		return true;
	}

	public bool SubtractScore(int amount)
	{
		if (amount < 0)
		{
			// Ei tukea negatiivisille pisteille. Käytä lisäämiseen AddScore() metodia.
			return false;
		}

		Score -= amount;
		return true;
	}

	public void GoToScene(string path)
	{
		CallDeferred(MethodName.LoadScene, path);
	}

	public void RegisterLevel(LevelController currentLevel)
	{
		if (CurrentLevel == null && currentLevel != null)
		{
			CurrentLevel = currentLevel;
		}
	}

	public void SetLocale(string locale)
	{
		TranslationServer.SetLocale(locale);
	}

	private void LoadScene(string path)
	{
		if (CurrentLevel != null)
		{
			// Level is already loaded, unload it.
			CurrentLevel.Free();
			CurrentLevel = null;
		}

		// Fetch the scene to be loaded.
		PackedScene nextScene = GD.Load<PackedScene>(path);
		if (nextScene != null)
		{
			// Scene was loaded successfully.
			CurrentLevel = nextScene.Instantiate<LevelController>();
			SceneTree.Root.AddChild(CurrentLevel);
			SceneTree.CurrentScene = CurrentLevel;
		}
		else
		{
			GD.PushError($"Can't load a scene at the path {path}");
		}
	}

	private void LoadSettings()
	{
		ConfigFile settingsFile = new ConfigFile();

		float masterVolume = SettingsConfig.DEFAULT_VOLUME_LINEAR;
		float musicVolume = SettingsConfig.DEFAULT_VOLUME_LINEAR;
		float effectsVolume = SettingsConfig.DEFAULT_VOLUME_LINEAR;

		string locale = SettingsConfig.DEFAULT_LOCALE;

		Error loadError = settingsFile.Load(SettingsConfig.SETTINGS_PATH);
		if (loadError == Error.Ok)
		{
			// Loading the settings file succeeded.
			string section = SettingsConfig.AUDIO_SECTION;
			masterVolume = (float)settingsFile.GetValue(section, SettingsConfig.MASTER_BUS, SettingsConfig.DEFAULT_VOLUME_LINEAR);
			musicVolume = (float)settingsFile.GetValue(section, SettingsConfig.MUSIC_BUS, SettingsConfig.DEFAULT_VOLUME_LINEAR);
			effectsVolume = (float)settingsFile.GetValue(section, SettingsConfig.EFFECTS_BUS, SettingsConfig.DEFAULT_VOLUME_LINEAR);

			section = SettingsConfig.LANGUAGE_SECTION;
			locale = (string)settingsFile.GetValue(section, SettingsConfig.LANGUAGE, SettingsConfig.DEFAULT_LOCALE);
		}

		SetVolume(SettingsConfig.MASTER_BUS, masterVolume);
		SetVolume(SettingsConfig.MUSIC_BUS, musicVolume);
		SetVolume(SettingsConfig.EFFECTS_BUS, effectsVolume);

		SetLocale(locale);
	}

	private bool SetVolume(string audioBus, float linearVolume)
	{
		int busIndex = AudioServer.GetBusIndex(audioBus);
		if (busIndex >= 0)
		{
			float dbVolume = Mathf.LinearToDb(linearVolume);
			AudioServer.SetBusVolumeDb(busIndex, dbVolume);
		}

		return busIndex >= 0;
	}
}
