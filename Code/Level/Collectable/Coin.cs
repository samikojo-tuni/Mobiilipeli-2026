using Godot;

public partial class Coin : Collectable
{
	[Export] private int _score = 10;

	protected override void Collect(PlayerCharacter playerCharacter)
	{
		// TODO: Lisää pisteen sellaiseen järjestelmään, joka pitää niistä kirjaa.
		GD.Print($"Kerättiin kolikko, pisteet: {_score}");
		GameManager.Instance.AddScore(_score);
	}
}