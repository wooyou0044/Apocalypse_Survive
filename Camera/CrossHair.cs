using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    static bool _isGaze;
    [SerializeField] float _durSpeed = 0.2f;
    [SerializeField] float _minSize = 0.7f;
    [SerializeField] float _maxSize = 1.0f;
    [SerializeField] Color _gazeColor = new Color(1, 194 / 255f, 0);

    Color _originColor;
    Image _cross;
    Animator _anim;
    float _timeStart;

    Canvas _canvas;

    public static bool GazeStatus
    {
        get { return _isGaze; }
        set { _isGaze = value; }
    }
    void Awake()
    {
        _cross = GetComponent<Image>();
        _anim = GetComponent<Animator>();
        _originColor = _cross.color;
        transform.localScale = Vector3.one * _minSize;
        _timeStart = Time.time;
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    void Update()
    {
        if(_isGaze)
        {
            //_anim.SetBool("isGaze", _isGaze);
            float time = (Time.time - _timeStart) / _durSpeed;
            transform.localScale = Vector3.one * Mathf.Lerp(_minSize, _maxSize, time);
            _cross.color = _gazeColor;
        }
        else
        {
            //_anim.SetBool("isGaze", _isGaze);
            transform.localScale = Vector3.one * _minSize;
            _cross.color = _originColor;
            _timeStart = Time.time;
        }
    }
}
