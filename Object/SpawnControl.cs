using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] float _createDelayTime = 5.0f;
    [SerializeField] int _maxLiveCount = 30;
    [SerializeField] int _limitCreateCount = 30;
    [SerializeField] PublicDefine.eRoamingType _roamingType;
    [SerializeField] PublicDefine.eLocation _locationType;

    GameObject _zombie1;
    GameObject _zombie2;

    List<GameObject> _ltEnemys;

    float _createTime;

    LightManager _lightManager;
    void Awake()
    {
        _zombie1 = Resources.Load("Zombie/Zombie1") as GameObject;
        _zombie2 = Resources.Load("Zombie/Zombie2") as GameObject;
        _lightManager = GameObject.Find("Directional Light").GetComponent<LightManager>();

        _ltEnemys = new List<GameObject>();

        _createTime = 0;
    }

    void Update()
    {
        //if (_lightManager._isNight)
        //{
        //    CreateZombies();
        //}

        if (_locationType == PublicDefine.eLocation.InDoor)
            CreateZombies();
        else
        {
            if (_lightManager._isNight)
                CreateZombies();
            else
            {
                if(_ltEnemys.Count > 0)
                {
                    for(int n=0; n<_ltEnemys.Count; n++)
                    {
                        Destroy(_ltEnemys[n]);
                        _ltEnemys.RemoveAt(n);
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        for(int n=0; n<_ltEnemys.Count; n++)
        {
            if(_ltEnemys[n] == null)
            {
                _ltEnemys.RemoveAt(n);
                break;
            }
        }
    }

    GameObject RandomZombie()
    {
        int random = Random.Range(1, 3);
        GameObject enemy;
        if (random == 1)
            enemy = _zombie1;
        else
            enemy = _zombie2;
        return enemy;
    }
   
    void CreateZombies()
    {
        if (_limitCreateCount > 0)
        {
            if (_maxLiveCount >= _ltEnemys.Count)
            {
                _createTime += Time.deltaTime;
                if (_createTime >= _createDelayTime)
                {
                    int random = Random.Range(1, 3);
                    GameObject go = Instantiate(RandomZombie(), transform.position, transform.rotation);
                    //GameObject go = Instantiate(_zombie1, transform.position, transform.rotation);
                    //GameObject go = Instantiate(_zombie2, transform.position, transform.rotation);
                    ZombieController zoc = go.GetComponent<ZombieController>();
                    zoc.InitSetEnemy(this, transform, _roamingType);
                    _ltEnemys.Add(go);
                    _createTime = 0;
                    _limitCreateCount--;
                }
            }
        }
    }
}
