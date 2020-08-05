using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotComponent : AbstractDetail
{
    private DetailComponent _detailOnSlot;
    public override void OnClick(float touchTime)
    {
        if (ModuleController.Instance.SelectedDetail == null)
        {
            ModuleController.Instance.GamoMode = GameMode.Sphere;
        }
        else
        {
            ModuleController.Instance.SelectedDetail.MoveToSphere(this);
            ModuleController.Instance.SelectedDetail = null;
        }
    }
}
