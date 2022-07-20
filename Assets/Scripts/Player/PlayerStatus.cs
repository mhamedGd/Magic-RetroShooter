using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour
{
    int health;
    int mana;

    public int maxHealth;
    public int GetHealth => health;
    public UnityEvent onHealthConsumed;
    public UnityEvent onHealthAdded;

    public int maxMana;
    public int GetMana => mana;
    public UnityEvent onManaConsumed;
    public UnityEvent onManaAdded;

    public void ConsumeHealth(int _value)
    {
        health -= _value;
        if (health <= 0)
            health = 0;
        onHealthConsumed?.Invoke();
    }

    public void ConsumeMana(int _value)
    {
        mana -= _value;
        if (mana<= 0)
            mana = 0;
        onManaConsumed?.Invoke();
    }

    public void AddHealth(int _value)
    {
        health += _value;
        if (health > maxHealth)
            health = maxHealth;
        onHealthAdded?.Invoke();
    }

    public void AddMana(int _value)
    {
        mana += _value;
        if (mana > maxMana)
            mana = maxMana;
        onManaAdded?.Invoke();
    }

    private void Start()
    {
        AddHealth(10);
        AddMana(maxMana);
    }

    float timerToRegenerateMana;
    [SerializeField] float manaRegenerationIterationSpeed;
    [SerializeField] int manaRegenerationAmount;
    private void Update()
    {
        if (mana < maxMana && !ObjectsDatabase.singleton.itemsInvoker.isActing)
            timerToRegenerateMana += Time.deltaTime;

        if(timerToRegenerateMana >= manaRegenerationIterationSpeed)
        {
            AddMana(manaRegenerationAmount);
            timerToRegenerateMana = 0.0f;
        }
    }
}
