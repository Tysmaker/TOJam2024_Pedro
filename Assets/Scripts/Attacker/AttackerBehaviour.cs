using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AttackerBehaviour : MonoBehaviour, IDamageable
{
    public enum AttackerStates
    {
        Attacking,
        Moving,
        Dead
    }

    [SerializeField] private GameObject endZone;
    [SerializeField] private LayerMask towerLayer;

    private NavMeshAgent agent;
    private Rigidbody rb;
    private GameObject tower;
    private AttackerStats attackerStats;
    private TowerStats towerStats;
    AttackerStates attackerStates;
    private List<TowerStats> towersInRange = new List<TowerStats>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        endZone = GameObject.FindGameObjectWithTag("EndZone");
        attackerStats = GetComponent<AttackerStats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        attackerStates = AttackerStates.Moving;
        agent.destination = endZone.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckStates();
        CheckAttackRange();
        CheckHealth();
    }

    //Just temporary when enemy collides with tower it destroyed the tower
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Tower"))
    //     {
    //         Destroy(collision.gameObject);
    //     }
    // }

    void DetectTower()
    {
        towersInRange.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackerStats.detectionRange, towerLayer);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider)
            {
                var tower = hitCollider.GetComponent<TowerStats>();
                //agent.destination = hitCollider.transform.position;
                towersInRange.Add(tower);
            }
        }

        if (towersInRange.Count > 0)
        {
            SortTowers();
            tower = towersInRange[0].gameObject;

            if (tower != null)
            {
                agent.destination = tower.transform.position;
            }
        }
        else
        {
            agent.destination = endZone.transform.position;
            Debug.Log("Go Back to endzone");
        }
    }

    private void SortTowers()
    {
        // Sort towers by priority
        towersInRange.Sort((x, y) => x.GetPriority().CompareTo(y.GetPriority()));
    }


    void CheckStates()
    {
        switch (attackerStates)
        {
            case AttackerStates.Moving:
                agent.isStopped = false;
                DetectTower();
                Debug.Log("Attacker Is Moving");
                break;
            case AttackerStates.Attacking:
                agent.isStopped = true;
                AttackTower();
                break;
            case AttackerStates.Dead:
                agent.isStopped = true;
                Destroy(gameObject);
                Debug.Log("Attacker Is Dead");

                break;
        }
    }

    void CheckAttackRange()
    {
        if (tower != null)
        {
            float dist = Vector3.Distance(transform.position, tower.transform.position);

            if (dist <= attackerStats.AttackRange())
            {
                // Tower is within attack range
                attackerStates = AttackerStates.Attacking;

            }
            else
            {
                // Tower is out of attack range, continue moving
                attackerStates = AttackerStates.Moving;
            }
        }
        else
        {
            // No tower detected, continue moving
            attackerStates = AttackerStates.Moving;
        }
    }

    void CheckHealth()
    {
        if (attackerStats.GetHealth() <= 0)
        {
            attackerStates = AttackerStates.Dead;
        }
    }
    private void AttackTower()
    {
        if(tower != null)
        {
            print("Tower Missing");
            return;
        }

        IDamageable damageable = tower.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(attackerStats.GetAttackDamage());

            //Debug.Log(damageable);
        }
        else
        {
            Debug.Log("No Damageable Interface Found");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackerStats.detectionRange);
    }

    public void TakeDamage(int value)
    {
        attackerStats.SetHealth(value);

    }
}
