using Godot;
using ProjectilePrototypeCS.Data;
using ProjectilePrototypeCS.Objects;

namespace ProjectilePrototypeCS.Gameplay;

public partial class ScoreBox : Node2D
{
	// *** Properties ***
	[Export] public Area2D ScoreArea { get; private set; }
	
	[Export] public Score Score { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScoreArea.BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is not Projectile) return;
		Score.Value += 1;
		body.QueueFree();
	}
}
