/// <summary>
/// Standard attack for the Stone Amarok character.
/// </summary>
public class Bite : StandardAttack
{
    public Bite() : base("Bite") { }

    public override void DealDamage(Character user, Character attackTarget)
    {
        base.DealDamage(user, attackTarget);

        if (user.CurrentHP != user.MaxHP)
        {
            // The attacker will heal the amount of damage they dealt to the target
            user.Heal(Damage);
            ColoredConsole.WriteLine($"{user} healed itself for {Damage} HP!");
            ColoredConsole.WriteLine($"{user} is now at {user.CurrentHP}/{user.MaxHP} HP.");
        }
    }
}
