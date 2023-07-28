using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] float _secondPerRealTime;
    [SerializeField] float _fogDensityCalc = 0.5f;
    [SerializeField] float _nightFogDensity = 0.2f;

    int _judgeDayCount;
    int _day;
    int _nightCount;
    int _mornCount;

    public bool _isNight;
    float _rotateDegree;

    bool _isOneHour;
    bool _isMorning;

    float _dayFogDensity;
    float _currentFogDensity;

    Light _light;

    public bool IsOneHour
    {
        get { return _isOneHour; }
        set { _isOneHour = value; }
    }

    public int GetDay
    {
        get { return _day; }
    }

    public bool IsMorning
    {
        get { return _isMorning; }
    }

    public int DayisGone
    {
        get { return _judgeDayCount; }
    }

    void Awake()
    {
        _light = GetComponent<Light>();

        _nightCount = 1;
        _mornCount = 1;
    }

    void Start()
    {
        _dayFogDensity = RenderSettings.fogDensity;
    }

    void Update()
    {
        _rotateDegree = 0.1f * _secondPerRealTime * Time.deltaTime;
        transform.Rotate(Vector3.right, _rotateDegree);
        if (transform.eulerAngles.x >= 180 || transform.eulerAngles.x < 0)
        {
            //if(transform.eulerAngles.x >= 190)
            //    RenderSettings.fog = false;
            _isNight = true;
            _judgeDayCount++;
        }
        else if(transform.eulerAngles.x >= 0 && transform.eulerAngles.x < 180)
        {
            _isNight = false;
            _judgeDayCount = 0;
            //RenderSettings.fog = true;
        }

        if(_isNight)
        {
            _mornCount = 1;
            _isMorning = false;
            if(_currentFogDensity <= _nightFogDensity)
            {
                _currentFogDensity += 0.1f * _fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = _currentFogDensity;
            }
            else
            {
                if (!RenderSettings.fog)
                    RenderSettings.fog = true;
            }
            if(transform.eulerAngles.x <= 360 - (15.0f * _nightCount) && transform.eulerAngles.x > 0)
            {
                _isOneHour = true;
                _nightCount++;
            }
        }

        else
        {
            _nightCount = 1;
            _isMorning = true;
            if (_currentFogDensity >= _dayFogDensity)
            {
                _currentFogDensity -= 0.1f * _fogDensityCalc * Time.deltaTime;
                RenderSettings.fogDensity = _currentFogDensity;
            }
            else
            {
                if (RenderSettings.fog)
                    RenderSettings.fog = false;
            }
            if (transform.eulerAngles.x >= 15.0f * _mornCount)
            {
                _isOneHour = true;
                _mornCount++;
            }
        }

        if(_judgeDayCount == 1)
        {
            // 나중에 언제 탈출했는지 day로 판단해서 UserInfo._record로 저장
            _day++;
        }
    }

    public void IsLight(bool isOn)
    {
        if (isOn)
            _light.enabled = true;
        else
            _light.enabled = false;
    }
}
