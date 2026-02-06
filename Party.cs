/// <summary>
/// A party of characters that consists of 1-3 characters.
/// There are two parties in the game, the hero party and the enemy party.
/// </summary>
public class Party
{
    private List<Character> _characters;
    private List<Item> _items;
    private List<Gear> _gear;
    public IReadOnlyList<Character> Characters => _characters;
    public IReadOnlyList<Item> Items => _items;
    public IReadOnlyList<Gear> Gear => _gear;

    public Party(List<Character> characters)
    {
        _characters = characters;
        _items = new List<Item>();
        _gear = new List<Gear>();
    }

    public Party(List<Character> characters, List<Item> items) : this(characters)
    {
        _characters = characters;
        _items = items;
    }

    public Party(List<Character> characters, List<Item> items, List<Gear> gear) : this(characters, items)
    {
        _gear = gear;
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

            if (character.HasGearEquipped) ColoredConsole.WriteLine($" (HP: {character.CurrentHP}/{character.MaxHP}) " +
                                                                    $"({character.EquippedGear?.Name})");
            else ColoredConsole.WriteLine($" (HP: {character.CurrentHP}/{character.MaxHP})");
        }
    }

    public void AddItemToInventory(Item item)
    {
        _items.Add(item);
    }

    public void AddGearToInventory(Gear gear)
    {
        _gear.Add(gear);
    }

    public void RemoveItemFromInventory(Item item)
    {
        if (_items.Contains(item)) _items.Remove(item);
    }

    public void RemoveGearFromInventory(Gear gear)
    {
        if (_gear.Contains(gear)) _gear.Remove(gear);
    }

    public int GetItemTypeCount<T>() => _items.OfType<T>().Count();

    public int GetGearTypeCount(Gear gear)
    {
        if (_gear.Count == 0) return 0;

        return _gear.FindAll((item) => item.GetType() == gear.GetType()).Count;
    }

    public List<Item> GetListOfUniqueItemsInInventory() => _items.DistinctBy((item) => item.GetType()).ToList();
    public List<Gear> GetListOfUniqueGearInInventory() => _gear.DistinctBy((gear) => gear.GetType()).ToList();
}
