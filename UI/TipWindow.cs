using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipWindow : MonoBehaviour
{
    Text _tip;
    EyeCast _eyeCast;
    float _time = 0;

    MapManager _mapManager;

    void Awake()
    {
        _tip = GetComponentInChildren<Text>();
    }

    void Update()
    {
        _tip.color = Color.white;
        if (CrossHair.GazeStatus && !EyeCast.IsDestroy)
        {
            _eyeCast = GameObject.Find("PlayerCamera").GetComponent<EyeCast>();
            _mapManager = GameObject.Find("MapSceneManager").GetComponent<MapManager>();

            if (_eyeCast._isItem)
                _tip.text = _eyeCast.GetSlotItem._itemName + " 획득(E)";
            if(_eyeCast._isWeapon)
                _tip.text = _eyeCast.GetWeaponItem._weaponName + " 획득(E)";
            if (_eyeCast._isTree)
                SeeTree();
            if(_eyeCast._isDoor)
            {
                if (_eyeCast._isOpen)
                    _tip.text = "문 닫기(Q)";
                if (!_eyeCast._isOpen)
                    _tip.text = "문 열기(Q)";
            }
            if(_eyeCast._isDrawer)
            {
                if (_eyeCast._isDrawerOpen)
                    _tip.text = "서랍 닫기(Q)";
                if (!_eyeCast._isDrawerOpen)
                    _tip.text = "서랍 열기(Q)";
            }
        }
        else if (!EyeCast.IsDestroy)
            gameObject.SetActive(false);

        if (EyeCast.IsDestroy)
        {
            gameObject.SetActive(true);
            if (_eyeCast._isItem)
                _tip.text = _eyeCast.GetSlotItem._itemName + " 획득했습니다.";
            else
                _tip.text = _eyeCast.GetWeaponItem._weaponName + " 획득했습니다.";
            _time += Time.deltaTime;    
            if (_time >= 1.0f)
            {
                EyeCast.IsDestroy = false;
                _time = 0;
            }
        }

        if (_mapManager != null)
        {
            if (_mapManager.IsMakePos)
            {
                gameObject.SetActive(true);
                _tip.text = "놓으려면 마우스 오른쪽 버튼을 클릭해주세요";
                _tip.color = new Color(1, 194 / 255f, 0);
            }
        }
    }

    void SeeTree()
    {
        // 이건 나중에 prefab으로 Map에 instantiate하는 거면 Tag로 바꾸기
        //TreeControl treeControl = GameObject.Find("Tree").GetComponent<TreeControl>();
        _tip.text = "나무를 베시겠습니까?(남은 나무 HP = " + _eyeCast.GetTreeLife + ")";
    }
}
