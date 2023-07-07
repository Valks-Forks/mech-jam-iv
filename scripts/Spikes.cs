using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Spikes : Area2D
{

	[Export]
	public int Damage { get; set; } = 10;

	private Dictionary<Rid, Player> collidingBodies = new ();

	public override void _Ready()
	{
		BodyEntered += (body) =>
		{
			if (body is Player player)
			{
				collidingBodies.Add(player.GetRid(), player);

				player.HurtAsync(Damage);
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

	public override void _Process(double delta)
	{
		if (collidingBodies.Any())
		{
			foreach (KeyValuePair<Rid, Player> kvp in collidingBodies)
			{
				kvp.Value.HurtAsync(Damage);
			}
		}
	}

}
