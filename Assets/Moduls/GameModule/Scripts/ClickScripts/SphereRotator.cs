using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SphereRotator : DraggingBase
{
    protected override void OnBeginDrag(Vector2 screenPosition)
    {
    }


    protected override void OnDrag(Vector2 angle)
    {
        Rotate(angle);
    }

    protected override void OnEndDrag(Vector2 screenPosition)
    {
    }
    
    private float AnglePower(float angleFirst, float angleSecond)
    {
        return angleFirst * angleFirst + angleSecond * angleSecond;
    }

    private void Rotate(Vector2 angle)
    {
        var sphere = ModuleController.Instance.SphereRoot;
        var sphereRot = sphere.transform.rotation;
        var angles = new List<float> { sphereRot.eulerAngles.x, sphereRot.eulerAngles.z };
        sphere.transform.Rotate(angle.x, 0, 0, Space.World);
        sphereRot = sphere.transform.rotation;
        angles.AddRange(new List<float> { sphereRot.eulerAngles.x, sphereRot.eulerAngles.z });
        angles = angles.Select(x => x > 180 ? 360 - x : x).ToList();
        if (AnglePower(angles[2], angles[3]) > 4000 && 
            AnglePower(angles[2], angles[3]) > AnglePower(angles[0], angles[1]))
            sphere.transform.Rotate(-angle.x, 0, 0, Space.World);
        sphere.transform.Rotate(0, angle.y, 0, Space.Self);
    }

}
