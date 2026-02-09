/// <summary>
/// Defines the characteristics for a consumable item that can be used by a
/// character during battle.
/// </summary>
public abstract class Item
{
    public string Name { get; }

    public Item(string name)
    {
        Name = name;
    }

    public override string ToString() => Name.ToUpper();
}
