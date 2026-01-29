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
    private readonly Battle _battle;

    public Game(string name)
    {
        _player1 = new ComputerPlayer(new Party(new TrueProgrammer(name)));
        _player2 = new ComputerPlayer(new Party(new Skeleton()));
        _battle = new Battle(player1: _player1, player2: _player2);
    }

    public void PlayGame()
    {
        _battle.Play();
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

    public Battle(Player player1, Player player2)
    {
        _player1 = player1;
        _player2 = player2;
    }

    public void Play()
    {
        while (true)
        {
            PlayRound();
        }
    }

    public void PlayRound()
    {
        Console.WriteLine("Player 1");
        TakePlayerTurn(currentPlayer: _player1, enemyPlayer: _player2);

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
        }
    }
}


/// <summary>
/// A character in the game that is controlled by a player or the computer.
/// </summary>
public class Character
{
    public string Name { get; }
    protected Dictionary<AttackType, Attack> _attacks;

    public Character(string name)
    {
        Name = name.ToUpper();
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
                Console.WriteLine($"{Name} used {_attacks[attackType].Name} on {attackTarget.Name}");
                break;
        }
    }
}


public class Skeleton : Character
{
    public Skeleton() : base("Skeleton")
    {
        _attacks.Add(AttackType.Standard, new StandardAttack("Bone Crunch"));
    }
}


public class TrueProgrammer : Character
{
    public TrueProgrammer(string name) : base(name)
    {
        _attacks.Add(AttackType.Standard, new StandardAttack("Punch"));
    }
}


/// <summary>
/// Defines characteristics of the different attacks that a character can
/// perform against enemy characters.
/// </summary>
public abstract class Attack
{
    public string Name { get; }

    public Attack(string name)
    {
        Name = name.ToUpper();
    }
}


public class StandardAttack : Attack
{
    public StandardAttack(string name) : base(name) { }
}


// Defines all of the different types of attacks that characters can perform
public enum AttackType { Standard }


/// <summary>
/// A Player in the game that controls a Party of Characters.
/// </summary>
public abstract class Player
{
    public Party Party { get; }

    public Player(Party party)
    {
        Party = party;
    }

    public abstract void TakeTurn(Character currentCharacter, Player enemyPlayer);
    protected abstract ActionType SelectAction();
    public abstract (AttackType, Character) PerformAttack(Party enemyParty);
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
}


// Enumeration with all of the possible actions that a character can take
public enum ActionType { Nothing, Attack }
