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
        TakePlayerTurn(_player1);

        Console.WriteLine("Player 2");
        TakePlayerTurn(_player2);
    }

    private void TakePlayerTurn(Player player)
    {
        foreach (Character character in player.Party.Characters)
        {
            Console.WriteLine($"It is {character.Name}'s turn...");
            player.TakeTurn(character);
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

    public Character(string name)
    {
        Name = name.ToUpper();
    }

    public void PerformAction(Actions action)
    {
        switch (action)
        {
            case Actions.Nothing:
                Console.WriteLine($"{Name} did {action.ToString().ToUpper()}");
                break;
        }
    }
}


public class Skeleton : Character
{
    public Skeleton() : base("Skeleton") { }
}


public class TrueProgrammer : Character
{
    public TrueProgrammer(string name) : base(name) { }
}


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

    public abstract void TakeTurn(Character character);
    public abstract Actions SelectAction();
}


public class HumanPlayer : Player
{
    public HumanPlayer(Party party) : base(party) { }

    public override void TakeTurn(Character character)
    {
        throw new NotImplementedException();
    }

    public override Actions SelectAction()
    {
        throw new NotImplementedException();
    }
}


public class ComputerPlayer : Player
{
    public ComputerPlayer(Party party) : base(party) { }

    public override void TakeTurn(Character character)
    {
        Thread.Sleep(500);
        Actions action = SelectAction();
        character.PerformAction(action);
    }

    public override Actions SelectAction()
    {
        return Actions.Nothing;
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
public enum Actions { Nothing }
