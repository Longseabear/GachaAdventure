using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DeckSettingUI : MonoBehaviour, IEndDragHandler , IDragHandler, IBeginDragHandler {
    
    static public List<DeckSettingUI> deckSettingList = new List<DeckSettingUI>();
    public Card card = null;
    private Image item;
    public Canvas sortCanvas;
    // For Event Att
    public int deckColIdx = 0;
    Vector2 prePos, originalPos;
    bool selected;

    //Commu
    DeckController dc = DeckController.GetInstance();

    //loop function ( All Deck Setting UI )
    public static DeckSettingUI SearchEnterMouseObject(Vector2 pos) {
        foreach(DeckSettingUI obj in deckSettingList){
            DeckSettingUI res = obj.SearchObject(pos);
            if (res != null)
                return res;
        }
        return null;
    }
    public static void synchronizeAll() {
        foreach (DeckSettingUI obj in deckSettingList) {
            obj.SynchronizeDeckData();
        }
    }

    public void SetCardIndex(Card inputCard) {
        DeckUI deckUI = DeckUI.GetInstance();

        if (card != null) {
            DeckSlot previousSlot = DeckSlot.GetSlotByCard(card);
            previousSlot.SlotFree();
        }
        DeckSlot selectedSlot = DeckSlot.GetSlotByCard(inputCard);
        selectedSlot.SlotSelect();

        card = inputCard;
        // Image Component Copy
        Image copyImage = DeckSlot.GetSlotByCard(inputCard).GetItemGameObject().GetComponent<Image>();

        Util.CopyImage(copyImage, item);

        // regist -> Controller
        dc.SetDeckData(card, 0, deckColIdx);
    }
    public void FreeCardIndex() {
        if (card != null) {
            DeckSlot previousSlot = DeckSlot.GetSlotByCard(card);
            previousSlot.SlotFree();
        }
        card = null;
        item.color = new Color(0, 0, 0, 0);

        // Free -> Controller
        DeckData deckData = DeckData.GetInstance();
        dc.SetDeckData(deckData.GetNullCard(), 0, deckColIdx);
    }
    void Awake() {
        DeckSettingUI.deckSettingList.Add(this);
        item = GetComponentsInChildren<Image>()[1];
        sortCanvas = GetComponentInChildren<Canvas>();
        deckColIdx = gameObject.name[4] - '0';
    }
    public DeckSettingUI SearchObject(Vector2 pos) {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt.position.x <= pos.x && pos.x <= rt.position.x + rt.rect.width)
            if (rt.position.y <= pos.y && pos.y <= rt.position.y + rt.rect.height)
                return this;
        return null;
    }
    public void SynchronizeDeckData() {
        DeckData deckData = DeckData.GetInstance();
        int cardIdx = deckData.deck[0, deckColIdx];
        if(cardIdx <= 0) {
            return;
        }
        Card c = deckData.characterCardList[cardIdx];
        card = c;

        DeckUI deckUI = DeckUI.GetInstance();

        DeckSlot selectedSlot = DeckSlot.GetSlotByCard(card);
        selectedSlot.SlotSelect();

        // Image Component Copy
        Image copyImage = DeckSlot.GetSlotByCard(card).GetItemGameObject().GetComponent<Image>();
        Util.CopyImage(copyImage, item);
    }

    //Event
    void IBeginDragHandler.OnBeginDrag(PointerEventData eve) {
        prePos = eve.position;
        sortCanvas.overrideSorting = true;
        sortCanvas.sortingOrder = 2;
        originalPos = item.transform.position;
    }

    void IDragHandler.OnDrag(PointerEventData eve) {
        Vector2 moveOffset = eve.position - prePos;

        DeckUI deckUI = DeckUI.GetInstance();
        item.GetComponent<RectTransform>().Translate(moveOffset);
        prePos = eve.position;
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eve) {
        sortCanvas.overrideSorting = false;
        RectTransform rt = GetComponent<RectTransform>();
        item.GetComponent<RectTransform>().position = originalPos;
        if (rt.position.x <= eve.position.x && eve.position.x <= rt.position.x + rt.rect.width)
            if (rt.position.y <= eve.position.y && eve.position.y <= rt.position.y + rt.rect.height) {
                return;
            }
        FreeCardIndex();
    }
}
