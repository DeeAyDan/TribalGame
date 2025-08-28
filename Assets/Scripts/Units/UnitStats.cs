using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public string charClass;
    public string charSubClass;
    public float baseHealth;
    public float baseDamage;
    public float baseSpeed;
    public float baseDefense;

    public float currentHealth;
    public float currentDamage;
    public float currentSpeed;
    public float currentDefense;

    public Transform spawnPoint;


    public float currentAvrSpeed;

    public int abilityCooldown = 0;

    void Awake()
    {
        currentHealth = baseHealth;
        currentDamage = baseDamage;
        currentSpeed = baseSpeed;
        currentDefense = baseDefense;
    }
}

