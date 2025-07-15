using UnityEngine;
using System;

namespace CF.Data {

[Serializable]
public class BulletData
{
    public float Speed;
    public float Size;
    public float Damage;
    public Sprite Sprite;
    public Vector2 MovementVector;
    public float delayTime; 
    [HideInInspector]
    public bool FromEnemy;
    public Color GlowColor;

    public BulletData(float _speed, float _size, float _damage,Sprite _sprite, Vector2 _movementVector, bool _fromEnemy, Color _glowColor)
    {
        Speed = _speed;
        Size = _size;
        Damage = _damage;
        Sprite = _sprite;
        MovementVector = _movementVector;
        FromEnemy = _fromEnemy;
        GlowColor = _glowColor;
    }
}
}
