public static class PartyGenerator
{
    private static Dictionary<int, List<Difficulty>> difficultyTiers = new ()
    {
        [1] = [Difficulty.Easy, Difficulty.Easy, Difficulty.Easy],
        [2] = [Difficulty.Easy, Difficulty.Easy, Difficulty.Medium],
        [3] = [Difficulty.Medium, Difficulty.Easy, Difficulty.Medium],
        [4] = [Difficulty.Medium, Difficulty.Medium, Difficulty.Medium],
        [5] = [Difficulty.Medium, Difficulty.Hard, Difficulty.Medium],
        [6] = [Difficulty.Hard, Difficulty.Medium, Difficulty.Hard],
        [7] = [Difficulty.Hard, Difficulty.Hard, Difficulty.Hard]
    };

    public static List<Party> GenerateParties(Difficulty difficulty)
    {
        List<Party> parties = [];
        List<List<Difficulty>> partyDifficulties = [];

        switch (difficulty)
        {
            case Difficulty.Easy:
                partyDifficulties = [difficultyTiers[1], difficultyTiers[3]];
                break;
            case Difficulty.Medium:
                partyDifficulties = [difficultyTiers[1], difficultyTiers[3], difficultyTiers[4], difficultyTiers[5]];
                break;
            case Difficulty.Hard:
                partyDifficulties = [..difficultyTiers.Values];
                break;
        }

        foreach (List<Difficulty> characterDifficulties in partyDifficulties)
        {
            parties.Add(GenerateParty(characterDifficulties));
        }

        parties.Add(GenerateFinalBattle(difficulty));

        return parties;
    }

    private static Party GenerateParty(List<Difficulty> characterDifficulties)
    {
        List<Character> characters = [];

        foreach (Difficulty characterDifficulty in characterDifficulties)
        {
            characters.Add(RandomGenerator.GetRandomEnemy(characterDifficulty));
        }

        return new Party(characters, GetRandomStartingItems(), GetRandomStartingGear());
    }

    private static Party GenerateFinalBattle(Difficulty difficulty)
    {
        List<Character> characters = [];
        List<Item> startingItems = [];

        switch (difficulty)
        {
            case Difficulty.Easy:
                characters    = [new TheUncodedOne(maxHP: 50)];

                startingItems = [..HealthPotion.CreatePotions(numberOfSmall: 1, numberOfMedium: 2)];
                break;
            case Difficulty.Medium:
                characters    = [new TheUncodedOne(maxHP: 60),
                                 RandomGenerator.GetRandomEnemy(Difficulty.Medium)];

                startingItems = [..HealthPotion.CreatePotions(numberOfSmall: 3, numberOfMedium: 2, numberOfLarge: 1)];
                break;
            case Difficulty.Hard:
                characters    = [new TheUncodedOne(maxHP: 80),
                                 RandomGenerator.GetRandomEnemy(Difficulty.Medium),
                                 RandomGenerator.GetRandomEnemy(Difficulty.Hard)];

                startingItems = [..HealthPotion.CreatePotions(numberOfSmall: 4, numberOfMedium: 2, numberOfLarge: 2)];
                break;
        }

        return new Party
        (
            characters: characters,
            startingItems: startingItems,
            startingGear: []
        );
    }

    private static List<Item> GetRandomStartingItems(int numberOfItems = 5)
    {
        List<Item> startingItems = [];

        for (int item = 0; item < numberOfItems; item++)
        {
            startingItems.Add(RandomGenerator.GetRandomItem());
        }

        return startingItems;
    }

    private static List<Gear> GetRandomStartingGear(int numberOfGear = 2)
    {
        List<Gear> startingGear = [];

        for (int item = 0; item < numberOfGear; item++)
        {
            startingGear.Add(RandomGenerator.GetRandomGear());
        }

        return startingGear;
    }
}
