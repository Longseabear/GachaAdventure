using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NullCard : Card {
    string name = "NULL";
    string type = "CHARACTER";
    int level = 0;
    [NonSerialized]
    Sprite imgSprite;

    public override string Name
    {
        get {
            throw new NotImplementedException();
        }

        set {
            throw new NotImplementedException();
        }
    }

    public override string Type
    {
        get {
            throw new NotImplementedException();
        }

        set {
            throw new NotImplementedException();
        }
    }

    public override int Level
    {
        get {
            throw new NotImplementedException();
        }

        set {
            throw new NotImplementedException();
        }
    }

    public override Sprite ImageSprite
    {
        get {
            throw new NotImplementedException();
        }

        set {
            throw new NotImplementedException();
        }
    }

    public override void init() {
        throw new NotImplementedException();
    }
}
