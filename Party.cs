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
    public IReadOnlyList<Item> ItemInventory => _items;
    public IReadOnlyList<Gear> GearInventory => _gear;

    public Party(List<Character> characters, List<Item> startingItems, List<Gear> startingGear)
    {
        // By creating a copy of each list parameter, there is no chance of accidentally
        // sharing a reference to the same list with any other objects
        _characters = characters.ToList();
        _items = startingItems.ToList();
        _gear = startingGear.ToList();
    }

    public void RemoveFromParty(Character character)
    {
        if (_characters.Contains(character)) _characters.Remove(character);
    }

    public void AddItemToInventory(Item item) => _items.Add(item);
    public void AddGearToInventory(Gear gear) => _gear.Add(gear);

    public void RemoveItemFromInventory(Item item)
    {
        if (_items.Contains(item)) _items.Remove(item);
    }

    public void RemoveGearFromInventory(Gear gear)
    {
        if (_gear.Contains(gear)) _gear.Remove(gear);
    }

    public int GetItemTypeCount(Item itemToCheck) => _items.FindAll(item => item.GetType() == itemToCheck.GetType()).Count;
    public int GetGearTypeCount(Gear gear) => _gear.FindAll(item => item.GetType() == gear.GetType()).Count;
    public List<Item> GetListOfUniqueItemsInInventory() => _items.DistinctBy((item) => item.GetType()).ToList();
    public List<Gear> GetListOfUniqueGearInInventory() => _gear.DistinctBy((gear) => gear.GetType()).ToList();
}
