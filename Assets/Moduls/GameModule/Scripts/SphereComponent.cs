using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereGenerator))]
public class SphereComponent : MonoBehaviour
{
    public IEnumerable<AbstractDetail> Details;

    public IEnumerable<AbstractDetail> Generate(int squeres, int devideNumbers, float radius, float thickness)
    {
        Details = GetComponent<SphereGenerator>().Generate(squeres, devideNumbers, thickness, radius);
        foreach (var detail in Details)
        {
            detail.gameObject.AddComponent<MeshCollider>();
            detail.transform.tag = transform.tag;
        }

        if (thickness == 0)
            transform.LookAt(ModuleController.Instance.SphereCamera.transform);
        
        return Details;
    }
}
