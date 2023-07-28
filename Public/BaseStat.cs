using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat : MonoBehaviour
{
    protected int _hp;
    protected int _nowHP;
    protected int _powAtt;
    protected int _powDef;

    public float _hpRate
    {
        get { return (float) _nowHP / _hp; }
    }

    protected void InitSetData(int hp, int powAtt, int powDef)
    {
        _hp = _nowHP = hp;
        _powAtt = powAtt;
        _powDef = powDef;
    }

    public bool CalcHit(int damageNum)
    {
        int random = Random.Range(0, 100);
        if (random > _powDef)
            _nowHP -= damageNum;

        if (_nowHP >= _hp)
            _nowHP = _hp;

        if(_nowHP <=0)
        {
            _nowHP = 0;
            return true;
        }
        return false;
    }
}
