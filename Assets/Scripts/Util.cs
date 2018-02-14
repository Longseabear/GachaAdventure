using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Util {
    public static Transform[] getFirstChildren(Transform parent) {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        Transform[] firstChildren = new Transform[parent.childCount];
        int index = 0;
        foreach (Transform child in children) {
            if (child.parent == parent) {
                firstChildren[index] = child;
                index++;
            }
        }
        return firstChildren;
    }
    public static void CopyImage(Image src, Image dst) {
        dst.sprite = src.sprite;
        dst.color = src.color;
    }
}
