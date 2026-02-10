/// <summary>
/// A character companion that is part of the hero's party.
/// </summary>
public class VinFletcher : Character
{
    public VinFletcher() : base("Vin Fletcher", maxHP: 15, new VinsBow())
    {
        _attacks.Insert(0, new Punch());
    }
}
