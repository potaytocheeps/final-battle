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

    public override string ToString() => Name;

    public abstract int CalculateDamage(Character character);
}
