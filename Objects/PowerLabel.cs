using Godot;

namespace ProjectilePrototypeCS.Objects;

public partial class PowerLabel : Label
{
	[Export] public ProjectileLauncher Launcher { get; private set; }

// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Launcher.PowerChanged += OnPowerChanged;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void OnPowerChanged(float newPower)
	{
		Text = "POWER: " + newPower;
	}
}
