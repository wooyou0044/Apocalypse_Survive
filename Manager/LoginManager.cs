using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginManager : MonoBehaviour
{
    UserInfo _userInfo;
    SceneControlManager _sceneManager;

    void Awake()
    {
        _sceneManager = GameObject.Find("SceneManagerObject").GetComponent<SceneControlManager>();
        _userInfo = GameObject.Find("UserInfo").GetComponent<UserInfo>();
        DontDestroyOnLoad(_userInfo);
        InitUserInfo();
    }

    void Update()
    {

    }

    public void InitUserInfo()
    {
        // 로그인해서 DB에 있으면 coin setting
        // 없으면 coin 10만 주기 => 일단 지금 바로 넣기
        _userInfo._coin = 100000;
        _userInfo._clearMap = 0;
        // 기본으로 설정되는 정보
        _userInfo._temperature = 36.5f;
        _userInfo._sleepHour = 0;
        _userInfo._moisture = 24;
        // 레벨 올라갈 수록 높아짐
        _userInfo._hp = 500;

        _userInfo._record = 0;

        // 기본무기, 서브무기 설정
        //_userInfo._baseWeapon = PublicDefine.eWeaponKind.BAT;

        _userInfo._baseWeapon = PublicDefine.eWeaponKind.GUN;
        _userInfo._subWeapon = PublicDefine.eWeaponKind.GRENADE;
        _userInfo._subWeaponNum = 5;
    }

    public void LoginButton()
    {
        // ID와 PASSWORD가 같아서 로그인 성공시
        _sceneManager.StartMainScene();
    }
}
