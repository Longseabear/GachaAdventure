using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSlot : MonoBehaviour {
    public Card card;
    public Transform item;
    public Transform selectedMask;
    public bool slotSelected = false;
    public void InitializeDeckSlot(Card c) {
        card = c;

        Transform[] childs = Util.getFirstChildren(gameObject.transform);
        item = childs[0];
        selectedMask = childs[1];

        DeckData deckData = DeckData.GetInstance();

        slotSelected = deckData.CheckAlreadySelectedCard(card);
        selectedMask.gameObject.SetActive(slotSelected);
    }
    public void SlotSelect() {
        slotSelected = true;
        selectedMask.gameObject.SetActive(slotSelected);
    }
    public void SlotFree() {
        slotSelected = false;
        selectedMask.gameObject.SetActive(slotSelected);
    }
    public GameObject GetItemGameObject() {
        return item.gameObject;
    }
    public static DeckSlot GetSlotByCard(Card card) {
        DeckSlot[] deckslots = GameObject.FindObjectsOfType<DeckSlot>();
        foreach (DeckSlot d in deckslots) {
            if (d.card == card && d.name.StartsWith("slot")) {
                return d;
            }
        }
        return null;
    }
}
