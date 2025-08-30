using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SlotDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedCard = eventData.pointerDrag;
        if (droppedCard == null) return;

        UnitCardUI droppedCardUI = droppedCard.GetComponent<UnitCardUI>();
        CardDragHandler dragHandler = droppedCard.GetComponent<CardDragHandler>();
        if (droppedCardUI == null || dragHandler == null) return;

        // Check if this slot already has a card (ignoring background images)
        UnitCardUI existingCard = GetComponentInChildren<UnitCardUI>();

        if (existingCard != null && existingCard.gameObject != droppedCard)
        {
            // --- Swap ---
            Transform existingSlot = existingCard.transform.parent;
            Transform droppedSlot = dragHandler.originalParent; // where the dragged card came from

            existingCard.transform.SetParent(droppedSlot, false);
            droppedCard.transform.SetParent(existingSlot, false);

            existingCard.transform.localPosition = Vector3.zero;
            droppedCard.transform.localPosition = Vector3.zero;

            // Update + save statuses
            UpdateUnitStatusFromParent(existingCard.gameObject, existingSlot);
            UpdateUnitStatusFromParent(droppedCard, existingSlot);
            PersistStatusOf(existingCard.gameObject);
            PersistStatusOf(droppedCard);
        }
        else
        {
            // --- Place in empty slot ---
            droppedCard.transform.SetParent(transform, false);
            droppedCard.transform.localPosition = Vector3.zero;

            // Update + save status
            UpdateUnitStatusFromParent(droppedCard, transform);
            PersistStatusOf(droppedCard);
        }
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

        List<Unit> all = SaveSystemUnits.LoadAllUnits();
        Unit found = all.Find(x => x.ID == ui.boundUnit.ID);

        if (found != null)
        {
            found.UnitStatus = ui.boundUnit.UnitStatus;
        }
        else
        {
            all.Add(ui.boundUnit);
        }
        SaveSystemUnits.SaveAllUnits(all);
    }
}
