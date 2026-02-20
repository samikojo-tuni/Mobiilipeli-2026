using System;
using Godot;

/// <summary>
/// Kantaluokka kaikelle kerättävälle. Toteutta perustoiminnan, joka on yhteinen kaikille
/// kerättäville esineille.
/// </summary>
public partial class Collectable : Area2D
{
	private bool _isCollected = false;

	// Tämä property on tällä hetkellä read-only, koska sillä ei ole set-määrettä.
	public bool IsCollected
	{
		get { return _isCollected; }
	}

	public override void _EnterTree()
	{
		// Aloita BodyEntered-signaalin kuuntelu.
		BodyEntered += OnBodyEntered;
	}

	public override void _ExitTree()
	{
		// Lopeta signaalin kuuntelu.
		BodyEntered -= OnBodyEntered;
	}

	/// <summary>
	/// Suoritetaan, kun BodyEntered signaali laukeaa. Signaalilla tiedotetaan Collectable-oliota
	/// tlörmäyksestä toisen kappaleen kanssa.
	/// </summary>
	/// <param name="body">Viittaus työmäävään kappaleeseen.</param>
	private void OnBodyEntered(Node2D body)
	{
		if (body is PlayerCharacter playerCharacter)
		{
			// Törmäys tapahtui pelaajan kanssa. Reagoi siihen.
			_isCollected = true;
			Collect(playerCharacter);

			// Poista kerättävä Node kuluvan framen päätteeksi.
			QueueFree(); // Käytä tätä Free:n sijaan, turvallisempi!
		}
	}

	// Virtual-avainsana tarkoittaa sitä, että tämän metodin toiminnallisuus voidaan
	// määrittää uudelleen lapsiluokassa override-avainsanalla.
	// Protected tarkoittaa sitä, että metodi on saatavilla lapsiluokalle (mutta ei julkisesti).
	protected virtual void Collect(PlayerCharacter playerCharacter)
	{
	}
}
