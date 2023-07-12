using Godot;
using System;
using MechJamIV;

public partial class EnemyMech : EnemyBase
{

	#region Resources

	private PackedScene shrapnelSplatter = ResourceLoader.Load<PackedScene>("res://scenes/shrapnel_splatter.tscn");

	#endregion

	public override void _Ready()
	{
		base._Ready();
	}

	protected override Vector2 GetMovementDirection() => Vector2.Zero;

    protected override bool IsJumping() => false;

    protected override void ProcessAttack(double delta)
    {
        //TODO
    }

	protected override void AnimateInjury(int damage, Vector2 normal)
    {
        GpuParticles2D splatter = shrapnelSplatter.Instantiate<GpuParticles2D>();

		AddChild(splatter);

		splatter.Emitting = true;

		splatter.TimedFree(splatter.Lifetime + splatter.Lifetime * splatter.Randomness, processInPhysics:true);
    }

}
