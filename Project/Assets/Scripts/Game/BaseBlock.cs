using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBlock : MonoBehaviour
{
    public SpriteRenderer spriteRender;
    public Animator animator;

    public int level { get; protected set; }
    public Vector2Int logicPos { get; protected set; }

}
