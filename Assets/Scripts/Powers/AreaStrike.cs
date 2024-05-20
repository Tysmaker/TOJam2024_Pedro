using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using CartoonFX;
using UnityEngine.EventSystems;

public class AreaStrike : MonoBehaviour
{
    [SerializeField] private float initialArea = 1;
    [SerializeField] private float areaGrowthRate = 1;
    [SerializeField] private float maxArea = 10;
    [SerializeField] private Transform areaCenter;
    [SerializeField] private float damage = 1;
    [SerializeField] private float damageGrowthRate = 1;
    [SerializeField] private float maxDamage = 10;
    [SerializeField] private float cooldown = 1;
    [SerializeField] private Slider cooldownSlider;

    private float currentArea;
    private float currentDamage;
    private float currentTime;

    private bool isActive = false;
    private bool isPreviewing = false;
    private bool isCooldown = false;

    // FXs
    [SerializeField] private ParticleSystem areaStrikeFX;
    [SerializeField] private GameObject areaPreview;

    private void Update()
    {
        // Check if not over UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && isPreviewing )
        {
            isActive = true;
        }

        if (Input.GetKey(KeyCode.Mouse0) && isActive)
        {
            Preview();
            currentArea += areaGrowthRate * Time.deltaTime;
            currentDamage += damageGrowthRate * Time.deltaTime;
            if (currentArea > maxArea)
            {
                currentArea = maxArea;
            }
            if (currentDamage > maxDamage)
            {
                currentDamage = maxDamage;
            }
            areaPreview.transform.localScale = new Vector3(currentArea, currentArea, currentArea);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && isActive)
        {
            areaStrikeFX.Play();
            var camShake = areaStrikeFX.GetComponent<CFXR_Effect>();
            DOVirtual.DelayedCall(areaStrikeFX.main.duration, DisablePreview);
            isActive = false;
            DoAreaStrike();
        }
    }

    private void Preview()
    {
        isPreviewing = true;
        areaPreview.SetActive(true);
        Vector3 position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
        {
            position = hit.point;
            position.y = 0.1f;
            areaPreview.transform.position = position;
        }
    }

    private void DisablePreview()
    {
        areaPreview.SetActive(false);
        isPreviewing = false;
        currentArea = initialArea;
        currentDamage = damage;
        areaPreview.transform.localScale = new Vector3(currentArea, currentArea, currentArea);
    }

    private void DoAreaStrike()
    {
        var colliders = Physics.OverlapSphere(areaCenter.position, currentArea, LayerMask.GetMask("Enemy"));
        foreach (var collider in colliders)
        {
            var attacker = collider.GetComponent<AttackerBehaviour>();
            if (attacker != null)
            {
                var roundedDamage = Mathf.RoundToInt(currentDamage);
                attacker.TakeDamage(roundedDamage);
            }
        }
        isCooldown = true;
        StartCoroutine(DoCooldown());
    }

    private IEnumerator DoCooldown()
    {
        currentTime = cooldown;
        cooldownSlider.DOValue(0, cooldown).SetEase(Ease.Linear);
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            yield return null;
        }
        isCooldown = false;
    }

    public void Activate()
    {
        cooldownSlider.value = 1;
        if (!isCooldown)
        {
            isPreviewing = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(areaCenter.position, currentArea);
    }
}
