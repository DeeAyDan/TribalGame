using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SlotDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObj = eventData.pointerDrag;
        if (droppedObj == null) return;

        CardDragHandler dragHandler = droppedObj.GetComponent<CardDragHandler>();
        if (dragHandler == null) return;

        Transform oldParent = dragHandler.originalParent;
        Transform newParent = this.transform;

        if (newParent == oldParent)
        {
            droppedObj.transform.SetParent(newParent, false);
            droppedObj.transform.localPosition = Vector3.zero;
            return;
        }

        GameObject existingCard = null;
        if (newParent.childCount > 0)
        {
            existingCard = newParent.GetChild(0).gameObject;
        }

        if (existingCard != null)
        { 
            existingCard.transform.SetParent(oldParent, false);
            existingCard.transform.SetSiblingIndex(dragHandler.originalSiblingIndex);
            UpdateUnitStatusFromParent(existingCard, oldParent);
        }

        droppedObj.transform.SetParent(newParent, false);
        droppedObj.transform.localPosition = Vector3.zero;

        UpdateUnitStatusFromParent(droppedObj, newParent);

        PersistStatusOf(droppedObj);
        if (existingCard != null) PersistStatusOf(existingCard);
    }

    private void UpdateUnitStatusFromParent(GameObject cardObj, Transform parentSlot)
    {
        UnitCardUI ui = cardObj.GetComponent<UnitCardUI>();
        if (ui == null || ui.boundUnit == null) return;

        Slot slotComp = parentSlot.GetComponent<Slot>();
        if (slotComp == null) return;

        if (slotComp.slotType == SlotType.Active)
            ui.boundUnit.UnitStatus = UnitStatus.Active;
        else if (slotComp.slotType == SlotType.Reserved)
            ui.boundUnit.UnitStatus = UnitStatus.Reserved;
    }

    private void PersistStatusOf(GameObject cardObj)
    {
        UnitCardUI ui = cardObj.GetComponent<UnitCardUI>();
        if (ui == null || ui.boundUnit == null) return;

        List<Unit> all = SaveSystem.LoadAllUnits();
        Unit found = all.Find(x => x.ID == ui.boundUnit.ID);

        if (found != null)
        {
            found.UnitStatus = ui.boundUnit.UnitStatus;
        }
        else
        {
            all.Add(ui.boundUnit);
        }
        SaveSystem.SaveAllUnits(all);
    }
}
