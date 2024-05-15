using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    [SerializeField]
    private int MaxHealth = 100;
    private int currentHealth;
    [SerializeField]
    private GameObject gateArea;
    [SerializeField]
    private float gateArearadius;

    // FXs

    [SerializeField]
    private ParticleSystem takeDamageFX;
    [SerializeField]
    private GameObject healthBelow50FX;
    [SerializeField]
    private GameObject healthBelow25FX;
    [SerializeField]
    private GameObject healthBelow10FX;
    [SerializeField]
    private GameObject deathFX;

    // Events

    public static event System.Action<int> OnHealthChanged;
    public static event System.Action OnDeath;

    private void Awake()
    {
        currentHealth = MaxHealth;
    }

    private void Update()
    {
        var colliders = Physics.OverlapSphere(gateArea.transform.position, gateArearadius, LayerMask.GetMask("Enemy"));
        foreach (var collider in colliders)
        {
            var attacker = collider.GetComponent<AttackerBehaviour>();

            if (attacker != null)
            {
                TakeDamage();
                attacker.GotToEndZone();
            }
        }
    }

    public void TakeDamage()
    {
        Debug.Log("Base taking damage");
        currentHealth -= 1;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (currentHealth <= MaxHealth / 2)
            {
                if (currentHealth <= MaxHealth / 4)
                {
                    if (currentHealth <= MaxHealth / 10)
                    {
                        healthBelow10FX.SetActive(true);
                    }
                    else
                    {
                        healthBelow25FX.SetActive(true);
                    }
                }
                else
                {
                    healthBelow50FX.SetActive(true);
                }
            }
            if (takeDamageFX.gameObject.activeSelf == false) 
            {
                takeDamageFX.gameObject.SetActive(true);
            }
            takeDamageFX.Play();
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    public int GetMaxHealth()
    {
        return MaxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private void Die()
    {
        deathFX.SetActive(true);
        OnDeath?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gateArea.transform.position, gateArearadius);
    }
}
