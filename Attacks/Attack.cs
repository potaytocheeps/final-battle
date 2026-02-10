/// <summary>
/// Defines characteristics of the different attacks that a character can
/// perform against enemy characters.
/// </summary>
public abstract class Attack
{
    public string Name { get; }
    public int Damage { get; protected set; }
    public AttackType AttackType { get; }
    public float HitChance { get; protected set; }

    public Attack(string name, AttackType attackType)
    {
        Name = name;
        AttackType = attackType;
        HitChance = 1; // By default, an attack has a 100% chance of hitting its target
    }

    public abstract int CalculateDamage();
    public override string ToString() => Name.ToUpper();
}
