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
    private Party HeroParty { get; }
    private Party EnemyParty { get; }
    private Battle Battle { get; }

    public Game(string name)
    {
        HeroParty = new Party(new TrueProgrammer(name));
        EnemyParty = new Party(new Skeleton());
        Battle = new Battle(heroParty: HeroParty, enemyParty: EnemyParty);
    }

    public void PlayGame()
    {
        Battle.Play();
    }
}


/// <summary>
/// A battle consists of several turns where each character in each party takes an action.
/// The battle is over once one of the two parties has been defeated.
/// </summary>
public class Battle
{
    private Party HeroParty { get; }
    private Party EnemyParty { get; }

    public Battle(Party heroParty, Party enemyParty)
    {
        HeroParty = heroParty;
        EnemyParty = enemyParty;
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
        Console.WriteLine("Hero Party");
        foreach (Character heroCharacter in HeroParty.Characters)
        {
            Console.WriteLine($"It is {heroCharacter.Name}'s turn...");
            heroCharacter.TakeTurn();
            Thread.Sleep(500);
            Console.WriteLine();
        }

        Console.WriteLine("Enemy Party");
        foreach (Character enemyCharacter in EnemyParty.Characters)
        {
            Console.WriteLine($"It is {enemyCharacter.Name}'s turn...");
            enemyCharacter.TakeTurn();
            Thread.Sleep(500);
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

    public void TakeTurn()
    {
        PerformAction(Actions.Nothing);
    }

    private void PerformAction(Actions action)
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
