using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AttackerBehaviour : MonoBehaviour
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
    AttackerStates attackerStates;

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
        CheckPlayerHealth();
    }

    //Just temporary when enemy collides with tower it destroyed the tower
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tower"))
        {
            Destroy(collision.gameObject);
        }
    }

    void DetectTower()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackerStats.detectionRange, towerLayer);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider)
            {
                tower = hitCollider.gameObject;
                agent.destination = hitCollider.transform.position;
               
            }
        }

        //Checking for is the tower is destroyed then moves back to the endZones Position
        if (tower.IsDestroyed())
        {
            agent.destination = endZone.transform.position;
        }
    }

    void CheckStates()
    { 
        switch(attackerStates)
        {
            case AttackerStates.Moving:
                agent.isStopped = false;
                DetectTower();
                Debug.Log("Attacker Is Moving");
                break;
            case AttackerStates.Attacking:
                agent.isStopped = true;
                if(agent.isStopped)
                {
                    Debug.Log("Attacker Doing Dmg");
                }
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

            if (dist <= attackerStats.attackRange)
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

    void CheckPlayerHealth()
    {
        if (attackerStats.health <= 0)
        {
            attackerStates = AttackerStates.Dead;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackerStats.detectionRange);
    }
}
