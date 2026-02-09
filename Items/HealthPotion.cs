/// <summary>
/// Restores HP to the character that uses it.
/// </summary>
public class HealthPotion : Item
{
    private readonly int _healAmount;

    public HealthPotion() : base("Health Potion")
    {
        _healAmount = 10;
    }

    public void Heal(Character user, Character healTarget)
    {
        healTarget.Heal(_healAmount);
        ColoredConsole.WriteLine($"{user} used {this}.");
        ColoredConsole.WriteLine($"{this} heals {healTarget} for {_healAmount} HP.");
        ColoredConsole.WriteLine($"{healTarget} is now at {healTarget.CurrentHP}/{healTarget.MaxHP} HP.");
    }
}
