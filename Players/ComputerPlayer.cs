/// <summary>
/// A computer-controlled player.
/// This player has basic AI logic that will allow it to take pre-determined
/// actions for the different choices that a human player would otherwise
/// need to make.
/// </summary>
public class ComputerPlayer : Player
{
    public ComputerPlayer(Party party) : base(party) { }

    public override void TakeTurn(Character currentCharacter, Player enemyPlayer, int currentRound)
    {
        ActionType action = SelectAction(currentCharacter);
        TryPerformAction(action, currentCharacter, enemyPlayer);
    }

    protected override ActionType SelectAction(Character currentCharacter)
    {
        bool hasHealthPotions = Party.ItemInventory.Any((item) => item is HealthPotion);

        // Determine whether current character should heal
        if (hasHealthPotions)
        {
            bool characterHealthIsUnderHalf = currentCharacter.CurrentHP < Math.Ceiling((float)currentCharacter.MaxHP / 2);

            if (characterHealthIsUnderHalf)
            {
                // There's a 25% chance that this character will use a potion
                if (Random.Shared.Next(4) == 0) return ActionType.UseItem;
            }
        }

        // Determine whether current character should equip gear
        if (Party.GearInventory.Count > 0 && !currentCharacter.HasGearEquipped)
        {
            // There's a 50% chance that this character will equip gear
            if (Random.Shared.Next(2) == 0) return ActionType.Equip;
        }

        // Player will default to attacking, if no other action was executed
        return ActionType.Attack;
    }

    protected override bool TrySelectAttack(Character currentCharacter, out Attack attack)
    {
        attack = currentCharacter.Attacks.FirstOrDefault
        (
            attack => attack.AttackType == AttackType.Special, // Return special attack, if character has one
            defaultValue: currentCharacter.Attacks.First() // Otherwise, return the character's standard attack
        );

        return true;
    }

    protected override bool TrySelectAttackTarget(Party enemyParty, out Character attackTarget)
    {
        // Randomly select a character from the enemy player's party
        int enemyPartySize = enemyParty.Characters.Count;
        int randomIndex = Random.Shared.Next(enemyPartySize);
        attackTarget = enemyParty.Characters[randomIndex];
        return true;
    }

    public override bool TrySelectItem(out Item item)
    {
        item = Party.ItemInventory.First();
        return true;
    }

    public override bool TrySelectGear(out Gear gear)
    {
        gear = Party.GearInventory.First();
        return true;
    }
}
