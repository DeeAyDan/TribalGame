using UnityEngine;

public enum SlotType
{
    Active,
    Reserved
}

public class Slot : MonoBehaviour
{
    public SlotType slotType = SlotType.Reserved;
    public int index = 0;
}
