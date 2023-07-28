using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignUpManager : MonoBehaviour
{
    FirstSceneManager _firstManager;

    // 중복체크 모두 해서 둘 다 true가 나오면 회원가입됨

    void Awake()
    {
        _firstManager = GameObject.Find("FirstSceneManager").GetComponent<FirstSceneManager>();
    }

    void Update()
    {
        
    }

    public void SignUpButton()
    {
        // 로그인 페이지 나오게 하기
        _firstManager.isSignUp = true;
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.StartButton1);
    }
}
