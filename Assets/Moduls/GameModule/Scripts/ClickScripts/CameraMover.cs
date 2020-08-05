using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraMover : DraggingBase
{
    
    private Vector2 borderFrom = new Vector2(-4.9f, -12.3f);
    private Vector2 borderTo = new Vector2(4.6f, -6.8f);
    protected override void OnBeginDrag(Vector2 angle)
    {
        
    }

    protected override void OnDrag(Vector2 angle)
    {
        Move(angle);
    }

    protected override void OnEndDrag(Vector2 angle)
    {
    }
    
    
    private void Move(Vector2 angle)
    {
        var camera = ModuleController.Instance.TableCamera;
        var angles = new Vector3( angle.y, 0, -angle.x );
        camera.transform.position += angles * 0.003f * (camera.transform.position.y + 1);
        if(camera.transform.localPosition.x < borderFrom.x  || camera.transform.localPosition.x > borderTo.x ||
           camera.transform.localPosition.z < borderFrom.y || camera.transform.localPosition.z > borderTo.y)
        {
            camera.transform.position -= angles * 0.003f * (camera.transform.position.y + 1);
            
        }
    }
}
