using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TestCard : Card {
    string name;
    string type;
    int level;
    [NonSerialized]
    Sprite imgSprite;

    public TestCard() {
        init();
    }
    public override void init() {
        // must to modify
        name = "TestCard";
        type = "CHARACTER";
        level = 1;
        // auto
        imgSprite = Resources.Load<Sprite>("Sprites/" + name);
    }
    public override string Name
    {
        get {
            return name;
        }
        set {
            name = value;
        }
    }

    public override int Level
    {
        get {
            return level;
        }
        set {
            level = value;
        }
    }

    public override string Type
    {
        get {
            return type;
        }
        set {
            type = value;
        }
    }

    public override Sprite ImageSprite
    {
        get {
            return imgSprite;
        }

        set {
            imgSprite = value;
        }
    }
}
