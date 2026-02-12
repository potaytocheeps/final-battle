/// <summary>
/// The main character in the game.
/// </summary>
public class TrueProgrammer : Character
{
    public TrueProgrammer(string name) : base(name, maxHP: 25, new Sword())
    {
        _attacks.Insert(0, new Punch());
        _modifiers.Add(ModifierType.Defensive, [new ObjectSight()]);
        _modifiers.Add(ModifierType.Offensive, [new DamageBuff()]);
    }
}
