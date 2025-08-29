using UnityEngine;
using UnityEngine.UI;

public class SelectAttack : MonoBehaviour
{
    FightManager fightManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fightManager = GameObject.Find("Fight Manager").GetComponent<FightManager>();
        this.gameObject.GetComponent<Button>().onClick.AddListener(SelectClick);
    }

    // Update is called once per frame
    void Update()
    {


    }
    void SelectClick()
    {
        print("clicked hehee");
        fightManager.Attack(this.transform.parent.parent);
    }
}
