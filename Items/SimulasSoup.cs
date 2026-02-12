/// <summary>
/// Fully restores a character's HP.
/// </summary>
public class SimulasSoup : HealthPotion
{
    public SimulasSoup() : base("Simula's Soup") { }

    public override void Heal(Character user, Character healTarget)
    {
        healTarget.Heal(healTarget.MaxHP);
        ColoredConsole.WriteLine($"{user} used {this}.");
        ColoredConsole.WriteLine($"{this} heals {healTarget} back to full health!");
        ColoredConsole.WriteLine($"{healTarget} is now at {healTarget.CurrentHP}/{healTarget.MaxHP} HP.");
    }
}
