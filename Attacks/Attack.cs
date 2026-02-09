/// <summary>
/// Defines characteristics of the different attacks that a character can
/// perform against enemy characters.
/// </summary>
public abstract class Attack
{
    public string Name { get; }
    public int Damage { get; protected set; }
    public AttackType AttackType { get; }

    public Attack(string name, AttackType attackType)
    {
        Name = name;
        AttackType = attackType;
    }

    public abstract int CalculateDamage();
    public override string ToString() => Name.ToUpper();
}
