using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static Assets.Scripts.Utils.TweenUtils;

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
    [SerializeField]
    AttackerStates attackerStates;
    [SerializeField]
    private List<TowerStats> towersInRange = new List<TowerStats>();
    private Animator animator;
    [SerializeField]
    private bool hasRagdoll = false;
    private Rigidbody[] ragdollRigidbodies;

    private void Awake()
    {
        if (hasRagdoll)
        {
            ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (var rb in ragdollRigidbodies)
            {
                rb.isKinematic = true;
            }
        }
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        attackerStats = GetComponent<AttackerStats>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        attackerStates = AttackerStates.Moving;
        agent.destination = endZone.transform.position;
        Delay(attackerStats.GetAttackSpeed(), AttackTower, -1, LoopType.Restart);
    }

    // Update is called once per frame
    void Update()
    {
        if (attackerStates == AttackerStates.Dead)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            StopAllCoroutines();

            if (hasRagdoll)
            {
                foreach (var rb in ragdollRigidbodies)
                {
                    rb.isKinematic = false;
                }
            }
            gameObject.SetActive(false);
            Destroy(gameObject, 10f);
            return;
        }
        CheckForTowers();
        CheckStates();
        CheckAttackRange();
    }

    //Just temporary when enemy collides with tower it destroyed the tower
    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Tower"))
    //     {
    //         Destroy(collision.gameObject);
    //     }
    // }

    void CheckForTowers()
    {
        towersInRange.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackerStats.detectionRange, towerLayer);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider)
            {
                var tower = hitCollider.GetComponentInParent<TowerStats>();
                //agent.destination = hitCollider.transform.position;
                if (tower != null)
                    towersInRange.Add(tower);
                else
                    Debug.Log("How could this happen to me");
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
                animator.SetBool("isMoving", true);
                break;
            case AttackerStates.Attacking:
                agent.isStopped = true;
                animator.SetBool("isMoving", false);
                animator.SetTrigger("attack");
                break;
        }
    }

    void CheckAttackRange()
    {
        if (tower != null)
        {
            float dist = Vector3.Distance(transform.position, tower.transform.position);

            if (dist <= attackerStats.GetAttackRange())
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


    private void AttackTower()
    {
        if (tower == null)
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

        if (attackerStats.GetHealth() <= 0)
        {
            attackerStates = AttackerStates.Dead;
        }

    }

    public void SetEndZone(GameObject endZone)
    {
        this.endZone = endZone;
    }

    public AttackerStates GetAttackerState()
    {
        return attackerStates;
    }
}
