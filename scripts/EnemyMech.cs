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

	protected override bool IsAttacking() => false;

    protected override void ProcessAttack(double delta)
    {
        throw new NotImplementedException();
    }

	protected async override void AnimateInjuryAsync(int damage, Vector2 normal)
    {
        GpuParticles2D splatter = shrapnelSplatter.Instantiate<GpuParticles2D>();

		AddChild(splatter);

		splatter.Emitting = true;

		await ToSignal(GetTree().CreateTimer(5.0f), SceneTreeTimer.SignalName.Timeout);

		splatter.QueueFree();
    }

}