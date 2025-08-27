using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor;

public class FightManager : MonoBehaviour
{
    public GameObject[] team;
    public GameObject[] enemyTeam;
    public List<GameObject> allEntities = new List<GameObject>();
    public Transform[] unitSpawn;
    public Transform[] enemySpawn;

    public Transform instantiationParent;

    public float avrSpeed;

    public List<GameObject> turnOrder = new List<GameObject>();
    public List<GameObject> turnOrderReal = new List<GameObject>();
    public List<float> turnOrderCounter = new List<float>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        for (int i = 0; i < team.Length; i++)
        {
            GameObject InstantiatedUnit = Instantiate(team[i], instantiationParent);
            InstantiatedUnit.transform.position = unitSpawn[i].position;
            allEntities.Add(InstantiatedUnit);
        }
        for (int i = 0; i < enemyTeam.Length; i++)
        {
            GameObject InstantiatedEnemy = Instantiate(enemyTeam[i], instantiationParent);
            InstantiatedEnemy.transform.position = enemySpawn[i].position;
            allEntities.Add(InstantiatedEnemy);
        }
        
    }
    void Start()
    {
       avrSpeed = averageSpeed();
       for (int i = 0; i< allEntities.Count; i++)
       {
            allEntities[i].GetComponent<UnitStats>().currentAvrSpeed = allEntities[i].GetComponent<UnitStats>().currentSpeed/avrSpeed;
            
            print(allEntities[i].GetComponent<UnitStats>().currentSpeed/avrSpeed);
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
        return speedSum / (enemyCounter+teamCounter);
    }

    public void TurnOrderCalculation()
    {
        int removeCounter = 0;
        
        allEntities.Sort((a, b) => b.GetComponent<UnitStats>().currentAvrSpeed.CompareTo(a.GetComponent<UnitStats>().currentAvrSpeed));
        turnOrder.Clear();
        for(int i=0; i<allEntities.Count; i++)
        {
            
            turnOrder.Add(allEntities[i]);
            //print("added " + allEntities[i]);
            if (turnOrderCounter.Count == allEntities.Count && turnOrderCounter[i] >= 0 )
            {
                turnOrderCounter[i] += allEntities[i].GetComponent<UnitStats>().currentAvrSpeed - 1;
               // print("substracted number " + turnOrder[i]);
                

            }
            else if(turnOrderCounter.Count == allEntities.Count && turnOrderCounter[i] < 0)
            {
                //print("removed " + turnOrder[i]);
                turnOrder.RemoveAt(i-removeCounter);
                removeCounter += 1;
                turnOrderCounter[i] += 1;
            }
            else if (turnOrderCounter.Count < allEntities.Count)
            {
                turnOrderCounter.Add(allEntities[i].GetComponent<UnitStats>().currentAvrSpeed - 1);
            }
            print(Mathf.RoundToInt(Mathf.Abs(turnOrderCounter[i])));
            
            if (turnOrderCounter[i] >= 1)
            {
                for (int y = 0; y < Mathf.RoundToInt(Mathf.Abs(turnOrderCounter[i]));)
                {
                    turnOrder.Add(allEntities[i]);
                   
                    turnOrderCounter[i] -= 1;
                    turnOrder.Sort((a, b) => b.GetComponent<UnitStats>().currentAvrSpeed.CompareTo(a.GetComponent<UnitStats>().currentAvrSpeed));
                }
            }
            
            
            
            
        }
    }
    public void NextRound() 
    {
        TurnOrderCalculation();
    }

    // Update is called once per frame
    
}
