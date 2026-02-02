/// <summary>
/// Restores HP to the character that uses it.
/// </summary>
public class HealthPotion : Item
{
    public int HealAmount { get; }

    public HealthPotion() : base("Health Potion")
    {
        HealAmount = 10;
    }
}
