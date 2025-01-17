namespace MechJamIV;

public abstract partial class EnemyBase : CharacterBase
{
    [Signal]
    public delegate void PickupDroppedEventHandler(PickupBase pickup);

    [Export]
    public float FieldOfView { get; set; } = 45.0f;

    [Export]
    public float CriticalHitRate { get; set; } = 0.3f;

    [Export]
    public float PickupDropRate { get; set; } = 0.5f;

    public EnemyState State { get; protected set; } = EnemyState.Idle;

    #region Node references

    protected Player Player { get; private set; }

    #endregion Node references

    public override void _Ready()
    {
        base._Ready();

        Player = (Player)GetTree().GetFirstNodeInGroup("player");

        foreach (Node2D node in GetNode<Node2D>("Hitboxes").GetChildren())
        {
            if (node is Hitbox hitbox)
            {
                hitbox.Hit += (damage, isWeakSpot, position, normal) =>
                {
                    if (isWeakSpot || RandomHelper.GetSingle() <= CriticalHitRate)
                    {
                        Hurt(damage * 2, position, normal);
                    }
                    else
                    {
                        Hurt(damage, position, normal);
                    }
                };
                hitbox.Colliding += (body) =>
                {
                    if (Health <= 0)
                    {
                        return;
                    }

                    if (body is Player player)
                    {
                        player.Hurt(hitbox.Damage, hitbox.GlobalTransform.Origin, Vector2.Zero);
                    }
                };
            }
        }

#if DEBUG
        AddRayCastToPlayer();
#endif
    }

    protected override sealed Vector2 GetMovementDirection() =>
        State switch
        {
            EnemyState.Idle => GetMovementDirection_Idle(),
            EnemyState.Chase => GetMovementDirection_Chase(),
            EnemyState.Attacking => GetMovementDirection_Attacking(),
            _ => Vector2.Zero,
        };

    protected abstract Vector2 GetMovementDirection_Idle();

    protected abstract Vector2 GetMovementDirection_Chase();

    protected abstract Vector2 GetMovementDirection_Attacking();

    protected override sealed void ProcessAction()
    {
        switch (State)
        {
            case EnemyState.Idle:
                ProcessAction_Idle();

                break;

            case EnemyState.Chase:
                ProcessAction_Chase();

                break;

            case EnemyState.Attacking:
                ProcessAction_Attacking();

                break;
        }

#if DEBUG
        UpdateRayCastToPlayer();
#endif
    }

    protected abstract void ProcessAction_Idle();

    protected abstract void ProcessAction_Chase();

    protected abstract void ProcessAction_Attacking();

    public override void Hurt(int damage, Vector2 position, Vector2 normal)
    {
        if (Health <= 0)
        {
            return;
        }

        base.Hurt(damage, position, normal);

        if (Health <= 0)
        {
            DropPickup();

            this.TimedFree(5.0f, processInPhysics: true);
        }
    }

    private void DropPickup()
    {
        PickupBase pickup = PickupHelper.GenerateRandomPickup(PickupDropRate);

        if (pickup != null)
        {
            pickup.GlobalTransform = GlobalTransform;

            EmitSignal(SignalName.PickupDropped, pickup);
        }
    }

    protected Vector2 GetDirectionToPlayer() =>
        GlobalTransform.Origin.DirectionTo(Player.GlobalTransform.Origin);

    protected bool IsPlayerInFieldOfView() =>
        Mathf.RadToDeg(FaceDirection.AngleTo(GetDirectionToPlayer())) < FieldOfView;

    protected bool IsPlayerInLineOfSight()
    {
        Godot.Collections.Dictionary collision = GetWorld2D().DirectSpaceState.IntersectRay(new PhysicsRayQueryParameters2D()
        {
            From = GlobalTransform.Origin + Vector2.Up, // offset so we don't collide with ground
            To = Player.GlobalTransform.Origin,
            Exclude = null,
            CollideWithBodies = true,
            CollideWithAreas = true,
            CollisionMask = (uint)(CollisionLayerMask.World | CollisionLayerMask.Player)
        });

        return collision.ContainsKey("collider") && collision["collider"].Obj == Player;
    }
}