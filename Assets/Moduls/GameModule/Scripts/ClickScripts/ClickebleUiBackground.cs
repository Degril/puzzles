using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickebleUiBackground : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ClickSystem _clickSystem;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("click on background down");
        _clickSystem._clickOnBackGround = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Game.Get<Coroutine>().Animate(0.01f, (time, isAnimate) =>
        {
            if (isAnimate == false)
            {
                _clickSystem._clickOnBackGround = false;
                Debug.Log("click on background up");
            }
        });
    }
}
