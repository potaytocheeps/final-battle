/// <summary>
/// Restores 20 HP to the character that uses it.
/// </summary>
public class MediumHealthPotion : HealthPotion
{
    public MediumHealthPotion() : base("Medium Health Potion", healAmount: 20) { }
}
