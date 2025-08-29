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

public enum UnitStatus
{
    Active,
    Reserved,
    NotObtained,
    Dead
}

public enum UnitBodyType
{
    FemaleThin,
    FemaleBulky,
    FemaleFat,
    MaleThin,
    MaleBulky,
    MaleFat
}

[System.Serializable]
public class Unit
{
    public string ID;
    public string Name;
    public int Age;

    public int Attack;
    public int Defense;
    public int Health;
    public int Speed;

    public UnitStatus UnitStatus;
    public UnitBodyType UnitBodyType;

    public int AbilityInt;

    public UnitClass Class;
    public UnitSubClass SubClass;

    public Rarity Rarity;

    public Unit(string name, int age, int attack, int defense, int health, int speed, UnitBodyType bodyType, UnitClass unitClass, UnitSubClass unitSubClass, int abilityInt, Rarity rarity)
    {
        ID = System.Guid.NewGuid().ToString();

        Name = name;
        Age = age;

        Attack = attack;
        Defense = defense;
        Health = health;
        Speed = speed;

        UnitStatus = UnitStatus.NotObtained;
        UnitBodyType = bodyType;

        AbilityInt = abilityInt;

        Class = unitClass;
        SubClass = unitSubClass;
        Rarity = rarity;
    }

    public override string ToString()
    {
        return $"{Name} (ID: {ID}, Age: {Age}, Class: {Class}, SubClass: {SubClass}, Rarity: {Rarity}, " +
               $"Attack: {Attack}, Defense: {Defense}, Health: {Health}, Speed: {Speed}, Ability: {AbilityInt})";
    }
}
