using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleController : MonoBehaviour
{
    #region singleton
    private static ModuleController _instance;
    public static ModuleController Instance
    {
        get {
            if (_instance == null)
                _instance = FindObjectOfType<ModuleController>();
            return _instance;
        }
    }
    #endregion
    public Camera TableCamera;
    public Camera SphereCamera;
    public SphereComponent SphereRoot;
    public SphereComponent SphereDetails;
    public TableComponent Table;
    public DetailShuffler Shuffler;
    public DetailComponent SelectedDetail;
    public GameMode GamoMode { get => _modeSystem.mode; set => _modeSystem.mode = value; }

    [SerializeField] private float _animationDuration = 0.8f;
    [SerializeField] private int _squeres = 5;
    [SerializeField] private int _devideNumbers = 5;
    [SerializeField] private float _radius = 1;
    [SerializeField] private float _thickness = 0.1f;
    [SerializeField] private Material _defaultSphereMaterial;
    [SerializeField] private Material _defaultRootSphereMaterial;

    private ModeSystem _modeSystem;

    public void Start()
    {
        if (Game.Get<GameLoader>().GameLoaderData != null)
        {
            _squeres = Game.Get<GameLoader>().GameLoaderData.Squeres;
        }
        _modeSystem = new ModeSystem(TableCamera, SphereRoot, _animationDuration);

        var meshDetails = SphereDetails.Generate(_squeres, _devideNumbers, _radius, _thickness);
        var rootDetails = SphereRoot.Generate(_squeres, _devideNumbers, _radius, 0);

        foreach (var meshDetail in meshDetails)
            meshDetail.gameObject.AddComponent<DetailComponent>().Init(meshDetails);

        foreach (var rootDetail in rootDetails)
            rootDetail.gameObject.AddComponent<SlotComponent>();
        
        var sprite = Game.Get<GameLoader>().GameLoaderData?.SelectedSprite;
        
        if (sprite != null)
            _defaultSphereMaterial.mainTexture = sprite.texture;
        AddMaterial(meshDetails, _defaultSphereMaterial);
        AddMaterial(rootDetails, _defaultRootSphereMaterial);

        TableCamera.transform.localEulerAngles += -Vector3.left * 10;

    }

    private void AddMaterial(IEnumerable<AbstractDetail> details, Material material)
    {
        foreach (var detail in details)
            detail.GetComponent<MeshRenderer>().material = material;

    }

    public void ShuffleDetails()
    {
        Shuffler.Shuffle(SphereDetails.Details);
    }

    public void SetDetailPositionOnSphere(SlotComponent slot, DetailComponent detail)
    {
        var slotPosition = SphereRoot.transform.position;
        var detailPosition = slot.transform.position;
        
        detail.transform.parent.position =
            Vector3.LerpUnclamped(slotPosition, detailPosition, (_radius + _thickness / 2) / _radius);
        detail.transform.parent.LookAt(Vector3.LerpUnclamped(slotPosition, detailPosition,_radius * 2));
    }
}
