/// <summary>
/// Cures all status effects from target.
/// </summary>
public class CureAll : CurePotion
{
    public CureAll() : base("Cure All") { }

    public override void Cure(Character target)
    {
        if (target.StatusEffects.Count > 0)
        {
            target.RemoveAllStatusEffects();

            ColoredConsole.WriteLine($"{this} cures {target} from all status effects!");
        }
        else
        {
            ColoredConsole.WriteLine($"{this} did nothing.");
        }
    }
}
