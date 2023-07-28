using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public string _nickName;
    public long _coin;
    public int _clearMap;
    public int _hp;
    public int _moisture;
    public float _temperature;
    public float _sleepHour;
    public int _record;
    public PublicDefine.eWeaponKind _baseWeapon;
    public PublicDefine.eWeaponKind _subWeapon;
    public int _subWeaponNum;

    public static UserInfo _uniqueInstance;

    public static UserInfo _instance
    {
        get { return _uniqueInstance; }
    }
}
