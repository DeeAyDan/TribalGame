using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public VisualTreeAsset cardTemplate;
    private VisualElement root;

    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

    }

    public void AddCard(Unit unit, string slotName)
    {
        VisualElement card = cardTemplate.CloneTree();

        card.Q<Label>("NameLabel").text = unit.Name;
        card.Q<Label>("AgeLabel").text = unit.Age.ToString();

        // Image here when available

        card.Q<Label>("ClassLabel").text = unit.Class.ToString();
        card.Q<Label>("SubClassLabel").text = unit.SubClass.ToString();

        card.Q<Label>("AttackLabel").text = unit.Attack.ToString();
        card.Q<Label>("DefenseLabel").text = unit.Defense.ToString();
        card.Q<Label>("HealthLabel").text = unit.Health.ToString();
        card.Q<Label>("SpeedLabel").text = unit.Speed.ToString();

        // Equipments TODO later

        VisualElement slot = root.Q<VisualElement>(slotName);
        slot.Clear();
        slot.Add(card);
    }
}

