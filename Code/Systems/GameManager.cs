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

	#region Game Data
	private int _score = 0;

	public int Score
	{
		get { return _score; }
		set
		{
			// TODO: Mieti parempi maksimiarvo.
			_score = Mathf.Clamp(value, 0, Int32.MaxValue);
			GD.Print($"Pisteet nyt: {Score}");
			// TODO: Päivitä pisteet käyttöliittymälle.
		}
	}

	#endregion

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
}