using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MTUnity.Actions;

public enum BombType
{
    
    Square,
    Up,
    Right,
    Down,
    Left,
    None
}

public enum CellType
{
    Cell,
    Bomb
}

public class Cell : MonoBehaviour {

    public SpriteRenderer CellBg;
    public SpriteRenderer CellCenter;
    public Transform CellCenterTrans;
    public SpriteRenderer Number;
    public Text TestText;

    public CellType _cellType = CellType.Cell;
    public BombType _bombType = BombType.None;

    private void Start()
    {

    }

    public void SetBgColor(Color c)
    {
        CellBg.color = c;
    }

    public void SetCenterColor(Color c)
    {
        CellCenter.color = c;
    }

    public Unit unit = null;
    const int BOMBINDEX = 6;
    public int number = 2;
    public int pow = 0;
    public void SetPow(int n)
    {
        pow = n;
        var bgColor = ResMgr.Current.cbg[n  ];
        var cellColor = ResMgr.Current.Colors[n];
        SetCenterColor(cellColor);
        SetBgColor(bgColor);
        if (pow < BOMBINDEX)
        {
            number = (int)System.Math.Pow(2, n);
            Number.sprite = ResMgr.Current.Numbers[n];
            
        }
        else
        {
            LevelMgr.Current.OnGenBomb();
            _cellType = CellType.Bomb;
            int index =  MTRandom.GetRandomInt(0, 4);
            
            _bombType = (BombType)index;
            Number.sprite = ResMgr.Current.Bombs[index];
            

        }
   
    }

    internal void MoveToUnit()
    {
        var tarPos = TargetPos();
        this.RunAction(new MTMoveTo(0.1f, tarPos));
    }

    bool isCellActive = false;
    internal void SetActive()
    {
        isCellActive = true;
    }
    private void Update()
    {
        if(isCellActive)
        {

            transform.Translate(Vector3.up * 0.15f);
        }
  
        CellCenterTrans.rotation = Quaternion.identity;
    }
    public bool isAttached = false;

    const float CELL_SIZE = 0.426f;
   

    GameObject GenRocket()
    { 
                    SoundMgr.Current.PlayRocketSound();
           

            var fireWork = Instantiate<GameObject>(ResMgr.Current.FireWorkPrefab);
    
            fireWork.transform.position = transform.position - Vector3.forward *0.1f;
        return fireWork;
    }

    public void DestoryAndGenCoin()
    {
        int m = MTRandom.GetRandomInt(0, 2);
        if (m == 0)
        {
            var coin = Instantiate<GameObject>(ResMgr.Current.CoinPrefab);
            coin.transform.position = transform.position;
            SoundMgr.Current.PlayCoinCameOut();
        }
        DestroyCell();
    }


    bool isDestroying = false;
    public void DestroyCell()
    {
        if (isDestroying)
        {
            return;
        }
        isDestroying = true;
        if (_cellType == CellType.Bomb)
        {
        }
        else
        {
            if (unit != null)
            {
                unit.cell = null;
            }
            unit = null;
            PlayDestoryAnim();

        }
    }

    void PlayDestoryAnim()
    {
        this.RunActions(new MTSequence(new MTScaleTo(0.1f, 1.1f), new MTScaleTo(0.08f, 0.1f)), new MTDestroy());

    }

    public const float MERGE_TIME = 0.1f;
    public bool isMerging = false;


    public int corX = 0;
    public int corY = 0;
    public void SetLocalCoord()
    {
        corX = (int)(Math.Round(transform.localPosition.x / CELL_SIZE)) + 10;
        corY = (int)(Math.Round(transform.localPosition.y / CELL_SIZE)) + 10;
        SetTestText();
        LevelMgr.Current.SetCells(corX, corY, this);
    }

    public void SetTestText()
    {
        return;
        TestText.text = corX.ToString() + " " + corY.ToString();
    }

    public void SetPostion(int x, int y)
    {
        corX = x;
        corY = y;
        transform.localPosition = TargetPos();
        transform.localRotation = Quaternion.identity;
    }

    Vector3 TargetPos()
    {
        return new Vector3((corX - 10) * CELL_SIZE, (corY - 10) * CELL_SIZE, 0);
    }

    private void IncreaseNum()
    {
        SetPow(pow + 1);
        this.RunAction(GetScale());
    }

    MTFiniteTimeAction GetScale()
    {
        return new MTSequence(new MTScaleTo(0.1f, 0.8f), new MTScaleTo(0.08f, 1f));
    }
}
