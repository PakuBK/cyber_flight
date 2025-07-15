using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CF.Particles {
public class ParticleManager : MonoBehaviour
{
    public static ParticleManager current { get { return _current; } }

    public static ParticleManager _current;

    [SerializeField]
    private Particle[] particles;

    private Hashtable originToPrefab;

    private Dictionary<ParticleOrigin, List<GameObject>> originToObjects;

    [SerializeField]
    private bool debugLog;

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

        RegisterParticles();
    }

    [Serializable]
    public class Particle
    {
        public GameObject ParticlePrefab;
        public ParticleOrigin origin;
    }

    private void RegisterParticles()
    {
        originToPrefab = new Hashtable();
        originToObjects = new Dictionary<ParticleOrigin, List<GameObject>>();

        foreach (ParticleOrigin type in Enum.GetValues(typeof(ParticleOrigin))) // Register each type and instantiate list
        {
            originToObjects.Add(type, new List<GameObject>());
        }

        for (int i = 0; i < particles.Length; i++) // Register each used Particle with its prefab
        {
            var obj = Instantiate(particles[i].ParticlePrefab, transform.position, Quaternion.identity, transform);

            Log($"Instantiate Object {particles[i].origin}");

            originToObjects[particles[i].origin].Add(obj); // Add 1 Object to the originToObject List

            originToPrefab.Add(particles[i].origin, particles[i].ParticlePrefab);
        }
    }

    public void SpawnVFX(ParticleOrigin _origin, Vector3 _pos, Transform _parent = null)
    {
        SpawnParticle(_origin, _pos, _parent);
    }

    public ParticleSystem SpawnBulletTrail(ParticleOrigin _origin, Vector3 _pos, Transform _parent = null)
    {
        return SpawnParticle(_origin, _pos, _parent);
    }

    private ParticleSystem SpawnParticle(ParticleOrigin _origin, Vector3 _pos, Transform _parent = null)
    {
        ParticleSystem system = GetParticle(_origin);

        if (_parent != null)
        {
            system.transform.parent = _parent;
        }
        else
        {
            system.transform.parent = transform;
        }
        
        if (system == null)
        {
            LogWarning("You didnt't assign a Prefab to this Origin");
            return null;
        }
        
        system.transform.position = _pos;
        system.transform.localScale = Vector3.one;

        system.Play();

        return system;
    }

    private ParticleSystem GetParticle(ParticleOrigin _origin)
    {
        foreach (var obj in originToObjects[_origin])
        {
            ParticleSystem system = obj.GetComponent<ParticleSystem>();
            if (!system.isPlaying)
            {
                return system;
            }
        }

        var prefab = originToPrefab[_origin] as GameObject;

        if (prefab == null)
        {
            return null;
        }

        var added_obj = Instantiate(prefab, transform.position, Quaternion.identity, transform);

        originToObjects[_origin].Add(added_obj);
        return added_obj.GetComponent<ParticleSystem>();
    }

    private void Log(string _message)
    {
        if (!debugLog) return;
        Debug.Log("[ParticleManager] " + _message);
    }

    private void LogWarning(string _message)
    {
        Debug.Log("[ParticleManager] " + _message);
    }
}
}
