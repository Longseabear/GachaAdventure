using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckEvent : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    private RectTransform cardPanel;
    private Vector2 prePos = Vector2.zero;
    private string dragType = "";
    private DeckData deckData;

    private int selectedIndex;

    private GameObject currentSelectedItem = null;

    void Awake() {
        cardPanel = GetComponentsInChildren<RectTransform>()[1];
        deckData = DeckData.GetInstance();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eve) {
        DeckUI deckUI = DeckUI.GetInstance();

        prePos = eve.position;
        selectedIndex = (int)((eve.position.x - cardPanel.position.x) / deckUI.slotSize + 1);
    }

    void IDragHandler.OnDrag(PointerEventData eve) {
        Vector2 moveOffset = eve.position - prePos;

        if (dragType == "horizon") {
            cardPanel.Translate(Vector2.right * moveOffset.x, 0);
            prePos = eve.position;
        }
        else if (dragType == "vertical") {
            DeckUI deckUI = DeckUI.GetInstance();
            currentSelectedItem.GetComponent<RectTransform>().Translate(moveOffset);
            prePos = eve.position;
        }

        if (dragType == "" && Vector2.Distance(eve.position, prePos) > 1.0f) {
            if (Math.Abs(moveOffset.x) > Math.Abs(moveOffset.y))
                dragType = "horizon";
            else {
                // drage vertical
                DeckUI deckUI = DeckUI.GetInstance();
                Camera _camera = Camera.main;

                float screenWidth = _camera.pixelWidth;

                if (eve.position.x < 20 || eve.position.x > screenWidth - 20)
                    return;

                // n <= ( mouse position - k - widthPad ) / slotSize + 1
                if (selectedIndex <= 0 || selectedIndex > deckData.characterCardList.Count-1) {
                    return;
                }

                if (DeckSlot.GetSlotByCard(deckUI.readonlyCardList[selectedIndex]).slotSelected == true) {
                    return;
                }

                //select marking
                DeckSlot selectedSlot = DeckSlot.GetSlotByCard(deckUI.readonlyCardList[selectedIndex]);
                selectedSlot.SlotSelect();

                //Selected Screen correction
                /* slotSize(n-1) + k + 20 -> StartPoint */
                if (deckUI.slotSize * (selectedIndex-1) + cardPanel.position.x < deckUI.horizonPad) {
                    cardPanel.localPosition = new Vector2(0 - (50 * (selectedIndex-1))+ deckUI.horizonPad, cardPanel.localPosition.y);
                }
                if (deckUI.slotSize * (selectedIndex - 1) + cardPanel.position.x + deckUI.slotSize > screenWidth - deckUI.horizonPad) {
                    cardPanel.Translate(-1 * Vector2.right * (deckUI.slotSize * selectedIndex + cardPanel.position.x - screenWidth + deckUI.horizonPad));
                }

                currentSelectedItem = deckUI.GetSelectedItem(selectedIndex);
                dragType = "vertical";
            }
        }
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eve) {
        if (dragType == "vertical") {
            DeckSettingUI selectedDeck;
            DeckUI deckUI = DeckUI.GetInstance();

            if (selectedDeck = DeckSettingUI.SearchEnterMouseObject(eve.position)){
                selectedDeck.SetCardIndex(deckUI.readonlyCardList[selectedIndex]);
            }else {
                DeckSlot selectedSlot = DeckSlot.GetSlotByCard(deckUI.readonlyCardList[selectedIndex]);
                selectedSlot.SlotFree();
                selectedIndex = -1;
            }
            Destroy(currentSelectedItem.gameObject);
        }
        prePos = Vector2.zero;
        dragType = "";
    }
}
