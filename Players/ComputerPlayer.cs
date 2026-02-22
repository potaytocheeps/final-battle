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
        if (currentCharacter.Attacks.Count == 1)
        {
            attack = currentCharacter.Attacks.First();
            return true;
        }

        // Shadow Octopoids and Stone Amaroks may occasionally use their standard attack
        // to take advantage of their standard attacks' side effects
        if (currentCharacter is ShadowOctopoid || currentCharacter is StoneAmarok)
        {
            // Character has a 70% chance to use special attack and 30% chance of using standard attack
            bool willUseSpecialAttack = Random.Shared.NextSingle() < 0.70f;

            if (willUseSpecialAttack) attack = currentCharacter.Attacks.First(a => a.AttackType == AttackType.Special);
            else attack = currentCharacter.Attacks.First(a => a.AttackType == AttackType.Standard);

            return true;
        }

        // Any other character will always use their special attack over their standard attack
        attack = currentCharacter.Attacks.First(a => a.AttackType == AttackType.Special);

        return true;
    }

    protected override bool TrySelectTarget(Party party, out Character target)
    {
        // Randomly select a character from the enemy player's party
        int partySize = party.Characters.Count;
        int randomIndex = Random.Shared.Next(partySize);
        target = party.Characters[randomIndex];
        return true;
    }

    protected override bool TrySelectItemTarget(Character currentCharacter, Party currentParty, Item item, out Character target)
    {
        // Use item on self
        target = currentCharacter;
        return true;
    }

    public override bool TrySelectItem(out Item item)
    {
        item = Party.ItemInventory.FirstOrDefault(i => i is HealthPotion, Party.ItemInventory.First());
        return true;
    }

    public override bool TrySelectGear(IReadOnlyList<Gear> _, out Gear gear, bool __)
    {
        int randomGearIndex = Random.Shared.Next(Party.GearInventory.Count);

        // Equip a random piece of gear from inventory
        gear = Party.GearInventory[randomGearIndex];
        return true;
    }
}
