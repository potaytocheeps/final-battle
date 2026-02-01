/// <summary>
/// A party of characters that consists of 1-3 characters.
/// There are two parties in the game, the hero party and the enemy party.
/// </summary>
public class Party
{
    private List<Character> _characters;
    public IReadOnlyList<Character> Characters => _characters;

    public Party(params List<Character> characters)
    {
        _characters = characters;
    }

    public void RemoveFromParty(Character character)
    {
        if (_characters.Contains(character)) _characters.Remove(character);
    }
}
