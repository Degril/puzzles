using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableComponent : ClickBehaviour
{
    public override void OnClick(float touchTime)
    {
        ModuleController.Instance.GamoMode = GameMode.Table;
    }
}
