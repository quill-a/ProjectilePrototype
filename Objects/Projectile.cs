using Godot;

namespace ProjectilePrototypeCS.Objects;

public partial class Projectile : RigidBody2D
{
	// *** Properties ***
	[Export] public float TimeToLive { get; private set; } = 3f;
	
	// *** Fields ***
	private Timer _freeTimer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_freeTimer = new Timer();
		AddChild(_freeTimer);
		_freeTimer.WaitTime = TimeToLive;
		_freeTimer.Timeout += OnTimeout;
		_freeTimer.Start();
	}

	public void OnTimeout()
	{
		QueueFree();
	}
}
