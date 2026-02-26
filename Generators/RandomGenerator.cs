public static class RandomGenerator
{
    private enum GearType { BinaryHelm, Dagger, Sword }
    private enum EnemyCharacterType { Skeleton, StoneAmarok, ShadowOctopoid }
    private enum ModifierType { DamageBuff, StoneArmor }
    private enum ItemType { SmallHealthPotion, MediumHealthPotion, LargeHealthPotion, SimulasSoup,
                            BurnCure, ElectrifiedCure, PoisonCure, CureAll }

    public static Character GetRandomEnemy(Difficulty characterDifficulty)
    {
        Gear? gear = null;
        Modifier? modifier = null;
        int maxHP = 0;

        switch (characterDifficulty)
        {
            case Difficulty.Easy:
                maxHP = Random.Shared.Next(8, 13);
                break;
            case Difficulty.Medium:
                gear = GetRandomGear();
                maxHP = Random.Shared.Next(12, 21);
                break;
            case Difficulty.Hard:
                gear = GetRandomGear();
                modifier = GetRandomModifier();
                maxHP = Random.Shared.Next(15, 26);
                break;
        }

        return GetRandomEnemyCharacter(gear, modifier, maxHP);
    }

    public static Gear GetRandomGear()
    {
        GearType randomGearType;
        DamageType randomDamageType;

        while (true)
        {
            int random = Random.Shared.Next(Enum.GetNames<GearType>().Count());
            randomGearType = (GearType)random;
            randomDamageType = GetRandomDamageType();

            if (randomGearType != GearType.BinaryHelm) break;
            // There's a final 5% chance that the Binary Helm will be chosen
            else if (Random.Shared.NextSingle() < 0.15f) break;
        }

        return randomGearType switch
        {
            GearType.BinaryHelm => new BinaryHelm(),
            GearType.Dagger     => new Dagger(randomDamageType),
            GearType.Sword      => new Sword(randomDamageType),
            _                   => new Dagger()
        };
    }

    private static Character GetRandomEnemyCharacter(Gear? startingGear, Modifier? startingModifier, int maxHP)
    {
        int random = Random.Shared.Next(Enum.GetNames<EnemyCharacterType>().Count());
        EnemyCharacterType randomEnemyType = (EnemyCharacterType)random;

        return randomEnemyType switch
        {
            EnemyCharacterType.ShadowOctopoid => new ShadowOctopoid(startingGear, startingModifier, maxHP),
            EnemyCharacterType.Skeleton       => new Skeleton(startingGear, startingModifier, maxHP),
            EnemyCharacterType.StoneAmarok    => new StoneAmarok(startingGear, startingModifier, maxHP),
            _                                 => new Skeleton(null, null, 1)
        };
    }

    private static Modifier GetRandomModifier()
    {
        int random = Random.Shared.Next(Enum.GetNames<ModifierType>().Count());
        ModifierType randomModifierType = (ModifierType)random;

        return randomModifierType switch
        {
            ModifierType.DamageBuff => new DamageBuff(),
            ModifierType.StoneArmor => new StoneArmor(),
            _                       => new DamageBuff()
        };
    }

    private static DamageType GetRandomDamageType()
    {
        while (true)
        {
            int random = Random.Shared.Next(Enum.GetNames<DamageType>().Count());
            DamageType randomDamageType = (DamageType)random;

            if (randomDamageType != DamageType.Decoding) return randomDamageType;
        }
    }

    public static Item GetRandomItem()
    {
        int random = Random.Shared.Next(Enum.GetNames<ItemType>().Count());
        ItemType randomItemType = (ItemType)random;

        return randomItemType switch
        {
            ItemType.SmallHealthPotion  => HealthPotion.SmallHealthPotion(),
            ItemType.MediumHealthPotion => HealthPotion.MediumHealthPotion(),
            ItemType.LargeHealthPotion  => HealthPotion.LargeHealthPotion(),
            ItemType.SimulasSoup        => new SimulasSoup(),
            ItemType.BurnCure           => CurePotion.BurnCure(),
            ItemType.ElectrifiedCure    => CurePotion.ElectrifiedCure(),
            ItemType.PoisonCure         => CurePotion.PoisonCure(),
            ItemType.CureAll            => new CureAll(),
            _                           => HealthPotion.SmallHealthPotion()
        };
    }
}
