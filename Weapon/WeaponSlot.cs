using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    public Weapon _weapon;
    public Image _weaponImg;
    public Text _weaponText;
    public int _count;

    public void AddWeaponSlot(Weapon weapon, int grenadeCount = 1)
    {
        _weaponImg = transform.GetChild(0).GetComponent<Image>();
        _weaponText = transform.GetChild(1).GetComponent<Text>();

        _weapon = weapon;
        _weaponImg.sprite = weapon._weaponImage;
        if (weapon._weaponNum == 106)
        {
            _count = grenadeCount;
            _weaponText.text = _count.ToString();
        }
        else
        {
            _count = 1;
            _weaponText.enabled = false;
        }
    }

    public void SetCount(Weapon weapon)
    {
        if (weapon._weaponNum == 106)
        {
            _count++;
            _weaponText.text = _count.ToString();
            //if (_count <= 0)
            //    ClearWeapon();
        }
        else
        {
            _count = 1;
            _weaponText.enabled = false;
        }
    }

    public void ShootGrenadeCount()
    {
        _count--;
        if (_count < 0)
            _count = 0;
        _weaponText.text = _count.ToString();
        Debug.Log("Count : " + _count);
    }

    public void PickUpGrenade()
    {
        _count++;
        _weaponText.text = _count.ToString();
        Debug.Log("Count : " + _count);
    }

    void ClearWeapon()
    {
        _weapon = null;
        _weaponImg.sprite = null;
        _count = 0;
        _weaponText.text = _count.ToString();
    }
}
