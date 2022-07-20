using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item")]
public class Item : ScriptableObject
{
    public string name { get { return _name; } private set { _name = value; } }
    [SerializeField] string _name;

    public Sprite sprite { get { return _sprite; } private set { _sprite = value; } }
    [SerializeField] Sprite _sprite;

    public bool stackable { get { return _stackable; } private set { _stackable = value; } }
    [SerializeField] bool _stackable;

    public int maxStock { get { return _maxStock; } private set { _maxStock = value; } }
    [SerializeField] int _maxStock;

    public string description { get { return _description; } private set { _description = value; } }
    [SerializeField, TextArea] string _description;

    public GameObject prefab { get { return _prefab; } private set { _prefab = value; } }
    [SerializeField] GameObject _prefab;
    public GameObject pickup { get { return _pickup; } private set { _pickup = value; } }
    [SerializeField] GameObject _pickup;


}
