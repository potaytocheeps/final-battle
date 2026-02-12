/// <summary>
/// Standard attack for the duo character Mylara & Skorin.
/// </summary>
public class CannonShot : StandardAttack
{
    public CannonShot() : base("Cannon Shot") { }

    public override int CalculateDamage() => Random.Shared.Next(1, 3); // Randomly deals 1-2 damage
}
