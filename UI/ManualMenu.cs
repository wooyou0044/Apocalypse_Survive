using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualMenu : MonoBehaviour
{
    public void CloseWindow()
    {
        SoundManager._instance.PlayEffectSound(PublicDefine.eEffectSoundType.ManualButton);
        gameObject.SetActive(false);
    }
}
