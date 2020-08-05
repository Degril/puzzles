using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereRotator))]
[RequireComponent(typeof(CameraMover))]
public class ClickSystem : MonoBehaviour
{
    public bool _clickOnBackGround { get; set; }= false;
    private DraggingBase _sphereRotator;
    private DraggingBase _cameraMover;
    private bool _isDragging = false;

    private void Awake()
    {
        _sphereRotator = GetComponent<SphereRotator>();
        _cameraMover = GetComponent<CameraMover>();
    }
    private Vector2 _lastScreenPosition;
    private Vector2 _screenPosition
    {
        get
        {
            if (Input.touchCount == 1)
                return Input.GetTouch(0).position;
            return Input.mousePosition;
        }
    }

    private IEnumerable<RaycastHit> _hits
    {
        get
        {
            var ray = ModuleController.Instance.TableCamera.ScreenPointToRay(_screenPosition);
            var ray2 = ModuleController.Instance.SphereCamera.ScreenPointToRay(_screenPosition);
            Debug.DrawRay(ModuleController.Instance.SphereCamera.transform.position, ray2.direction, Color.yellow, 100f);
            var hits = Physics.RaycastAll(ray).ToList();
            hits.AddRange(Physics.RaycastAll(ray2));
            foreach (var hit in hits)
            {
                Debug.Log("raycast hit = " + hit.transform.name);
            }
            return  hits;
        }
    }

    private DraggingBase _currentDraggingObject;
    private DraggingBase _dragginObject
    {
        get
        {
            if (ModuleController.Instance.GamoMode != GameMode.Table ||
                _hits.Any(hit => hit.transform.TryGetComponent<SlotComponent>(out _) == true))
                return _sphereRotator;
            if (ModuleController.Instance.GamoMode == GameMode.Table
                && _hits.Any(hit => hit.transform.TryGetComponent<TableComponent>(out _) == true))
                return _cameraMover;
            return default;
        }
    }
    private float startTouchTime = 0;
    void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 1e-5 || Input.touchCount == 2)
        {
            OnScroll();
        }
        else if (lastTouchDistance != -1)
        {
            OnEndScrol();
        }
        
        if (_clickOnBackGround  == true)
        {
            if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) ||
                (Input.GetMouseButtonDown(0)))
            {
                startTouchTime = Time.time;
                if (_isDragging == false)
                {
                    _currentDraggingObject = _dragginObject;
                    if (_currentDraggingObject != null)
                    {
                        _currentDraggingObject.BeginDrag(_screenPosition);
                    }
                }
            }

            if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) ||
                (Input.GetMouseButton(0)))
            {
                if (_isDragging == false)
                {
                    if (Vector2.Distance(_lastScreenPosition, _screenPosition) > 1)
                        _isDragging = true;
                }

                if (_isDragging == true)
                {
                    if (_currentDraggingObject != null)
                        _currentDraggingObject.Drag(_screenPosition);
                }
            }

            if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Canceled) ||
                (Input.GetMouseButtonUp(0)))
            {
                var touchTime = Time.time - startTouchTime;
                if (_isDragging == false)
                {
                    OnClick(_screenPosition, touchTime);
                }
                else
                {
                    if (_currentDraggingObject != null)
                        _currentDraggingObject.EndDrag(_screenPosition);
                    _currentDraggingObject = null;
                    _isDragging = false;
                }
            }
        }
        _lastScreenPosition = _screenPosition;
    }

    private float lastTouchDistance = -1;
    private void OnEndScrol()
    {
        lastTouchDistance = -1;
    }
    
    private void OnScroll()
    {
        if (ModuleController.Instance.GamoMode != GameMode.Table)
            return;
        if (lastTouchDistance != -1  || Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 1e-5)
        {
            float currentScrollSpeed;
            if (Input.touchCount == 2)
            {
                var distance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                currentScrollSpeed = distance;
                lastTouchDistance = currentScrollSpeed;
            }
            else
            {
                currentScrollSpeed = -Input.GetAxis("Mouse ScrollWheel");
            }
            var pos = ModuleController.Instance.TableCamera.transform.position;
            ModuleController.Instance.TableCamera.transform.position = new Vector3(pos.x,
                Mathf.Clamp(pos.y + currentScrollSpeed, 0, 5), pos.z);
        }
    }

    private void OnClick(Vector2 screenPosition, float touchTime)
    {
        var rays = _hits;
        if ((rays.Any(currentHit => currentHit.transform.CompareTag("UI") == true) == false))
        {
            ClickBehaviour touchedObject = null;
            rays = rays.OrderBy(hitObject =>hitObject.distance).ToArray();
            foreach (var currentHit in rays)
            {
                if (currentHit.transform.TryGetComponent<ClickBehaviour>(out var finderToucheObject))
                {
                    touchedObject = finderToucheObject;
                    break;
                }
            }
            if (touchedObject != null)
            {
                if (touchedObject.TryGetComponent<ClickBehaviour>(out var clickedObject))
                {
                    clickedObject.OnClick(touchTime);
                }
            }
        }
    }
}
