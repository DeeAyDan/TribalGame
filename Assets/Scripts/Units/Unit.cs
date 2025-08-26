public enum UnitClass
{
    Melee,
    Ranged,
    Support
}

public enum UnitSubClass
{
    // Melee
    Tank,
    Berserker,
    Slammer,

    // Ranged
    Archer,
    SpearThrower,
    DartBlower,

    // Support
    Healer,
    Runner,
    Herbalist,
    Crafter,
    Alchemist
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public class Unit
{
    public string Name;
    public int Age;

    public int Attack;
    public int Defense;
    public int Health;
    public int Speed;

    public int abilityInt;

    public UnitClass Class;
    public UnitSubClass SubClass;

    public Rarity Rarity;

    public Unit(string name, int age, int attack, int defense, int health, int speed, UnitClass unitClass, UnitSubClass unitSubClass, int abilityInt, Rarity rarity)
    {
        Name = name;
        Age = age;

        Attack = attack;
        Defense = defense;
        Health = health;
        Speed = speed;

        abilityInt = abilityInt;

        Class = unitClass;
        SubClass = unitSubClass;
        Rarity = rarity;
    }

    public override string ToString()
    {
        return $"{Name} (Age: {Age}, Class: {Class}, SubClass: {SubClass}, Rarity: {Rarity}, Attack: {Attack}, Defense: {Defense}, Health: {Health}, Speed: {Speed}, Ability: {abilityInt})";
    }
}
