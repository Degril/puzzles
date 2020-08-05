using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutine : UtilMonoBehaviour
{
    private IEnumerator AnimationCoroutine(float duration, Action<float, bool> action)
    {
        float currentTime = 0;
        while(currentTime < duration)
        {
            currentTime += Time.deltaTime;
            action?.Invoke(currentTime,true);
            yield return new WaitForEndOfFrame();
        }
        action.Invoke(duration,false);
    }

    public void Animate(float duration, Action<float, bool> action)
    {
        StartCoroutine(AnimationCoroutine(duration, action));
    }

}
