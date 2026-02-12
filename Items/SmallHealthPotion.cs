/// <summary>
/// Restores 10 HP to the character that uses it.
/// </summary>
public class SmallHealthPotion : HealthPotion
{
    public SmallHealthPotion() : base("Small Health Potion", healAmount: 10) { }
}
