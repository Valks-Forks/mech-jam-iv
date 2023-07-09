using Godot;
using System;

namespace MechJamIV {
    public enum CollisionLayer
    {
        World,
        Player,
        Robot,
        Environment,
        Hazard,
        Hitbox,
        Enemy
    }

    [Flags]
    public enum CollisionLayerMask : uint
    {
        World = 1,
        Player = 2,
        Robot = 4,
        Environment = 8,
        Hazard = 16,
        Hitbox = 32,
        Enemy = 64
    }
}