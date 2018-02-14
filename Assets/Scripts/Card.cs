using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using UnityEngine.UI;

[Serializable]
public abstract class Card{
    public abstract void init();
    public abstract string Name { get; set; }
    public abstract string Type { get; set; }
    public abstract int Level { get; set; }
    public abstract Sprite ImageSprite {  get; set; }
}
