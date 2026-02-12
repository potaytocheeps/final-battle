/// <summary>
/// A consumable item that restores HP to a character when used.
/// </summary>
public abstract class HealthPotion : Item
{
    private readonly int _healAmount;

    public HealthPotion(string name, int healAmount = 0) : base(name)
    {
        _healAmount = healAmount;
    }

    public virtual void Heal(Character user, Character healTarget)
    {
        healTarget.Heal(_healAmount);
        ColoredConsole.WriteLine($"{user} used {this}.");
        ColoredConsole.WriteLine($"{this} heals {healTarget} for {_healAmount} HP.");
        ColoredConsole.WriteLine($"{healTarget} is now at {healTarget.CurrentHP}/{healTarget.MaxHP} HP.");
    }
}
