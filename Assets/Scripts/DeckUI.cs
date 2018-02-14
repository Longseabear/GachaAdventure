using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.ObjectModel;
public class DeckUI : MonoBehaviour {
    private static DeckUI instance = null;

    // predefined
    public RectTransform cardPanel;
    public RectTransform selectedPanel;

    private DeckData deckData;
    public List<Card> currentCardList = new List<Card>();
    public ReadOnlyCollection<Card> readonlyCardList
    {
        get {
            return currentCardList.AsReadOnly();
        }
    }

    // Inventory
    public GameObject OriginSlot;


    public float slotSize = 50.0f;
    public float slotPadding = 0.0f;
    public int slotCountX;
    public float horizonPad = 20.0f;

    private float curInvenWidth;

    public static DeckUI GetInstance() {
        if (instance != null)
            return instance;
        return null;
    }
    void Awake() {
        instance = this;

        // Debug
        deckData = DeckData.GetInstance();
        deckData.LoadData();
        
        slotCountX = deckData.characterCardList.Count-1;
        
        if (slotCountX != 0) {
            // indexing initialization
            for (int i = 0; i <= slotCountX; i++) {
                currentCardList.Add(deckData.characterCardList[i]);
            }

            curInvenWidth = (slotCountX * slotSize) + (slotCountX * slotPadding) + slotPadding;

            for (int i = 1; i <= slotCountX; i++) {
                GameObject slotObj = Instantiate(OriginSlot) as GameObject;
                DeckSlot slot = slotObj.GetComponent<DeckSlot>();
                slot.InitializeDeckSlot(readonlyCardList[i]);

                RectTransform slotRect = slot.GetComponent<RectTransform>();
                slotRect = slot.GetComponent<RectTransform>();

                // get item component
                RectTransform item = slot.GetItemGameObject().GetComponent<RectTransform>();
                slot.name = "slot_" + i;
                slot.transform.parent = cardPanel.transform;

                slotRect.localPosition = new Vector3((slotSize * (i-1)) + (slotPadding * i), 0, 0);

                slotRect.localScale = Vector3.one;
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50);

                slotRect.offsetMin = new Vector2(slotRect.offsetMin.x, 0);
                slotRect.offsetMax = new Vector2(slotRect.offsetMax.x, 0);

                Image img = item.GetComponent<Image>();
                img.sprite = slot.card.ImageSprite;

                // code for test
                if (slot.card.Level==1)
                    img.color = Color.red;
            }
        }
    }
    void Update() {
        Camera _camera = Camera.main;

        float screenWidth = _camera.pixelWidth;
        float screenHeight = _camera.pixelHeight;

        if (slotCountX * slotSize * -1 + screenWidth - horizonPad > cardPanel.localPosition.x)
            cardPanel.localPosition = new Vector2(slotCountX * slotSize * -1 + screenWidth - horizonPad, cardPanel.localPosition.y);
        if (horizonPad < cardPanel.localPosition.x)
            cardPanel.localPosition = new Vector2(horizonPad, cardPanel.localPosition.y);
    }
    public GameObject GetSelectedItem(int selectedIndex) {
        GameObject slot = Instantiate(OriginSlot) as GameObject;
        RectTransform slotRect = slot.GetComponent<RectTransform>();
        DeckSlot deckSlot = slot.GetComponent<DeckSlot>();
        deckSlot.InitializeDeckSlot(readonlyCardList[selectedIndex]);

        // get item component
        RectTransform item = deckSlot.GetItemGameObject().GetComponent<RectTransform>();

        slot.name = "selected_slot_" + selectedIndex;
        slot.transform.parent = selectedPanel.transform;

        // don't need DeckSlot in case of selected slot
        Destroy(slot.GetComponent<DeckSlot>());

        // slotSize(n-1) + k  -> StartPoint */
        slotRect.localPosition = new Vector3(slotSize * (selectedIndex-1) + cardPanel.position.x , 0, 0);

        slotRect.localScale = Vector3.one;
        slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50);

        slotRect.offsetMin = new Vector2(slotRect.offsetMin.x, 0);
        slotRect.offsetMax = new Vector2(slotRect.offsetMax.x, 0);

        slotRect.pivot = new Vector2(0.5f, 0.5f);

        DeckSlot seletedDeckSlot = DeckSlot.GetSlotByCard(readonlyCardList[selectedIndex]);

        // Image Component Copy
        Image selectedImg = item.GetComponent<Image>();
        Image copyImage = seletedDeckSlot.GetItemGameObject().GetComponent<Image>();

        Util.CopyImage(copyImage, selectedImg);
        return slot;
    }
}
