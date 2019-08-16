using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtResManager: MonoBehaviour
{
    public ScriptableArtDict scriptableArtDict;

    public Sprite FindTileSprite(int nameInt)
    {
        foreach (var s in scriptableArtDict.tileSprites)
        {
            if (s.name == nameInt.ToString())
                return s;
        }
        return null;
    }

    public RuntimeAnimatorController FindTileAnim(int nameInt)
    {
        foreach (var anim in scriptableArtDict.tileAnims)
        {
            if (anim.name == nameInt.ToString())
                return anim;
        }
        return null;
    }

    public Sprite FindObjectOnTileSprite(int nameInt)
    {
        foreach (var s in scriptableArtDict.objOnTileSprites)
        {
            if (s.name == nameInt.ToString())
                return s;
        }
        return null;
    }

    public RuntimeAnimatorController FindObjectOnTileAnim(int nameInt)
    {
        foreach (var anim in scriptableArtDict.objOnTileAnims)
        {
            if (anim.name == nameInt.ToString())
                return anim;
        }
        return null;
    }
}
