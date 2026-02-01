/// <summary>
/// A character in the game that is controlled by a player or the computer.
/// </summary>
public abstract class Character
{
    public string Name { get; }
    public int MaxHP { get; }
    public int CurrentHP { get; private set; }
    protected Dictionary<AttackType, Attack> _attacks;
    public IReadOnlyDictionary<AttackType, Attack> Attacks => _attacks;

    public Character(string name, int maxHP)
    {
        Name = name.ToUpper();
        MaxHP = maxHP;
        CurrentHP = maxHP;
        _attacks = new ();
    }

    public void PerformAction(Player currentPlayer, ActionType action, Player enemyPlayer)
    {
        switch (action)
        {
            case ActionType.Nothing:
                Console.WriteLine($"{Name} did {action.ToString().ToUpper()}");
                break;
            case ActionType.Attack:
                (AttackType attackType, Character attackTarget) = currentPlayer.PerformAttack(currentCharacter: this, enemyPlayer.Party);

                // Get reference to attack object of the chosen attack type.
                // This reference will be used to get the attack data for the attack
                Attack attack = _attacks[attackType];

                Console.WriteLine($"{Name} used {attack} on {attackTarget}.");

                // The damage that an attack deals can vary per turn. It should be calculated
                // each time the attack is used during a turn
                attack.CalculateDamage(character: this);

                DealDamage(attack.Damage, attackTarget);

                // Display the results of having performed the attack
                Console.WriteLine($"{attack} dealt {attack.Damage} damage to {attackTarget}.");
                Console.WriteLine($"{attackTarget} is now at {attackTarget.CurrentHP}/{attackTarget.MaxHP} HP.");

                if (attackTarget.CurrentHP == 0)
                {
                    ColoredConsole.WriteLine($"{attackTarget} has been defeated!", ConsoleColor.Red);
                    enemyPlayer.Party.RemoveFromParty(attackTarget);
                }

                break;
        }
    }

    protected void DealDamage(int damage, Character attackTarget)
    {
        if (attackTarget.CurrentHP - damage <= 0) attackTarget.CurrentHP = 0;
        else attackTarget.CurrentHP -= damage;
    }

    public override string ToString() => Name;
}
