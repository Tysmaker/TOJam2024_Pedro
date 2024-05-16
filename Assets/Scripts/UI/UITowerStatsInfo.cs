using TMPro;
using UnityEngine;


public class UITowerStatsInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statsName;
    [SerializeField]
    private TextMeshProUGUI stats;

    public void SetInfo(string name, string stats)
    {
        statsName.text = name;
        this.stats.text = stats;
    }

    public void SetTextColour(Color colour)
    {
        statsName.color = colour;
        stats.color = colour;
    }
}
