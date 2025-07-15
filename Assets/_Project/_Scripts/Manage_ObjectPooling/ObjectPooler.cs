using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using CF.Environment;
using CF.Controller;
using CF.Data;

namespace CF {
public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Current { get { return _current; } }
    public static ObjectPooler _current;

    public GameObject BulletPrefab;

    private List<GameObject> BulletPool;

    private List<GameObject> SpecialProjectilePool;

    private List<GameObject> SpaceTrashPool;

    private void Awake()
    {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _current = this;
        }

        BulletPool = new List<GameObject>();
        SpecialProjectilePool = new List<GameObject>();
        SpaceTrashPool = new List<GameObject>();
    }

    private void Start()
    {
        GameEvents.Current.onBulletOverEdge += RegisterBullet;
        GameEvents.Current.onSpecialOverEdge += RegisterSpecial;
        GameEvents.Current.onSpaceTrashOverEdge += RegisterSpaceTrash;
    }

    public void InstantiateBullet(BulletData _bulletData, Vector3 _pos)
    {
        if (BulletPool.Count == 0) // Create New Bullet
        {
            var bullet = Instantiate(BulletPrefab, _pos, Quaternion.identity);
            var controller = bullet.GetComponent<BulletController>();
            controller.UpdateSettings(_bulletData);
        }
        else if (BulletPool.Count > 0)
        {
            var bullet = BulletPool[0];
            bullet.transform.position = _pos;
            DeregisterBullet(bullet);
            var controller = bullet.GetComponent<BulletController>();
            controller.UpdateSettings(_bulletData);
            bullet.SetActive(true);
        }
    }

    public void InstantiateBulletsDelayed(BulletData[] _bullets, Transform _origin, bool fromEnemy=false){
        StartCoroutine(ExecuteDelayedBulletSpawn(_bullets, _origin, fromEnemy));
    }

    private IEnumerator ExecuteDelayedBulletSpawn(BulletData[] _bullets, Transform _origin, bool fromEnemy=false) {
        foreach (BulletData bullet in _bullets)
        {
            bullet.FromEnemy = fromEnemy;
            yield return new WaitForSeconds(bullet.delayTime);
            InstantiateBullet(bullet, _origin.position);
        }
    }

    public void InstantiateSpecificPrefab(GameObject _obj, Vector3 _pos)
    {
        if (SpecialProjectilePool.Count == 0) // This System doesn't support dynamic specials
        {
            var obj = Instantiate(_obj, _pos, Quaternion.identity);
        }
        else if (SpecialProjectilePool.Count > 0)
        {
            GameObject special = SpecialProjectilePool[0];
            DeregisterSpecial(special);
            special.transform.position = _pos;
            special.SetActive(true);
        }
    }

    public void InstantiateSpaceTrash(GameObject _obj, Vector3 _pos, float size, Sprite sprite, float speed)
    {
        if (SpaceTrashPool.Count == 0) 
        {
            GameObject obj = Instantiate(_obj, _pos, Quaternion.identity);
            obj.transform.localScale = Vector3.one * size;
            obj.GetComponent<SpriteRenderer>().sprite = sprite;
            var controller = obj.GetComponent<SpaceTrashController>();
            controller.speed = speed;
            controller.UpdateVelocity();
        }
        else if (SpaceTrashPool.Count > 0)
        {
            GameObject trash = SpaceTrashPool[0];
            DeregisterSpaceTrash(trash);
            trash.transform.position = _pos;
            transform.localScale = Vector3.one * size;
            trash.GetComponent<SpaceTrashController>().speed = speed;
            trash.SetActive(true);
        }
    }

    public void RegisterBullet(GameObject _obj) 
    {
        _obj.SetActive(false);
        BulletPool.Add(_obj);
    }

    private void DeregisterBullet(GameObject _obj)
    {
        BulletPool.Remove(_obj);
    }

    public void RegisterSpecial(GameObject _obj)
    {
        _obj.SetActive(false);
        SpecialProjectilePool.Add(_obj);
    }

    private void DeregisterSpecial(GameObject _obj)
    {
        SpecialProjectilePool.Remove(_obj);
    }
    public void RegisterSpaceTrash(GameObject _obj)
    {
        _obj.SetActive(false);
        SpaceTrashPool.Add(_obj);
    }

    private void DeregisterSpaceTrash(GameObject _obj)
    {
        SpaceTrashPool.Remove(_obj);
    }
}
}

