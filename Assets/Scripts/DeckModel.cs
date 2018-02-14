using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckModel {
    private static DeckModel instance = null;
    private static DeckData deckData;
    public static DeckModel GetInstance() {
        if (instance == null) {
            instance = new DeckModel();
        }
        return instance;
    }
}
