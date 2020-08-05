using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSystem
{
    #region mode
    private GameMode _mode;
    public GameMode mode
    {
        get => _mode;
        set
        {
            if (_isAnimate == true || _mode == value)
                return;
            switch (value)
            {
                case GameMode.Detail: if (_mode == GameMode.Table) { SetDetailMode(); break; } else return;
                case GameMode.Sphere: SetSphereMode(); break;
                case GameMode.Table:  SeTableMode(); break;
            }
            _mode = value;

        }
    }
    #endregion

    private float _animationDuration;
    private Vector3 _sphereInSphereModePosition = new Vector3(0, 0, 2);
    private Vector3 _sphereInTableModePosition = new Vector3(-1.5f, 0f, 2.5f);
    private Vector3 _cameraInSphereModePosition = new Vector3(0, 1, -10);
    private Vector3 _cameraInTableModePosition = new Vector3(0, 5, -10);
    private Vector3 _cameraInSphereModeRotation = new Vector3(10, 0, 0);
    private Vector3 _cameraInTableModeRotation = new Vector3(85, 0, 0);
    private Camera _camera;
    private SphereComponent _sphere;

    public ModeSystem(Camera camera, SphereComponent sphere, float animationDuration)
    {
        _camera = camera;
        _sphere = sphere;
        _animationDuration = animationDuration;
        _mode = GameMode.Sphere;        
    }

    private bool _isAnimate = false;
    public void SeTableMode()
    {
        var currentCameraPosition = _camera.transform.localPosition;
        var currentCameraRotation = _camera.transform.localEulerAngles;
        var currentSpherePosition = _sphere.transform.localPosition;
        Game.Get<Coroutine>().Animate(_animationDuration, (time, isAnimate) =>
        {
            _camera.transform.localPosition = Vector3.Lerp(currentCameraPosition, _cameraInTableModePosition, time / _animationDuration);
            _camera.transform.localEulerAngles = Vector3.Lerp(currentCameraRotation, _cameraInTableModeRotation, time / _animationDuration);
            _sphere.transform.localPosition = Vector3.Lerp(currentSpherePosition, _sphereInTableModePosition, time / _animationDuration);

            _isAnimate = isAnimate;
        });
    }

    public void SetSphereMode()
    {
        SetSphereModePositions();
    }

    public void SetDetailMode()
    {
        //SetSphereModePositions();
    }

    private void SetSphereModePositions()
    {
        var currentCameraPosition = _camera.transform.localPosition;
        var currentCameraRotation = _camera.transform.localEulerAngles;
        var currentSpherePosition = _sphere.transform.localPosition;
        Game.Get<Coroutine>().Animate(_animationDuration, (time, isAnimate) =>
        {
            _camera.transform.localPosition = Vector3.Lerp(currentCameraPosition, _cameraInSphereModePosition, time / _animationDuration);
            _camera.transform.localEulerAngles = Vector3.Lerp(currentCameraRotation, _cameraInSphereModeRotation, time / _animationDuration);
            _sphere.transform.localPosition = Vector3.Lerp(currentSpherePosition, _sphereInSphereModePosition, time / _animationDuration);

            _isAnimate = isAnimate;            
        });
    }
}
