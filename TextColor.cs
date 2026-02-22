/// <summary>
/// Contains console commands for different types of colors based on damage
/// type, to display given text in a specific color.
/// </summary>
public static class TextColor
{
    // Define constant colors for the different damage types
    private const string POISON = "\e[38;2;0;200;0m";
    private const string ELECTRIC = "\e[38;2;85;130;255m";
    private const string FIRE = "\e[38;2;255;127;0m";
    private const string PHYSICAL = "\e[38;2;185;200;235m";
    private const string DECODING = "\e[38;2;175;75;225m";
    private const string DEFAULTCOLOR = "\e[39;m";

    public static string ColorText(string text, DamageType damageType)
    {
        string coloredText = damageType switch
        {
            DamageType.Normal   => PHYSICAL,
            DamageType.Poison   => POISON,
            DamageType.Electric => ELECTRIC,
            DamageType.Fire     => FIRE,
            DamageType.Decoding => DECODING,
            _                   => DEFAULTCOLOR
        };

        return coloredText + text + DEFAULTCOLOR;
    }
}
