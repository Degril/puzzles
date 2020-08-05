using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DraggingBase : MonoBehaviour
{

    private float _slidingTime { get; set; } = 1;
    private float _rotateModifierSpeed { get; set; } = 0.5f;
    private Vector2 lastscreenPosition { get; set; } 
    

    public void BeginDrag(Vector2 screenPosition)
    {
        OnBeginDrag(screenPosition - lastscreenPosition);
        lastscreenPosition = screenPosition;
        
    }

    public void Drag(Vector2 screenPosition)
    {
        
        float firstTouchPrevPosX = screenPosition.x - lastscreenPosition.x;
        float rotateModifierX = (-firstTouchPrevPosX) * _rotateModifierSpeed;
        float firstTouchPrevPosY = screenPosition.y - lastscreenPosition.y;
        float rotateModifierY = (-firstTouchPrevPosY) * _rotateModifierSpeed;
        OnDrag(new Vector2(-rotateModifierY, rotateModifierX));
        
        lastscreenPosition = screenPosition;
    }

    public void EndDrag(Vector2 screenPosition)
    {
        StartCoroutine(Sliding(screenPosition));
        OnEndDrag(screenPosition);
        lastscreenPosition = screenPosition;
    }
    
    protected abstract void OnBeginDrag(Vector2 angle);
    protected abstract void OnDrag(Vector2 angle);
    protected abstract void OnEndDrag(Vector2 angle);
    
    
    private IEnumerator Sliding(Vector2 screenPosition)
    {
        var rotateAngle = new Vector2((screenPosition.y - lastscreenPosition.y),
            -(screenPosition.x  - lastscreenPosition.x))  * _rotateModifierSpeed;
        
        var currentTime = 0f;
        
        while(currentTime  <  _slidingTime)
        {
            currentTime += Time.deltaTime;
            rotateAngle *= 0.9f;
            OnDrag(rotateAngle);
            yield return new WaitForEndOfFrame();
        }
    }
    
    
}
