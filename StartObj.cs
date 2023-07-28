using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObj : MonoBehaviour
{
    SceneControlManager _sceneControl;

    void Awake()
    {
        _sceneControl = GameObject.Find("SceneManagerObject").GetComponent<SceneControlManager>();
        SoundManager._instance.InitSetReference();
        _sceneControl.ApplicationStart();
    }

}
