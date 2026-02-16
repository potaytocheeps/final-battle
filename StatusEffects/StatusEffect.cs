/// <summary>
/// Status effects give characters negative effects during their turn, such as
/// taking damage each turn.
/// </summary>
public abstract class StatusEffect
{
    protected abstract DamageType DamageType { get; }
    protected abstract int Damage { get; }
    protected abstract string Name { get; }
    public abstract StatusEffectType StatusEffectType { get; }
    public int NumberOfTurns { get; private set; }
    public string StatusEffectName => StatusEffectType.ToString().ToUpper();

    public StatusEffect(int numberOfTurns)
    {
        NumberOfTurns = numberOfTurns;
    }

    public virtual void Resolve(Character target)
    {
        if (target is MylaraAndSkorin) ColoredConsole.Write($"{target} are {StatusEffectName}.");
        else ColoredConsole.Write($"{target} is {StatusEffectName}.");

        NumberOfTurns--;
        ColoredConsole.WriteLine($" (TURNS LEFT: {NumberOfTurns})", ConsoleColor.Red);

        if (NumberOfTurns <= 0) target.RemoveStatusEffect(this);
    }
}


// Defines all of the different types of status effects
public enum StatusEffectType { Poisoned }
