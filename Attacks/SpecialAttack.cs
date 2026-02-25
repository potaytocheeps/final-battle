/// <summary>
/// A special attack that can be gained based on equipped gear.
/// </summary>
public class SpecialAttack : Attack
{
    public SpecialAttack(string name, DamageType damageType = DamageType.Physical)
                  : base(name, AttackType.Special, damageType)
    {
        NumberOfUsesLeft = this switch
        {
            Slash => 5,
            Stab  => 8,
            _     => 20
        };
    }

    public override int CalculateDamage()
    {
        Damage = this switch
        {
            Slash     => 3,
            Stab      => 2,
            QuickShot => 4,
            _         => 0
        };

        return Damage;
    }

    public override void DealDamage(Character user, Character attackTarget)
    {
        base.DealDamage(user, attackTarget);

        NumberOfUsesLeft--;

        if (NumberOfUsesLeft <= 0)
        {
            // Get reference to gear object that provided this attack to its user
            Gear? gearToRemove = user.EquippedGear.FirstOrDefault
            (
                gear => gear?.AttackProvided == this,
                null
            );

            // Remove gear from the user, since there are no more uses left
            if (gearToRemove != null)
            {
                ConsoleIOHandler.WaitForPlayerConfirmation();
                ColoredConsole.WriteLine($"{user}'s {gearToRemove} broke!", ConsoleColor.Red);
                user.UnequipGear(gearToRemove, gearWasLost: true);
            }
        }
    }
}
