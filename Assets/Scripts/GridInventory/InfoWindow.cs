using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    public TextMeshProUGUI Label => label;
    [SerializeField] TextMeshProUGUI description;
    public TextMeshProUGUI Descrition => description;

}
