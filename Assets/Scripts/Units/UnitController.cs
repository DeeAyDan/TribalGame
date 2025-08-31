using UnityEngine;

public class UnitController : MonoBehaviour
{
    public FightManager fightManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Damage()
    {
        fightManager.Damage();
        print("Damage");
    }
    public void GoBack() 
    {
        fightManager.GoBack();
    }
}
