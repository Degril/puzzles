using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetailComponent : AbstractDetail
{
    public bool IsFixed { get;private set; } = false;
    public bool OnTable { get; private set; } = false;
    
    private IEnumerable<AbstractDetail> _writeNeigborDetails;

    public void Init(IEnumerable<AbstractDetail> details)
    {
        _writeNeigborDetails = details.OrderBy((detail) => Vector3.Distance(detail.transform.position, transform.position)).Take(4);
    }

    public bool CheckToAssemble(IEnumerable<AbstractDetail> details)
    {
        var currentNeighborDetails = details.OrderBy((detail) => Vector3.Distance(detail.transform.position, transform.position)).Take(4);
        return currentNeighborDetails.All(neighborDetail => _writeNeigborDetails.Contains(neighborDetail) == true);
    }

    public void Fix()
    {
        IsFixed = true;
        gameObject.layer = 8;
    }

    public override void OnClick(float touchTime)
    {
        if (OnTable == true)
        {
            ModuleController.Instance.SelectedDetail = this;
            ModuleController.Instance.GamoMode = GameMode.Detail;
        }
        else
        {
            if (IsFixed) return;
            
            if(touchTime < 0.5f)
                transform.parent.eulerAngles += new Vector3(0, 0, 90);
            else
                MoveToTable();
        }
    }

    public void MoveToSphere(SlotComponent slot)
    {
        OnTable = false;
        var slotTransform = slot.transform;
        var selectedDetailParent = transform.parent;
        selectedDetailParent.transform.parent = ModuleController.Instance.SphereDetails.transform;
        
        ModuleController.Instance.SetDetailPositionOnSphere(slot,this);
        var mini = 0f;
        var minDist = float.MaxValue;
        var selectedDetailVerts = Angles;
        var slotDetailVerts = slot.Angles;
        
        for (float i = 0; i < 360; i+=0.5f)
        {
            var dist = selectedDetailVerts.Sum(selecteDetailVert =>
                slotDetailVerts.Min(slotDetailVert => 
                    Vector3.Distance(slotTransform.TransformPoint(slotDetailVert),
                        transform.TransformPoint(selecteDetailVert))));
            
            if(dist < minDist)
            {
                minDist = dist;
                mini = i;
            }

            var eulerAngles = selectedDetailParent.eulerAngles;
            selectedDetailParent.eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y,i);
        }
        
        selectedDetailParent.rotation = slotTransform.parent.transform.rotation;
        var angles = selectedDetailParent.eulerAngles;
        angles =new Vector3(angles.x,angles.y,mini);
        selectedDetailParent.eulerAngles = angles;
    }

    public void MoveToTable()
    {
        OnTable = true;
        var tableTransform = ModuleController.Instance.Table.transform;
        var fromRayPosition = tableTransform.position + Vector3.up * 100;
        var tableScale = tableTransform.localScale;
        var toRayPosition = new Vector3(Random.Range(-tableScale.x, tableScale.x) / 2.2f,
            -100,
            Random.Range(-tableScale.z, tableScale.z) / 2.2f);
        var ray = new Ray(fromRayPosition, toRayPosition);
        Debug.DrawRay(fromRayPosition, toRayPosition, Color.white, 100f);
        var tableTag = tableTransform.tag;
        var hits = Physics.RaycastAll(ray);
        var detailParent = transform.parent;
        if (hits == null || hits.Length <= 0) return;
        if (hits.Any(x => x.transform.tag == tableTag))
        {
            var hit = hits.FirstOrDefault(x => x.transform.tag == tableTag);
            detailParent.position = hit.point;
            detailParent.LookAt(transform.position + Vector3.up);
            detailParent.localScale = Vector3.one;
            detailParent.parent = hit.transform.parent;
        }
    }
}
