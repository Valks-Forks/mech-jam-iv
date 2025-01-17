public partial class Spikes : Area2D
{
    [Export]
    public int Damage { get; set; } = 10;

    private Dictionary<Rid, Player> collidingBodies = new();

    public override void _Ready()
    {
        BodyEntered += (body) =>
        {
            if (body is Player player)
            {
                collidingBodies.Add(player.GetRid(), player);

                player.Hurt(Damage, player.GlobalTransform.Origin, Vector2.Zero);
            }
        };
        BodyExited += (body) =>
        {
            if (body is Player player)
            {
                collidingBodies.Remove(player.GetRid());
            }
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        if (collidingBodies.Any())
        {
            foreach (KeyValuePair<Rid, Player> kvp in collidingBodies)
            {
                kvp.Value.Hurt(Damage, kvp.Value.GlobalTransform.Origin, Vector2.Zero);
            }
        }
    }
}