using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName ="Enemy")]
public class Enemy : ScriptableObject
{
    public string _name;
    public float _defense;
    public float _attack;
    public float _health;
    public Sprite _sprite;
}
