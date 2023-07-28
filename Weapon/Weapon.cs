using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "New Weapon/Weapon")]
public class Weapon : ScriptableObject
{
    public int _weaponNum;
    public string _weaponName;
    public PublicDefine.eWeaponKind _weaponType;
    public Sprite _weaponImage;
    public GameObject _weaponPrefab;
    public int _damageNum;

    public Weapon(int num, string name, PublicDefine.eWeaponKind wType, Sprite img, int damageNum)
    {
        _weaponNum = num;
        _weaponName = name;
        _weaponType = wType;
        _weaponImage = img;
        _weaponPrefab = null;
        _damageNum = damageNum;
    }
}
