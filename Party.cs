/// <summary>
/// A party of characters that consists of 1-3 characters.
/// There are two parties in the game, the hero party and the enemy party.
/// </summary>
public class Party
{
    private List<Character> _characters;
    private List<Item> _items;
    public IReadOnlyList<Character> Characters => _characters;
    public IReadOnlyList<Item> Items => _items;

    public Party(List<Character> characters, List<Item> items)
    {
        _characters = characters;
        _items = items;
    }

    public void RemoveFromParty(Character character)
    {
        if (_characters.Contains(character)) _characters.Remove(character);
    }

    public void DisplayPartyInfo(Character currentCharacter, bool addPadding = false)
    {
        foreach (Character character in _characters)
        {
            // Padding is added only to player 2's party, to keep it aligned to the right
            // side of the 'vs.' line in the battle status information
            if (addPadding) Console.Write("".PadRight(30));

            // The character whose turn it is will be colored differently to distinguish it from the others.
            // This creates a visual difference to show that this is the currently selected character
            if (character == currentCharacter) ColoredConsole.Write($"{character}", ConsoleColor.Yellow);
            else ColoredConsole.Write($"{character}");
            ColoredConsole.WriteLine($" (HP: {character.CurrentHP}/{character.MaxHP})");
        }
    }

    public void AddItemToInventory(Item item)
    {
        _items.Add(item);
    }

    public void RemoveItemFromInventory(Item item)
    {
        if (_items.Contains(item)) _items.Remove(item);
    }

    public int GetItemTypeCount<T>() => _items.OfType<T>().Count();
    public List<Item> GetListOfUniqueItemsInInventory() => _items.DistinctBy((item) => item.GetType()).ToList();
}
