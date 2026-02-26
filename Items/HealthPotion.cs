/// <summary>
/// A consumable item that restores HP to a character when used.
/// </summary>
public class HealthPotion : Item
{
    private readonly int _healAmount;

    public HealthPotion(string name, int healAmount = 0) : base(name)
    {
        _healAmount = healAmount;
    }

    public virtual void Heal(Character healTarget)
    {
        healTarget.Heal(_healAmount);
        ColoredConsole.WriteLine($"{this} heals {healTarget} for {_healAmount} HP.");
        ColoredConsole.WriteLine($"{healTarget} is now at {healTarget.CurrentHP}/{healTarget.MaxHP} HP.");
    }

    public static List<HealthPotion> CreatePotions(int numberOfSmall = 0, int numberOfMedium = 0, int numberOfLarge = 0)
    {
        return [..CreatePotions(numberOfSmall, SmallHealthPotion),
                ..CreatePotions(numberOfMedium, MediumHealthPotion),
                ..CreatePotions(numberOfLarge, LargeHealthPotion)];
    }

    private static List<HealthPotion> CreatePotions(int numberOfPotions, Func<HealthPotion> createPotion)
    {
        List<HealthPotion> healthPotions = [];

        for (int currentPotion = 0; currentPotion < numberOfPotions; currentPotion++)
        {
            healthPotions.Add(createPotion());
        }

        return healthPotions;
    }

    public static HealthPotion SmallHealthPotion() => new HealthPotion("Small Health Potion", healAmount: 10);
    public static HealthPotion MediumHealthPotion() => new HealthPotion("Medium Health Potion", healAmount: 20);
    public static HealthPotion LargeHealthPotion() => new HealthPotion("Large Health Potion", healAmount: 30);
}
