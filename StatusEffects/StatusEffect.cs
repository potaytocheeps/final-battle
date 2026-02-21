/// <summary>
/// Status effects give characters negative effects during their turn, such as
/// taking damage each turn.
/// </summary>
public abstract class StatusEffect
{
    public virtual DamageType DamageType { get; }
    protected virtual int Damage { get; }
    protected virtual string Name { get; }
    public abstract StatusEffectType StatusEffectType { get; }
    public int NumberOfTurns { get; private set; }
    public string StatusEffectName => StatusEffectType.ToString().ToUpper();

    public StatusEffect(int numberOfTurns)
    {
        Name = "";
        NumberOfTurns = numberOfTurns;
    }

    public virtual void Resolve(Character target)
    {
        if (target is MylaraAndSkorin) ColoredConsole.Write($"{target} are ");
        else ColoredConsole.Write($"{target} is ");

        ColoredConsole.Write($"{TextColor.ColorText(StatusEffectName, DamageType)}.");

        NumberOfTurns--;
        ColoredConsole.WriteLine($" (TURNS LEFT: {NumberOfTurns})", ConsoleColor.Red);

        if (NumberOfTurns <= 0) target.RemoveStatusEffect(this);
    }
}


// Defines all of the different types of status effects
public enum StatusEffectType { Poisoned, Electrified }
