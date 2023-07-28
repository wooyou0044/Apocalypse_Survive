using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockButtonController : MonoBehaviour
{
    [SerializeField] GameObject _prefabLockPreview;
    [SerializeField] Image _pwdPreviewBack;
    [SerializeField] GameObject _prefabError;

    int _pwdNum;
    string _password;

    List<int> _ltPassword;

    UserInfo _userInfo;
    BankManager _bankManage;
    IngameManager _ingameManage;
    InventoryManager _inventManage;

    List<Text> _ltPwdPreview;
    Text _pwdPreviewText;
    int _clickNum;
    bool _isChange;

    void Awake()
    {
        _ltPassword = new List<int>();
        _ltPwdPreview = new List<Text>();
        _userInfo = GameObject.Find("UserInfo").GetComponent<UserInfo>();
        _bankManage = GameObject.Find("BankManagerObject").GetComponent<BankManager>();
        _ingameManage = GameObject.Find("IngameManagerObject").GetComponent<IngameManager>();
        _inventManage = GameObject.Find("InventoryBack").GetComponent<InventoryManager>();

        _clickNum = 0;
        InitPassword();
    }

    void Update()
    {
        if(_isChange)
        {
            _ltPwdPreview[_clickNum].text = _pwdNum.ToString();
            _clickNum++;
            _isChange = false;
        }
    }

    void InitPassword()
    {
        if (_userInfo._clearMap == 0)
            _password = "3896";
        if (_userInfo._clearMap == 1)
            _password = "41028";
        if (_userInfo._clearMap == 2)
            _password = "520896";
        if (_userInfo._clearMap == 3)
            _password = "8795012";
        if (_userInfo._clearMap == 4)
            _password = "96001472";
        int _pwdLength = _password.Length;
        for (int n = 0; n < _pwdLength; n++)
        {
            GameObject go = Instantiate(_prefabLockPreview, _pwdPreviewBack.transform.position + new Vector3((n*150)-220,0,0), _pwdPreviewBack.transform.rotation, _pwdPreviewBack.transform);
            _pwdPreviewText = go.GetComponentInChildren<Text>();
            _pwdPreviewText.text = string.Empty;
            _ltPwdPreview.Add(_pwdPreviewText);
        }
    }

    public void Button1()
    {
        _pwdNum = 1;
        _ltPassword.Add(_pwdNum);
        _isChange = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.BankLockButton);
    }

    public void Button2()
    {
        _pwdNum = 2;
        _ltPassword.Add(_pwdNum);
        _isChange = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.BankLockButton);
    }
    public void Button3()
    {
        _pwdNum = 3;
        _ltPassword.Add(_pwdNum);
        _isChange = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.BankLockButton);
    }
    public void Button4()
    {
        _pwdNum = 4;
        _ltPassword.Add(_pwdNum);
        _isChange = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.BankLockButton);
    }
    public void Button5()
    {
        _pwdNum = 5;
        _ltPassword.Add(_pwdNum);
        _isChange = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.BankLockButton);
    }
    public void Button6()
    {
        _pwdNum = 6;
        _ltPassword.Add(_pwdNum);
        _isChange = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.BankLockButton);
    }
    public void Button7()
    {
        _pwdNum = 7;
        _ltPassword.Add(_pwdNum);
        _isChange = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.BankLockButton);
    }
    public void Button8()
    {
        _pwdNum = 8;
        _ltPassword.Add(_pwdNum);
        _isChange = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.BankLockButton);
    }
    public void Button9()
    {
        _pwdNum = 9;
        _ltPassword.Add(_pwdNum);
        _isChange = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.BankLockButton);
    }
    public void Button0()
    {
        _pwdNum = 0;
        _ltPassword.Add(_pwdNum);
        _isChange = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.BankLockButton);
    }
    public void ButtonShap()
    {
        string pwd = "";
        for(int n=0; n<_ltPassword.Count; n++)
        {
            pwd += _ltPassword[n].ToString();
        }

        // 비밀번호가 같으면 꺼지고 금고 안에 나옴
        if(pwd == _password)
        {
            SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.LockOpen);
            _bankManage._isOpen = true;
            gameObject.SetActive(false);
        }
        else
        {
            SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.LockError);
            GameObject go = Instantiate(_prefabError, _pwdPreviewBack.transform);
            Destroy(go, 3.0f);
            _ltPassword = new List<int>();
            for (int n = 0; n < _ltPwdPreview.Count; n++)
            {
                _ltPwdPreview[n].text = string.Empty;
                _clickNum = 0;
            }
        }
    }

    public void ButtonErase()
    {
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.LockErase);
        _ltPassword.RemoveAt(_ltPassword.Count - 1);
        _ltPwdPreview[_clickNum - 1].text = string.Empty;
        _clickNum--;
    }

    public void BackToMap()
    {
        _ingameManage.GoToMapInUI();
    }

    public void ClueButton()
    {
        // inventory에 단서를 발견하면
        _ingameManage.CreateClue();
    }
}
