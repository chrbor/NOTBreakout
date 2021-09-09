using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    private static Dictionary<GameObject, GameObject> texts = new Dictionary<GameObject, GameObject>();


    public static void AddText(GameObject target, string text){ AddText(target, text, 1, Vector2.zero); }
    public static void AddText(GameObject target, string text, float scale){ AddText(target, text, scale, Vector2.zero); }
    public static void AddText(GameObject target, string text, Vector2 offset){ AddText(target, text, 1, offset); }
    public static void AddText(GameObject target, string text, float scale, Vector2 offset)
    {
        if (texts.ContainsKey(target)) return;

        //texts.Add(target, )
    }

    public static void ChangeText(GameObject target, string text){ ChangeText(target, text, 1, Vector2.zero); }
    public static void ChangeText(GameObject target, float scale) { ChangeText(target, "", scale, Vector2.zero); }
    public static void ChangeText(GameObject target, Vector2 offset) { ChangeText(target, "", 1, offset); }
    public static void ChangeText(GameObject target, string text, float scale, Vector2 offset)
    {
        if (!texts.ContainsKey(target)) return;
    }

    public static void DeleteText(GameObject target)
    {
        if (!texts.ContainsKey(target)) return;
        texts.Remove(target);
    }
}
