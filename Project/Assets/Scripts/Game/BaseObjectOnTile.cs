using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseObjectOnTile : BaseBlock
{
    private ObjectType m_ObjectType;
    public int index { get; protected set; }

    public Vector2Int initLogicPos { get; protected set; }

    public ObjectType objectType {
        get
        {
            return m_ObjectType;
        }
        protected set
        {
            m_ObjectType = value;
            OnObjectTypeChanged(value);
        }
    }

    private void SetData(int level, Vector2Int logicPos, int typeInt)
    {
        var objectType = (ObjectType)(typeInt);

        //设置位置
        var levelData = ConfigDataHolder.levelDataDict[level];
        int matrixX = levelData[0].Length;
        int matrixY = levelData.Length;
        transform.position = PositionRelateMethods.
            CalcLevelBlockPosition(matrixX, matrixY, logicPos);

        this.level = level;
        this.logicPos = logicPos;
        this.initLogicPos = logicPos;
        this.objectType = objectType;
        
    }

    private void OnObjectTypeChanged(ObjectType type)
    {
        spriteRender.sprite =
            GameMain.Instance.artResManager.FindObjectOnTileSprite((int)type);

        animator.runtimeAnimatorController =
            GameMain.Instance.artResManager.FindObjectOnTileAnim((int)type);
    }

    protected virtual void EndMove(Tile endTile) { }


    public virtual void SetData(int level, Vector2Int logicPos, int typeInt, int index)
    {
        SetData(level, logicPos, typeInt);
        this.index = index;
    }

    public void MoveToTile(Tile nextTile)
    {
        logicPos = nextTile.logicPos;
        transform.DOKill();
        transform.DOLocalMove(nextTile.transform.localPosition, GameMain.Instance.unitDeltaTime);
        EndMove(nextTile);
    }

    //重置到初始
    public virtual void ResetToInit()
    {
        logicPos = initLogicPos;
        var initTile = GameMain.Instance.levelManager.GetCurrentLevel().GetTile(initLogicPos);
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBounce).
        OnComplete(()=>{
             transform.localPosition = initTile.transform.localPosition;
             transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBounce);
        });

    }
}
