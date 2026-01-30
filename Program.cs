string playerName = AskUserForInput("Enter the name for the True Programmer: ");

Game game = new Game(playerName);
game.PlayGame();


string AskUserForInput(string question)
{
    while (true)
    {
        Console.Write(question);
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input)) continue;
        else return input;
    }
}


/// <summary>
/// Contains the main logic for the whole game.
/// </summary>
public class Game
{
    private readonly Player _player1;
    private readonly Player _player2;
    private readonly List<Party> _enemyBattleParties;
    private Battle _battle;

    public Game(string name)
    {
        _player1 = new ComputerPlayer(new Party(new TrueProgrammer(name)));
        _enemyBattleParties =
            [
                new Party(new Skeleton()), // Battle 1
                new Party(new Skeleton(1), new Skeleton(2)), // Battle 2
                new Party(new TheUncodedOne()) // Battle 3
            ];
        _player2 = new ComputerPlayer(_enemyBattleParties[0]); // Start with the first enemy party
        _battle = new Battle(player1: _player1, player2: _player2);
    }

    public void PlayGame()
    {
        int currentBattleIndex = 0;

        while (true)
        {
            // Display current battle information
            Console.WriteLine("---------------------------------------------");
            Console.Write($"Battle {currentBattleIndex + 1}: ");
            DisplayPlayerParty(_player1);
            Console.Write(" vs. ");
            DisplayPlayerParty(_player2);
            Console.WriteLine("\n---------------------------------------------\n");

            _battle.Play();

            // End game if hero party was defeated during this current battle
            if (_player1.Party.Characters.Count == 0)
            {
                Console.WriteLine("The heroes lost! The Uncoded One's forces have prevailed.");
                return;
            }

            currentBattleIndex++;

            if (currentBattleIndex >= _enemyBattleParties.Count) break; // End game if there are no more battles left
            else
            {
                // Continue to next battle
                _player2.SetParty(_enemyBattleParties[currentBattleIndex]);
                _battle = new Battle(player1: _player1, player2: _player2);
            }
        }

        // Hero party emerged victorious from all battles
        Console.WriteLine("The heroes won! The Uncoded One's reign is finally over!");
    }

    private void DisplayPlayerParty(Player player)
    {
        int playerPartySize = player.Party.Characters.Count;

        for (int index = 0; index < playerPartySize; index++)
        {
            string characterName = player.Party.Characters[index].Name;

            if (index >= playerPartySize - 1) Console.Write(characterName); // This is the last character in the party
            else Console.Write(characterName + ", ");
        }
    }
}


/// <summary>
/// A battle consists of several turns where each character in each party takes an action.
/// The battle is over once one of the two parties has been defeated.
/// </summary>
public class Battle
{
    private readonly Player _player1;
    private readonly Player _player2;
    private bool _battleIsOver;

    public Battle(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
        _battleIsOver = false;
    }

    public void Play()
    {
        while (!_battleIsOver)
        {
            PlayRound();
        }
    }

    public void PlayRound()
    {
        Console.WriteLine("Player 1");
        TakePlayerTurn(currentPlayer: _player1, enemyPlayer: _player2);

        if (_battleIsOver) return;

        Console.WriteLine("Player 2");
        TakePlayerTurn(currentPlayer: _player2, enemyPlayer: _player1);
    }

    private void TakePlayerTurn(Player currentPlayer, Player enemyPlayer)
    {
        foreach (Character character in currentPlayer.Party.Characters)
        {
            Console.WriteLine($"It is {character.Name}'s turn...");
            currentPlayer.TakeTurn(character, enemyPlayer);
            Console.WriteLine();

            // Check to see if either player's party has been completely defeated
            if (_player1.Party.Characters.Count == 0 || _player2.Party.Characters.Count == 0)
            {
                _battleIsOver = true;
                return;
            }
        }
    }
}


/// <summary>
/// A character in the game that is controlled by a player or the computer.
/// </summary>
public abstract class Character
{
    public string Name { get; }
    public int MaxHP { get; }
    public int CurrentHP { get; private set; }
    protected Dictionary<AttackType, Attack> _attacks;

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
                (AttackType attackType, Character attackTarget) = currentPlayer.PerformAttack(enemyPlayer.Party);

                // Get reference to attack object of the chosen attack type.
                // This reference will be used to get the attack data for the attack
                Attack attack = _attacks[attackType];

                Console.WriteLine($"{Name} used {attack.Name} on {attackTarget.Name}.");

                // The damage that an attack deals can vary per turn. It should be calculated
                // each time the attack is used during a turn
                attack.CalculateDamage(character: this);

                DealDamage(attack.Damage, attackTarget);

                // Display the results of having performed the attack
                Console.WriteLine($"{attack.Name} dealt {attack.Damage} damage to {attackTarget.Name}.");
                Console.WriteLine($"{attackTarget.Name} is now at {attackTarget.CurrentHP}/{attackTarget.MaxHP} HP.");

                if (attackTarget.CurrentHP == 0)
                {
                    Console.WriteLine($"{attackTarget.Name} has been defeated!");
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
}


public class Skeleton : Character
{
    public Skeleton() : base("Skeleton", maxHP: 5)
    {
        _attacks.Add(AttackType.Standard, new StandardAttack(name: "Bone Crunch"));
    }

    public Skeleton(int number) : base($"Skeleton {number}", maxHP: 5)
    {
        _attacks.Add(AttackType.Standard, new StandardAttack(name: "Bone Crunch"));
    }
}


public class TrueProgrammer : Character
{
    public TrueProgrammer(string name) : base(name, maxHP: 25)
    {
        _attacks.Add(AttackType.Standard, new StandardAttack(name: "Punch"));
    }
}


public class TheUncodedOne : Character
{
    public TheUncodedOne() : base("The Uncoded One", maxHP: 15)
    {
        _attacks.Add(AttackType.Standard, new StandardAttack(name: "Unraveling"));
    }
}


/// <summary>
/// Defines characteristics of the different attacks that a character can
/// perform against enemy characters.
/// </summary>
public abstract class Attack
{
    public string Name { get; }
    public int Damage { get; protected set; }

    public Attack(string name)
    {
        Name = name.ToUpper();
    }

    public abstract int CalculateDamage(Character character);
}


public class StandardAttack : Attack
{
    public StandardAttack(string name) : base(name) { }

    public override int CalculateDamage(Character character)
    {
        Damage = character switch
        {
            Skeleton       => new Random().Next(2),
            TrueProgrammer => 1,
            TheUncodedOne  => new Random().Next(3),
            _              => 0
        };

        return Damage;
    }
}


// Defines all of the different types of attacks that characters can perform
public enum AttackType { Standard }


/// <summary>
/// A Player in the game that controls a Party of Characters.
/// </summary>
public abstract class Player
{
    public Party Party { get; private set; }

    public Player(Party party)
    {
        Party = party;
    }

    public abstract void TakeTurn(Character currentCharacter, Player enemyPlayer);
    protected abstract ActionType SelectAction();
    public abstract (AttackType, Character) PerformAttack(Party enemyParty);

    public void SetParty(Party party)
    {
        Party = party;
    }
}


public class HumanPlayer : Player
{
    public HumanPlayer(Party party) : base(party) { }

    public override void TakeTurn(Character currentCharacter, Player enemyPlayer)
    {
        throw new NotImplementedException();
    }

    protected override ActionType SelectAction()
    {
        throw new NotImplementedException();
    }

    public override (AttackType, Character) PerformAttack(Party enemyParty)
    {
        throw new NotImplementedException();
    }
}


public class ComputerPlayer : Player
{
    public ComputerPlayer(Party party) : base(party) { }

    public override void TakeTurn(Character currentCharacter, Player enemyPlayer)
    {
        Thread.Sleep(500);
        ActionType action = SelectAction();
        currentCharacter.PerformAction(currentPlayer: this, action, enemyPlayer);
    }

    protected override ActionType SelectAction()
    {
        return ActionType.Attack;
    }

    public override (AttackType, Character) PerformAttack(Party enemyParty)
    {
        int enemyPartySize = enemyParty.Characters.Count;
        int randomIndex = new Random().Next(enemyPartySize);
        Character attackTarget = enemyParty.Characters[randomIndex];

        AttackType attackType = SelectAttack();

        return (attackType, attackTarget);
    }

    public AttackType SelectAttack()
    {
        return AttackType.Standard;
    }
}


/// <summary>
/// A party of characters that consists of 1-3 characters.
/// There are two parties in the game, the hero party and the enemy party.
/// </summary>
public class Party
{
    private List<Character> _characters;
    public IReadOnlyList<Character> Characters => _characters;

    public Party(params List<Character> characters)
    {
        _characters = characters;
    }

    public void RemoveFromParty(Character character)
    {
        if (_characters.Contains(character)) _characters.Remove(character);
    }
}


// Enumeration with all of the possible actions that a character can take
public enum ActionType { Nothing, Attack }
