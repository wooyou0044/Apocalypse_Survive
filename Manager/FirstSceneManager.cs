using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstSceneManager : MonoBehaviour
{
    [SerializeField] GameObject _loginSection;
    [SerializeField] GameObject _signUpSection;
    [SerializeField] GameObject _moveButton;

    SignUpManager _signUpManager;
    LoginManager _loginManager;
    Canvas _canvas;
    Text _moveText;
    Transform _logoTrans;

    float _time;

    bool _isLogin;
    bool _isMove;
    bool _isSignUp;

    Vector3 _dir;

    public bool isSignUp
    {
        get { return _isSignUp; }
        set { _isSignUp = value; }
    }

    void Awake()
    {
        SoundManager._instance.PlayBGMSound(PublicDefine.eBGMType.UIScene);
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        _moveText = _moveButton.GetComponentInChildren<Text>();
        _moveButton.SetActive(false);
        _isLogin = true;
        _logoTrans = GameObject.Find("Logo").transform;
        _moveText.text = "회원가입";
    }

    void Update()
    {
        if(_isSignUp)
        {
            _loginManager.gameObject.SetActive(true);
            _signUpManager.gameObject.SetActive(false);
            _moveText.text = "회원가입";
            _isSignUp = false;

            // 회원가입 성공하면 회원가입 성공했다는 표시 나오게 하기
        }

        if(_isMove)
        {
            _time += Time.deltaTime;
            _dir = new Vector3(0, _logoTrans.position.y, 0);
            _logoTrans.Translate(_dir * Time.deltaTime * 0.1f);
            if(_time >= 3.5f)
            {
                CreateLogin();
                _isMove = false;
            }
        }
    }

    public void StartButton()
    {
        _isMove = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.FirstButton);
    }

    public void MoveButton()
    {
        _isLogin = !_isLogin;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.SignUpNLoginButton);
        if (_isLogin)
        {
            _loginManager.gameObject.SetActive(true);
            _signUpManager.gameObject.SetActive(false);
            _moveText.text = "회원가입";
        }
        else
        {
            _loginManager.gameObject.SetActive(false);
            _moveText.text = "로그인";
            if (_signUpManager == null)
            {
                GameObject go = Instantiate(_signUpSection, _canvas.transform);
                _signUpManager = go.GetComponent<SignUpManager>();
            }
            else
                _signUpManager.gameObject.SetActive(true);
        }
    }

    void CreateLogin()
    {
        GameObject go = Instantiate(_loginSection, _canvas.transform);
        _loginManager = go.GetComponent<LoginManager>();
        _moveButton.SetActive(true);
    }
}
