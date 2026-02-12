/// <summary>
/// A duo character companion that is part of the hero's party.
/// </summary>
public class MylaraAndSkorin : Character
{
    public MylaraAndSkorin() : base("Mylara & Skorin", maxHP: 12, new CannonOfConsolas())
    {
        _attacks.Insert(0, new CannonShot());
    }
}
