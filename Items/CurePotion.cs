/// <summary>
/// A consumable item that cures a character from a status effect.
/// </summary>
public class CurePotion : Item
{
    private StatusEffectType _statusEffectType;

    public CurePotion(string name, StatusEffectType statusEffectType = StatusEffectType.Burned) : base(name)
    {
        _statusEffectType = statusEffectType;
    }

    public virtual void Cure(Character target)
    {
        if (target.StatusEffects.ContainsKey(_statusEffectType))
        {
            StatusEffect statusEffect = target.StatusEffects[_statusEffectType];
            target.RemoveStatusEffect(statusEffect);

            ColoredConsole.WriteLine($"{this} cures {target} from {statusEffect}!");
        }
        else
        {
            ColoredConsole.WriteLine($"{this} did nothing.");
        }
    }

    public static List<CurePotion> CreatePotions(int numberOfBurn = 0, int numberOfElectrified = 0, int numberOfPoison = 0)
    {
        return [..CreatePotions(numberOfBurn, BurnCure),
                ..CreatePotions(numberOfElectrified, ElectrifiedCure),
                ..CreatePotions(numberOfPoison, PoisonCure)];
    }

    private static List<CurePotion> CreatePotions(int numberOfPotions, Func<CurePotion> createPotion)
    {
        List<CurePotion> curePotions = [];

        for (int currentPotion = 0; currentPotion < numberOfPotions; currentPotion++)
        {
            curePotions.Add(createPotion());
        }

        return curePotions;
    }

    public static CurePotion BurnCure() => new CurePotion("Burn Cure", StatusEffectType.Burned);
    public static CurePotion ElectrifiedCure() => new CurePotion("Electrified Cure", StatusEffectType.Electrified);
    public static CurePotion PoisonCure() => new CurePotion("Poison Cure", StatusEffectType.Poisoned);
}
