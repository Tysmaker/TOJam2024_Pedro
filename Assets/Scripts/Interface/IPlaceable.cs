using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPlaceable
{
    Material DefaultMaterial { get; set; }
    Renderer ObjectRenderer { get; set; }
    float ObjectRadius { get; set; }
    Collider ObjectCollider { get; set; }
}
