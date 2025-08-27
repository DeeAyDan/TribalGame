using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public float baseHealth;
    public float baseDamage;
    public float baseSpeed;
    public float baseDefense;

    public float currentHealth;
    public float currentDamage;
    public float currentSpeed;
    public float currentDefense;


    public float currentAvrSpeed;

    void Awake()
    {
        currentHealth = baseHealth;
        currentDamage = baseDamage;
        currentSpeed = baseSpeed;
        currentDefense = baseDefense;
    }
}

