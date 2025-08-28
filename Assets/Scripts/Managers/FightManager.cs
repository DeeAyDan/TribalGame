using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor;

public class FightManager : MonoBehaviour
{
    public List<GameObject> team = new List<GameObject>();
    public List<GameObject> enemyTeam = new List<GameObject>();
    public List<GameObject> allEntities = new List<GameObject>();
    public Transform[] unitSpawn;
    public Transform[] enemySpawn;
    public GameObject[] enemySelectButtons;

    public GameObject currentAttacker;
    public GameObject currentTarget;

    public GameObject CurrentTurn;
    public bool endTurn;

    public Camera mainCam;

    public Transform instantiationParent;

    public Transform[] cameraPointsArena;

    public float avrSpeed;

    public int turnCounter;

    public List<GameObject> turnOrder = new List<GameObject>();
    public List<GameObject> turnOrderReal = new List<GameObject>();
    public List<float> turnOrderCounter = new List<float>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        for (int i = 0; i < team.Count; i++)
        {
            GameObject InstantiatedUnit = Instantiate(team[i], instantiationParent);
            InstantiatedUnit.transform.position = unitSpawn[i].position;
            allEntities.Add(InstantiatedUnit);
        }
        for (int i = 0; i < enemyTeam.Count; i++)
        {
            GameObject InstantiatedEnemy = Instantiate(enemyTeam[i], instantiationParent);
            InstantiatedEnemy.transform.position = enemySpawn[i].position;
            allEntities.Add(InstantiatedEnemy);
            enemySelectButtons[i] = InstantiatedEnemy.transform.GetChild(2).GetChild(0).gameObject;
        }
        for (int i = 0; i < enemySelectButtons.Length; i++)
        {
            enemySelectButtons[i].SetActive(false);
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
        if(turnCounter + 1 == turnOrder.Count)
        {
            TurnOrderCalculation();
        }
        CamMovement();
    }
    public float averageSpeed()
    {
        float enemyCounter = 0;
        float teamCounter = 0;
        float speedSum = 0;
        for(int i = 0; i<team.Count; i++)
        {
            teamCounter++;
            speedSum += team[i].GetComponent<UnitStats>().baseSpeed;
        }
        for(int i = 0; i < enemyTeam.Count; i++)
        {
            enemyCounter++;
            speedSum += enemyTeam[i].GetComponent<UnitStats>().baseSpeed;
        }
        return speedSum / (enemyCounter+teamCounter);
    }

    public void TurnOrderCalculation()
    {
        turnCounter = 0;
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

    public void Turns()
    {
        for (int i = 0; i < turnOrder.Count;) 
        {
            for (int y = 0; y < team.Count; y++) 
            {
                if (turnOrder[turnCounter] == team[i])
                {
                    OpenTurnMenu();

                }
            }
            mainCam.transform.position = turnOrder[i].transform.GetChild(1).position;
            mainCam.transform.rotation = turnOrder[i].transform.GetChild(1).rotation;

            
            if (endTurn)
            {
                turnCounter++;
                i++;
            }
        }
    }
    public void OpenTurnMenu()
    {
        Debug.Log("Opened Menu");
    }
    public void OpenAttackSelect()
    {
        for(int i=0; i<enemySelectButtons.Length; i++)
        {
            enemySelectButtons[i].SetActive(true);
        }
    }
    public void Attack(Transform enemy)
    {
        for (int i = 0; i < enemySelectButtons.Length; i++)
        {
            enemySelectButtons[i].SetActive(false);
        }
        mainCam.transform.position = cameraPointsArena[1].position;
        currentAttacker = turnOrder[turnCounter];
        currentTarget = enemy.gameObject;
        currentAttacker.transform.position = enemy.GetChild(4).position;
        Damage();
        
    }
    void Damage()
    {
        currentTarget.GetComponent<UnitStats>().currentHealth -= currentAttacker.GetComponent<UnitStats>().currentDamage * (currentTarget.GetComponent<UnitStats>().currentDefense * 0.1f);
        if (currentTarget.GetComponent<UnitStats>().currentHealth <= 0) 
        {
            Death(currentTarget);
        }
        currentAttacker.transform.position = currentAttacker.GetComponent<UnitStats>().spawnPoint.position;
    }
    public void Ability()
    {

    }
    public void Item()
    {

    }
    public void OpenItemMenu()
    {

    }
    
    public void Death(GameObject target)
    {
        for(int i = 0; i<enemyTeam.Count; i++)
        {
            if (enemyTeam[i] == target)
            {
                enemyTeam.RemoveAt(i);
            }
        }
        for (int i = 0; i < team.Count; i++)
        {
            if (team[i] == target)
            {
                team.RemoveAt(i);
            }
        }
        for (int i = 0; i<allEntities.Count; i++)
        {
            if (allEntities[i] == target)
            {
                allEntities.RemoveAt(i);
            }
        }
        for(int i = 0; i<turnOrder.Count; i++)
        {
            if (turnOrder[i] == target && i>turnCounter)
            {
                turnOrder.RemoveAt(i);
            }
        }
        Destroy(target);
        if(enemyTeam.Count == 0)
        {
            Win();
        }
        if(team.Count == 0)
        {
            Loose();
        }
    }

    public void Loose()
    {

    }
    public void Win()
    {

    }
    
    public void EndTurn()
    {
        endTurn = true;
    }
    public void CamMovement()
    {
        
    }
    public void NextRound() 
    {
        TurnOrderCalculation();
    }

    // Update is called once per frame
    
}
