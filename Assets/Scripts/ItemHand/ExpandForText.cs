using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpandForText : MonoBehaviour
{
    [SerializeField] RectTransform label;
    [SerializeField] RectTransform description;
    [SerializeField] float padding;
    // Update is called once per frame
    public void Expand()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, label.sizeDelta.y + description.sizeDelta.y + padding);
    }

    private void Update()
    {
        Expand();
    }
}
