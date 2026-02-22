/// <summary>
/// Status effects give characters negative effects during their turn, such as
/// taking damage each turn.
/// </summary>
public abstract class StatusEffect
{
    private string _name;
    public virtual DamageType DamageType { get; }
    protected virtual int Damage { get; }
    public abstract StatusEffectType StatusEffectType { get; }
    public int NumberOfTurns { get; private set; }
    public string StatusEffectName => StatusEffectType.ToString().ToUpper();

    public StatusEffect(int numberOfTurns, string name = "")
    {
        _name = name;
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

    public override string ToString() => _name.ToUpper();
}


// Defines all of the different types of status effects
public enum StatusEffectType { Poisoned, Electrified, Burned }
