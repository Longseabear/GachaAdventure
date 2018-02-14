using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DeckController : MonoBehaviour{
    private static DeckController instance = null;
    private static DeckData deckData;
    public GameObject UI;
    public DeckUI deckUI;
    public static DeckController GetInstance() {
        if (instance == null) {
            deckData = DeckData.GetInstance();
        }
        return instance;
    }
    public void Awake() {
        if (instance == null) {
            instance = this;
            deckData = DeckData.GetInstance();
        }

        //Test
        //        ControllerDebug();
        OpenDeckSimulator();
    }
    public void ControllerDebug() {
        deckData.characterCardList.Add(new TestCard2());
        deckData.characterCardList.Add(new TestCard2());
        deckData.characterCardList.Add(new TestCard2());
        deckData.characterCardList.Add(new TestCard());
        deckData.characterCardList.Add(new TestCard2());
        deckData.characterCardList.Add(new TestCard2());
        deckData.characterCardList.Add(new TestCard());
        deckData.characterCardList.Add(new TestCard2());
        deckData.characterCardList.Add(new TestCard2());
        deckData.characterCardList.Add(new TestCard2());
        deckData.characterCardList.Add(new TestCard());
        deckData.characterCardList.Add(new TestCard());
        deckData.characterCardList.Add(new TestCard2());
        deckData.characterCardList.Add(new TestCard2());
        deckData.characterCardList.Add(new TestCard());
        deckData.characterCardList.Add(new TestCard2());
        deckData.characterCardList.Add(new TestCard2());
        SetDeckData(deckData.characterCardList[4], 0, 1);
        SaveDeck();
        //Test
    }
    public void SetDeckData(Card c, int decNum, int decColum) {
        deckData.deck[decNum, decColum] = deckData.GetCardIndex(c);
    }
    public void OpenDeckSimulator() {
        // UI -> include DeckData.Load
        UI = Instantiate(UI) as GameObject;
        deckUI = DeckUI.GetInstance();
        DeckSettingUI.synchronizeAll();
    }
    public void CloseDeckSimulator() {
        // UI -> include DeckData.Load
        UI = Instantiate(UI) as GameObject;
        deckUI = DeckUI.GetInstance();

        DeckSettingUI.synchronizeAll();
    }
    public void SaveDeck() {
        deckData.SaveData();
    }
}
