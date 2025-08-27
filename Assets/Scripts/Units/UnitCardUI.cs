using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitCardUI : MonoBehaviour
{
    [Header("UI Elements")]
    //public Image borderImage;
    //public Image portraitImage;
    public TMP_Text nameText;
    public TMP_Text ageText;
    public TMP_Text attackText;
    public TMP_Text defenseText;
    public TMP_Text healthText;
    public TMP_Text speedText;
    public TMP_Text subClassText;

    public void Setup(Unit data, Sprite portrait = null)
    {
        if (data == null)
        {
            Debug.LogError("Unit is null in UnitCardUI.Setup");
            return;
        }

        nameText.text = data.Name;
        ageText.text = $"{data.Age}";
        attackText.text = $"{data.Attack}";
        defenseText.text = $"{data.Defense}";
        healthText.text = $"{data.Health}";
        speedText.text = $"{data.Speed}";
        subClassText.text = data.SubClass.ToString();

        if (portrait != null)
        {
            //portraitImage.sprite = portrait;
        }
        else
        {
            // Placeholder color
            //portraitImage.color = Color.gray;
        }
    }
}
