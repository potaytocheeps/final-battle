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
        Name = name.ToUpper();
        AttackType = attackType;
    }

    public override string ToString() => Name;

    public abstract int CalculateDamage(Attack attack);
}
