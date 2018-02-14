using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
public class DeckData {
    private static DeckData instance = null;

    // constant
    const int MAX_DECK_NUM = 10;
    const int MAX_DECK_COLUM = 5;

    // card data
    // Start from index 1
    public int[,] deck = new int[MAX_DECK_NUM, MAX_DECK_COLUM];
    public List<Card> characterCardList = new List<Card>(); // Invarient

    // Data Manage
    public bool loaded = false;
    IFormatter serializer = new BinaryFormatter();

    public bool CheckAlreadySelectedCard(Card c) {
        int idx = GetCardIndex(c);
        for(int i=1;i< MAX_DECK_NUM; i++) {
            for(int j=1; j<MAX_DECK_COLUM; j++) {
                if(deck[i,j] == idx) {
                    return true;
                }
            }
        }
        return false;
    }
    public int GetCardIndex(Card c) {
        return characterCardList.IndexOf(c);
    }
    public static DeckData GetInstance() {
        if (instance == null) {
            instance = new DeckData();
        }
        return instance;
    }
    public DeckData() {
        characterCardList.Add(new NullCard());
    }
    public void LoadData() {
        using (FileStream fs = new FileStream(Application.dataPath + "/Data/deck.data", FileMode.Open, FileAccess.Read, FileShare.None)) {
            deck = serializer.Deserialize(fs) as int[,];
        }
        using (FileStream fs = new FileStream(Application.dataPath + "/Data/characterCardList.data", FileMode.Open, FileAccess.Read, FileShare.None)) {
            characterCardList = serializer.Deserialize(fs) as List<Card>;
        }
        foreach(Card c in characterCardList) {
            if (c == characterCardList[0])
                continue;
            c.init();
        }
        loaded = true; 
    }
    public void SaveData() {
        using (FileStream fs = new FileStream(Application.dataPath + "/Data/deck.data", FileMode.Create, FileAccess.Write, FileShare.None)) {
            serializer.Serialize(fs, deck);
        }
        using (FileStream fs = new FileStream(Application.dataPath + "/Data/characterCardList.data", FileMode.Create, FileAccess.Write, FileShare.None)) {
            serializer.Serialize(fs, characterCardList);
        }
    }
    public Card GetNullCard() {
        return characterCardList[0];
    }
}