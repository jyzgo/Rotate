using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Number : MonoBehaviour {

    public Text _text;

    int _num = 0;

    // Use this for initialization
    public void Init(int x)
    {
        _num = x;
        _text.text = x.ToString();
    }

    public int GetNum()
    {
        return _num;
    }

    public void Fire()
    {
        isFire = true;
    }

    private void Update()
    {
        if (isFire)
        {
            transform.Translate(Vector3.up * 0.1f);
        }
    }

    bool isFire = false;

   

    public void Increase()
    {
        _num = _num * 2;
        _text.text = _num.ToString();
    }

    void Hitted()
    {
        isAttached = true;
        LevelMgr.Current.Hitted(_num);
    }

    public bool isAttached = false;


}
