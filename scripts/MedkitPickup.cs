using MechJamIV;

public partial class MedkitPickup : PickupBase
{
    public override PickupType PickupType { get; protected set; } = PickupType.Medkit;

    #region Node references

    private AnimatedSprite2D animatedSprite2D;

    #endregion Node references

    public override void _Ready()
    {
        base._Ready();

        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animatedSprite2D.AnimationLooped += () => animatedSprite2D.Rotate(Mathf.DegToRad(RandomHelper.GetInt(360)));
    }
}