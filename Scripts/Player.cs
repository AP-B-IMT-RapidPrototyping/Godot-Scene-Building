using Godot;
using System;

// https://www.youtube.com/watch?v=EP5AYllgHy8
public partial class Player : CharacterBody3D
{
	[Export]
	float _speed = 5.0f;
	[Export]
	float _jumpVelocity = 4.5f;
	[Export]
	Node3D _cameraMount = null;
	[Export] 
	AnimationPlayer _animationPlayer = null;
	[Export]
	Node3D _visuals = null;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion)
		{
			RotateY(-Mathf.DegToRad((@event as InputEventMouseMotion).Relative.X));
			_visuals.RotateY(Mathf.DegToRad((@event as InputEventMouseMotion).Relative.X));
			_cameraMount.RotateX(-Mathf.DegToRad((@event as InputEventMouseMotion).Relative.Y));
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = _jumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backwards");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			var speedMult = Input.IsActionPressed("run") ? 2 : 1;

			if (IsOnFloor() == false)
				_animationPlayer.Stop();
			else if (_animationPlayer.CurrentAnimation != "walking" && Input.IsActionPressed("run") == false)
				_animationPlayer.Play("walking");
			else if (_animationPlayer.CurrentAnimation != "running" && Input.IsActionPressed("run") == true)
				_animationPlayer.Play("running");

			velocity.X = direction.X * _speed * speedMult;
			velocity.Z = direction.Z * _speed * speedMult;

			_visuals.LookAt(Position + direction);
		}
		else
		{
			if (_animationPlayer.CurrentAnimation != "idle")
				_animationPlayer.Play("idle");
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
