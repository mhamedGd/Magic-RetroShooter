using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField] Image chargingBarImage;
    float chargingBar;

    [SerializeField] Image healthImage;
    [SerializeField] Image manaImage;

    public void SetChargingBar(float _value)
    {
        chargingBar = _value;
        chargingBarImage.fillAmount = chargingBar;
    }

    public void SynchHealthUIWithValue()
    {
        healthImage.fillAmount = (float) ObjectsDatabase.singleton.playerStatus.GetHealth / (float) ObjectsDatabase.singleton.playerStatus.maxHealth;
    }

    public void SynchManaUIWithValue()
    {
        manaImage.fillAmount = (float)ObjectsDatabase.singleton.playerStatus.GetMana / (float)ObjectsDatabase.singleton.playerStatus.maxMana;
    }

    private void Start()
    {
        ObjectsDatabase.singleton.playerStatus.onHealthConsumed.AddListener(SynchHealthUIWithValue);
        ObjectsDatabase.singleton.playerStatus.onHealthAdded.AddListener(SynchHealthUIWithValue);

        ObjectsDatabase.singleton.playerStatus.onManaConsumed.AddListener(SynchManaUIWithValue);
        ObjectsDatabase.singleton.playerStatus.onManaAdded.AddListener(SynchManaUIWithValue);
    }
}
