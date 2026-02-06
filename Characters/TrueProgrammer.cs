/// <summary>
/// The main character in the game.
/// </summary>
public class TrueProgrammer : Character
{
    public TrueProgrammer(string name) : base(name, maxHP: 25)
    {
        _attacks.Add(AttackType.Standard, new Punch());
        EquipGear(new Sword());
    }
}
