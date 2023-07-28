using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool _isEnter;
    public bool _isExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isEnter = true;
        _isExit = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isEnter = false;
        _isExit = true;
    }

}
