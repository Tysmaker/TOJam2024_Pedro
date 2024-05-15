using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static Assets.Scripts.Utils.TweenUtils;

public class TowerBehavior : MonoBehaviour, IPlaceable, IDamageable
{
    public Material DefaultMaterial { get => defaultMaterial; set => defaultMaterial = value; }
    public Renderer ObjectRenderer { get => objectRenderer; set => objectRenderer = value; }
    public float ObjectRadius { get => radius; set => radius = value; }
    public Collider ObjectCollider { get => objectCollider; set => objectCollider = value; }
    public GameObject ObjectPreview { get => objectPreview; set => objectPreview = value; }
    
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Renderer objectRenderer;
    [SerializeField, Range(0.1f, 10f)]
    private float radius;
    [SerializeField]
    private Collider objectCollider;
    [SerializeField]
    private GameObject objectPreview;
    private TowerStats towerStats;
    //private AttackerStats attackerStats;

    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private GameObject towerRangeLine;


    private readonly Collider[] _enemyColliders = new Collider[10];
    private List<AttackerStats> _orderedEnemiesInRange = new List<AttackerStats>();
    private AttackerStats enemyInRange;

    // FXs
    [SerializeField]
    private GameObject deathFX;

    private void Awake()
    {
        towerStats = GetComponent<TowerStats>();
    }

    private void Start()
    {
        Delay(towerStats.GetAttackSpeed(), DamageEnemy, -1, LoopType.Restart);
    }

    public void SetPersistent()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void TakeDamage(int damage)
    {
        towerStats.SetHealth(damage);

        if(towerStats.GetHealth() <= 0 ) 
        { 
            OnDeath();
        }
    }

    private void DamageEnemy()
    {
        if (enemyInRange == null) return;

        var damageable = enemyInRange.GetComponent<IDamageable>();

        // Damage enemyInRange
        damageable.TakeDamage(towerStats.GetAttackDamage());

    }

    private void Update()
    {
        CheckForEnemiesInRange();
        UpdateTowerRangeLine();

        if (enemyInRange == null) return;
        //LookAt(transform, enemyInRange.transform, 0, ease: Ease.Linear);
    }

    private void CheckForEnemiesInRange()
    {
        _orderedEnemiesInRange.Clear();
        enemyInRange = null;

        int enemiesInRange = Physics.OverlapSphereNonAlloc(transform.position, towerStats.GetAttackRange(), _enemyColliders, enemyLayer);

        if (enemiesInRange == 0) return;

        foreach (var enemyCollider in _enemyColliders)
        {
            if (enemyCollider == null) continue;

            var enemyState = enemyCollider.GetComponent<AttackerBehaviour>().GetAttackerState();
            if (enemyState == AttackerBehaviour.AttackerStates.Dead) continue;

            var enemy = enemyCollider.GetComponent<AttackerStats>();
            if (enemy == null) continue;

            if (!_orderedEnemiesInRange.Contains(enemy))
            {
                _orderedEnemiesInRange.Add(enemy);
            }
        }
        OrderEnemiesInRange();
    }

    private void UpdateTowerRangeLine()
    {
        float xScale = towerStats.GetAttackRange() / 5;
        float zScale = towerStats.GetAttackRange() / 5;

        towerRangeLine.transform.localScale = new Vector3(xScale,0, zScale);
    }

    private void OnDeath()
    {
        if (deathFX != null)
        {
            var positionOffset = transform.position + new Vector3(0,0.5f, 0);
            Instantiate(deathFX, positionOffset, Quaternion.AngleAxis(-90, Vector3.right));
        }
        Destroy(gameObject);
    }

    private void OrderEnemiesInRange()
    {
        if (_orderedEnemiesInRange == null || _orderedEnemiesInRange.Count == 0)
        {
            Debug.Log("No enemies in range");
            return;
        }

        _orderedEnemiesInRange.Sort((enemy1, enemy2) => Vector3.Distance(transform.position, enemy1.transform.position).CompareTo(Vector3.Distance(transform.position, enemy2.transform.position)));
        enemyInRange = _orderedEnemiesInRange[0];
    }

}
