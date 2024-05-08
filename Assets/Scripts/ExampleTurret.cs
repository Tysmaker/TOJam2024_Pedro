using static Assets.Scripts.Utils.TweenUtils;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public class ExampleTurret : MonoBehaviour
{
    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private float interactionRadius = 10f;
    [SerializeField]
    private float attackRate = 1f;
    [SerializeField]
    private int damage = 10;

    private readonly Collider[] _enemyColliders = new Collider[10];
    private List<ExampleEnemy> _orderedEnemiesInRange = new List<ExampleEnemy>();
    private ExampleEnemy enemyInRange;

    private void Start()
    {
        Delay(attackRate, DamageEnemy, -1, LoopType.Restart);
    }
    private void Update()
    {
        CheckForEnemiesInRange();

        if (enemyInRange == null) return;
        LookAt(transform, enemyInRange.transform, 0, ease: Ease.Linear);
    }
    private void CheckForEnemiesInRange()
    {
        _orderedEnemiesInRange.Clear();
        enemyInRange = null;

        int enemiesInRange = Physics.OverlapSphereNonAlloc(transform.position, interactionRadius, _enemyColliders, enemyLayer);

        if (enemiesInRange == 0) return;

        foreach (var enemyCollider in _enemyColliders)
        {
            if (enemyCollider == null) continue;

            var enemy = enemyCollider.GetComponent<ExampleEnemy>();
            if (enemy == null) continue;

            if (!_orderedEnemiesInRange.Contains(enemy))
            {
                _orderedEnemiesInRange.Add(enemy);
            }
        }
        Debug.Log(_orderedEnemiesInRange.Count + " enemies in range");
        OrderEnemiesInRange();
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

    private void DamageEnemy()
    {
        if (enemyInRange == null) return;

        // Damage enemyInRange
        enemyInRange.TakeDamage(damage);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
