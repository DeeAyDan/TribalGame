using UnityEngine;

public class UnitFactory
{
    public static Unit GenerateRandomUnit()
    {
        string name = GenerateRandomName();
        int age = GenerateRandomAge();

        UnitClass unitClass = GenerateRandomUnitClass();
        UnitSubClass subClass = GenerateRandomUnitSubClass(unitClass);

        Rarity rarity = GenerateRandomRarity();
        int rarityMultiplier = (int)rarity + 1;

        int attack = GenerateRandomStat(subClass, "attack") * rarityMultiplier;
        int defense = GenerateRandomStat(subClass, "defense") * rarityMultiplier;
        int health = GenerateRandomStat(subClass, "health") * rarityMultiplier;
        int speed = GenerateRandomStat(subClass, "speed") * rarityMultiplier;

        int abilityInt = SetAbilityInt(subClass);

        return new Unit(name, age, attack, defense, health, speed, unitClass, subClass, abilityInt, rarity);

    }

    private static string GenerateRandomName()
    {
        string[] names = { "Arin", "Borin", "Cirin", "Dorin", "Erin", "Farin", "Gorin", "Hirin" };
        return names[Random.Range(0, names.Length)];
    }

    private static int GenerateRandomAge()
    {
        return Random.Range(14, 40);
    }

    // TODO Need balancing for the classes
    private static int GenerateRandomStat(UnitSubClass subClass, string stat)
    {
        switch ((subClass, stat)){
            case (UnitSubClass.Tank, "attack"):
                return Random.Range(5, 11);
            case (UnitSubClass.Tank, "defense"):
                return Random.Range(10, 16);
            case (UnitSubClass.Tank, "health"):
                return Random.Range(20, 35);
            case (UnitSubClass.Tank, "speed"):
                return Random.Range(3, 8);

            case (UnitSubClass.Berserker, "attack"):
                return Random.Range(3, 8);
            case (UnitSubClass.Berserker, "defense"):
                return Random.Range(7, 11);
            case (UnitSubClass.Berserker, "health"):
                return Random.Range(12, 18);
            case (UnitSubClass.Berserker, "speed"):
                return Random.Range(15, 20);

            case (UnitSubClass.Slammer, "attack"):
                return Random.Range(20, 30);
            case (UnitSubClass.Slammer, "defense"):
                return Random.Range(9, 14);
            case (UnitSubClass.Slammer, "health"):
                return Random.Range(20, 26);
            case (UnitSubClass.Slammer, "speed"):
                return Random.Range(2, 6);

            case (UnitSubClass.Archer, "attack"):
                return Random.Range(5, 11);
            case (UnitSubClass.Archer, "defense"):
                return Random.Range(5, 11);
            case (UnitSubClass.Archer, "health"):
                return Random.Range(5, 11);
            case (UnitSubClass.Archer, "speed"):
                return Random.Range(5, 11);

            case (UnitSubClass.SpearThrower, "attack"):
                return Random.Range(5, 11);
            case (UnitSubClass.SpearThrower, "defense"):
                return Random.Range(5, 11);
            case (UnitSubClass.SpearThrower, "health"):
                return Random.Range(5, 11);
            case (UnitSubClass.SpearThrower, "speed"):
                return Random.Range(5, 11);

            case (UnitSubClass.DartBlower, "attack"):
                return Random.Range(5, 11);
            case (UnitSubClass.DartBlower, "defense"):
                return Random.Range(5, 11);
            case (UnitSubClass.DartBlower, "health"):
                return Random.Range(5, 11);
            case (UnitSubClass.DartBlower, "speed"):
                return Random.Range(5, 11);

            case (UnitSubClass.Healer, "attack"):
                return Random.Range(5, 11);
            case (UnitSubClass.Healer, "defense"):
                return Random.Range(5, 11);
            case (UnitSubClass.Healer, "health"):
                return Random.Range(5, 11);
            case (UnitSubClass.Healer, "speed"):
                return Random.Range(5, 11);

            case (UnitSubClass.Runner, "attack"):
                return Random.Range(5, 11);
            case (UnitSubClass.Runner, "defense"):
                return Random.Range(5, 11);
            case (UnitSubClass.Runner, "health"):
                return Random.Range(5, 11);
            case (UnitSubClass.Runner, "speed"):
                return Random.Range(5, 11);

            case (UnitSubClass.Herbalist, "attack"):
                return Random.Range(5, 11);
            case (UnitSubClass.Herbalist, "defense"):
                return Random.Range(5, 11);
            case (UnitSubClass.Herbalist, "health"):
                return Random.Range(5, 11);
            case (UnitSubClass.Herbalist, "speed"):
                return Random.Range(5, 11);

            case (UnitSubClass.Crafter, "attack"):
                return Random.Range(5, 11);
            case (UnitSubClass.Crafter, "defense"):
                return Random.Range(5, 11);
            case (UnitSubClass.Crafter, "health"):
                return Random.Range(5, 11);
            case (UnitSubClass.Crafter, "speed"):
                return Random.Range(5, 11);

            case (UnitSubClass.Alchemist, "attack"):
                return Random.Range(5, 11);
            case (UnitSubClass.Alchemist, "defense"):
                return Random.Range(5, 11);
            case (UnitSubClass.Alchemist, "health"):
                return Random.Range(5, 11);
            case (UnitSubClass.Alchemist, "speed"):
                return Random.Range(5, 11);
            default:
                return 0;
        }
    }

    private static UnitClass GenerateRandomUnitClass()
    {
        return (UnitClass)Random.Range(0, System.Enum.GetValues(typeof(UnitClass)).Length);
    }

    private static UnitSubClass GenerateRandomUnitSubClass(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.Melee:
                return (UnitSubClass)Random.Range(0, 3); // Tank, Berserker, Slammer
            case UnitClass.Ranged:
                return (UnitSubClass)Random.Range(3, 6); // Archer, SpearThrower, DartBlower
            case UnitClass.Support:
                return (UnitSubClass)Random.Range(6, 11); // Healer, Runner, Herbalist, Crafter, Alchemist
            default:
                return UnitSubClass.Tank; // Default case
        }
    }

    private static int SetAbilityInt(UnitSubClass subclass)
    {
        switch (subclass)
        {
            case UnitSubClass.Tank:
                return 1;
            case UnitSubClass.Berserker:
                return 2;
            case UnitSubClass.Slammer:
                return 3;
            case UnitSubClass.Archer:
                return 4;
            case UnitSubClass.SpearThrower:
                return 5;
            case UnitSubClass.DartBlower:
                return 6;
            case UnitSubClass.Healer:
                return 7;
            case UnitSubClass.Runner:
                return 8;
            case UnitSubClass.Herbalist:
                return 9;
            case UnitSubClass.Crafter:
                return 10;
            case UnitSubClass.Alchemist:
                return 11;
            default:
                return 0;
        }
    }

    private static Rarity GenerateRandomRarity()
    {
        int roll = Random.Range(0, 385);
        if (roll < 300) return Rarity.Common;
        if (roll < 360) return Rarity.Uncommon;
        if (roll < 370) return Rarity.Rare;
        if (roll < 384) return Rarity.Epic;
        else return Rarity.Legendary;
    }
}
