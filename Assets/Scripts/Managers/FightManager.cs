using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor;

public class FightManager : MonoBehaviour
{
    public static GameObject[] team;
    public static GameObject[] enemyTeam;
    public List<GameObject> allEntities = new List<GameObject>();
    public Transform[] unitSpawn;
    public Transform[] enemySpawn;

    public float avrSpeed;

    public List<UnitStats> turnOrder = new List<UnitStats>();
    public List<float> turnOrderCounter = new List<float>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        for (int i = 0; i < team.Length; i++)
        {
            Instantiate(team[i], unitSpawn[i]);
            allEntities.Add(team[i]);
        }
        for (int i = 0; i < enemyTeam.Length; i++)
        {
            Instantiate(enemyTeam[i], enemySpawn[i]);
            allEntities.Add(enemyTeam[i]);
        }
        
    }
    void Start()
    {
       avrSpeed = averageSpeed();
       for (int i = 0; i< allEntities.Count; i++)
       {
            allEntities[i].GetComponent<UnitStats>().currentAvrSpeed = allEntities[i].GetComponent<UnitStats>().currentSpeed / avrSpeed;
       }

        TurnOrderCalculation();
    }
    void Update()
    {

    }
    public float averageSpeed()
    {
        float enemyCounter = 0;
        float teamCounter = 0;
        float speedSum = 0;
        for(int i = 0; i<team.Length; i++)
        {
            teamCounter++;
            speedSum += team[i].GetComponent<UnitStats>().baseSpeed;
        }
        for(int i = 0; i < enemyTeam.Length; i++)
        {
            enemyCounter++;
            speedSum += enemyTeam[i].GetComponent<UnitStats>().baseSpeed;
        }
        return speedSum / enemyCounter;
    }

    public void TurnOrderCalculation()
    {
        
        
        allEntities.Sort((a, b) => b.GetComponent<UnitStats>().currentAvrSpeed.CompareTo(a.GetComponent<UnitStats>().currentAvrSpeed));
        for(int i=0; i<allEntities.Count; i++)
        {
            turnOrder.Clear();
            turnOrder.Add(allEntities[i].GetComponent<UnitStats>());
            if (turnOrderCounter[i] >= 1)
            {
                turnOrder.Add(allEntities[i].GetComponent<UnitStats>());
                turnOrder[i + 1].currentAvrSpeed = 1;
                turnOrderCounter[i] -= 1;
                turnOrder.Sort((a, b) => b.currentAvrSpeed.CompareTo(a.currentAvrSpeed));
            }
            if (turnOrderCounter[i] <= -1)
            {
                turnOrder.Add(allEntities[i].GetComponent<UnitStats>());
                turnOrder.RemoveAt(i);
                turnOrderCounter[i] += 1;
                turnOrder.Sort((a, b) => b.currentAvrSpeed.CompareTo(a.currentAvrSpeed));
            }
            if(turnOrderCounter.Count < allEntities.Count)
            {
                turnOrderCounter.Add(allEntities[i].GetComponent<UnitStats>().currentAvrSpeed - 1);
            }
            else
            {
                turnOrderCounter[i] += allEntities[i].GetComponent<UnitStats>().currentAvrSpeed - 1;
            }
            
        }
    }

    // Update is called once per frame
    
}
