using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableArtDict", menuName = "ScriptableConfig/ScriptableArtDict")]
public class ScriptableArtDict : ScriptableObject
{
    public List<Sprite> tileSprites;
    public List<RuntimeAnimatorController> tileAnims;

    public List<Sprite> objOnTileSprites;
    public List<RuntimeAnimatorController> objOnTileAnims;
}
