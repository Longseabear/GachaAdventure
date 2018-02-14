using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System;

public class DeckSaveClickEvent : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        DeckController dc = DeckController.GetInstance();
        dc.SaveDeck();
        print("Save OK!");
    }
}
