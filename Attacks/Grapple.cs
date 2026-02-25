/// <summary>
/// Standard attack for the Shadow Octopoid character.
/// Has a chance to steal a piece of equipped gear from its attack target.
/// </summary>
public class Grapple : StandardAttack
{
    public Grapple() : base("Grapple") { }

    public override int CalculateDamage() => 2;

    public override void DealDamage(Character user, Character attackTarget)
    {
        base.DealDamage(user, attackTarget);

        if (attackTarget.HasGearEquipped)
        {
            if (Random.Shared.NextSingle() < 1/3f) // Has one third chance to steal equipped gear
            {
                // Steal a random piece of gear from attack target
                int randomIndex = Random.Shared.Next(attackTarget.EquippedGear.Count);
                Gear gearToSteal = attackTarget.EquippedGear[randomIndex];

                ColoredConsole.WriteLine($"\n{user} stole {gearToSteal} from {attackTarget}!", ConsoleColor.DarkYellow);

                attackTarget.UnequipGear(gearToSteal, gearWasLost: true);
                user.EquipGear(gearToSteal);
            }
        }
    }
}
