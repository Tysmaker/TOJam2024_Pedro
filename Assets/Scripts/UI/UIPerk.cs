using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPerk : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI perkName;
    [SerializeField] private TextMeshProUGUI perkDescription;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button selectButton;

    public void UpdateUI(string perkName, string perkDescription, int cost)
    {
        this.perkName.text = perkName;
        this.perkDescription.text = perkDescription;
        this.costText.text = cost.ToString() + " Added to tower";
    }
}
