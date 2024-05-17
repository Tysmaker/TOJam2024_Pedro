using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerManager : MonoBehaviour
{
    private List<AttackerBehaviour> activeEnemies = new List<AttackerBehaviour>();
    private List<GameObject> spawnQueue = new List<GameObject>();
    [SerializeField]
    private Transform spawnArea;
    [SerializeField]
    private Transform targetArea;
    [SerializeField]
    private List<GameObject> enemyPrefabs = new List<GameObject>();

    private bool isGameOver = false;

    // Events
    public event System.Action<int> OnWaveNumberChanged;

    // Singleton
    public static AttackerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
