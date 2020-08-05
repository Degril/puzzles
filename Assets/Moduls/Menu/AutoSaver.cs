using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaver : UtilMonoBehaviour
{
    [SerializeField] private float _saveCooldown = 5;

    public void AutoSaveStart()
    {
        StartCoroutine(AutoSave(_saveCooldown));
    }

    private IEnumerator AutoSave(float cooldownSeconds)
    {
        while (true)
        {
            Game.Get<Data>().Save();
            yield return  new WaitForSeconds(cooldownSeconds);
        }
    } 
}
