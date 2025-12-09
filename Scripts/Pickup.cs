using Godot;
using System;

public partial class Pickup : Area3D
{
	[Export]
	GpuParticles3D _explosion;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
	{
		_explosion.Emitting = true;
		this.Visible = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}
} 
