using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour, IPlaceable
{
    public Material DefaultMaterial { get => defaultMaterial; set => defaultMaterial = value; }
    public Renderer ObjectRenderer { get => objectRenderer; set => objectRenderer = value; }
    public float ObjectRadius { get => radius; set => radius = value; }
    public Collider ObjectCollider { get => objectCollider; set => objectCollider = value; }    
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private Renderer objectRenderer;
    [SerializeField, Range(0.1f, 10f)]
    private float radius;
    [SerializeField]
    private Collider objectCollider;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}