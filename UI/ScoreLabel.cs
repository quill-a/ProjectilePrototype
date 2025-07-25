using Godot;
using ProjectilePrototypeCS.Data;

namespace ProjectilePrototypeCS.UI;

public partial class ScoreLabel : Label
{
	// *** Properties ***
	[Export] public Score Score { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Score.ScoreChanged += OnScoreChanged;
		Text = "SCORE: " + Score.Value;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnScoreChanged(int newScore)
	{
		Text = "SCORE: " + newScore;
	}
}
