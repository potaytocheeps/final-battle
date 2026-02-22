/// <summary>
/// A consumable item that cures a character from a status effect.
/// </summary>
public abstract class CurePotion : Item
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
}
