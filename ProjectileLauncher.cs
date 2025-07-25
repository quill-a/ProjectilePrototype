using Godot;

namespace ProjectilePrototypeCS;

public partial class ProjectileLauncher : Node2D
{
	// *** Properties ***
	[Export] public Node2D Aiming { get; private set; }

	[Export] public PackedScene ProjectileScene { get; private set; }

	[Export] public StringName ShootAction { get; private set; } = "Shoot";

	[Export] public StringName ProjectilesParentGroup { get; private set; } = "ProjectilesParent";
	
	[Export] public float LaunchPower { get; set; } = 30000f;
	
	[Export] public Sprite2D Sprite {get; private set;}

	/// <summary>
	/// Multiply this by the Launch Power to get the bonus power
	/// that is added to the projectile at launch-time.
	/// </summary>
	[Export] public float TimePowerMultiplier { get; set; } = 0.5f;

	private bool IsCharging
	{
		get => _isCharging;
		set
		{
			_isCharging = value;

			if (!_isCharging) _chargeTime = 0.0f;
		}
	}
	
	
	// *** Fields ***
	private Node _projectilesParent;
	private Vector2 _aimDirection;
	private bool _isCharging;
	private float _chargeTime;
	private Color _defaultSpriteColor;
	
	private const float MaxChargeTime = 3.0f; // Limit charge time to 3 seconds.
	
	
	// *** Methods ***
	//
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_projectilesParent = GetTree().GetFirstNodeInGroup(ProjectilesParentGroup);
		
		if (_projectilesParent == null) GD.PushWarning("No projectiles parent found in projectiles group " + ProjectilesParentGroup);

		if (Sprite != null) _defaultSpriteColor = Sprite.Modulate;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Determines what direction to aim.
		Vector2 mousePosition = GetGlobalMousePosition();
		_aimDirection = GlobalPosition.DirectionTo(mousePosition);
		if (Aiming != null) Aiming.Rotation = GetAngleTo(mousePosition);

		if (_isCharging)
		{
			_chargeTime += (float) delta;
			
			// Progressively change color of the launcher as the action button stays pressed.
			if (Sprite != null) Sprite.SelfModulate = new Color(
				1f,
				1f - (_chargeTime / MaxChargeTime),
				1f - (_chargeTime / MaxChargeTime)
				);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed(ShootAction))
		{
			ChargeProjectile();
		}
		else if (@event.IsActionReleased(ShootAction))
		{
			ShootProjectile(ProjectileScene);
		}
	}

	private void ChargeProjectile()
	{
		IsCharging = true;
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
		
		// Limit charge time.
		if (_chargeTime > MaxChargeTime) _chargeTime = MaxChargeTime;
		
		// Calculate power and launch projectile.
		float finalPower = LaunchPower + (LaunchPower * _chargeTime * TimePowerMultiplier);
		Vector2 launchVector = _aimDirection * finalPower;
		projectileInstance?.ApplyForce(launchVector);
		
		// Don't want the launcher to keep charging after the action button has been released.
		// Also, if projectile is launched, then time charged should implicitly reset to 0 seconds for the next event.
		IsCharging = false;
		
		if (Sprite != null) Sprite.SelfModulate = _defaultSpriteColor;
	}
}
