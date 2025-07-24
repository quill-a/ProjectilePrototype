using Godot;

namespace ProjectilePrototypeCS;

public partial class ProjectileLauncher : Node2D
{
	
	[Export] public Node2D Aiming { get; private set; }

	[Export] public PackedScene ProjectileScene { get; private set; }

	[Export] public StringName ShootAction { get; private set; } = "Shoot";

	[Export] public StringName ProjectilesParentGroup { get; private set; } = "ProjectilesParent";
	
	[Export] public float LaunchPower { get; set; } = 20000f;
	
	// Private Fields
	private Node _projectilesParent;
	private Vector2 _aimDirection;

// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_projectilesParent = GetTree().GetFirstNodeInGroup(ProjectilesParentGroup);
		
		if(_projectilesParent == null) GD.PushWarning("No projectiles parent found in projectiles group " + ProjectilesParentGroup);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 mousePosition = GetGlobalMousePosition();
		_aimDirection = GlobalPosition.DirectionTo(mousePosition);
		if (Aiming != null) Aiming.Rotation = GetAngleTo(mousePosition);
	}

	public override void _Input(InputEvent @event)
	{
		if(@event.IsActionPressed(ShootAction))
		{
			ShootProjectile(ProjectileScene);
		}
	}

	private void ShootProjectile(PackedScene projectileToShoot)
	{
		var projectileInstance = projectileToShoot?.Instantiate<Objects.Projectile>();

		// Only want to add a projectile or set its global position if it's not null.
		if (projectileInstance != null)
		{
			_projectilesParent.AddChild(projectileInstance);
			projectileInstance.GlobalPosition = GlobalPosition;
		}
		
		Vector2 launchVector = _aimDirection * LaunchPower;
		projectileInstance?.ApplyForce(launchVector);
	}
}
