/// <summary>
/// Special attack provided by the Vin's Bow gear item.
/// </summary>
public class QuickShot : SpecialAttack
{
    public QuickShot() : base("Quick Shot")
    {
        HitChance = 0.5f; // Has 50% chance of hitting its target
    }
}
