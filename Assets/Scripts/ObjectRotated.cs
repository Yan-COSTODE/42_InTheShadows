using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectRotated : MonoBehaviour
{
    public event Action OnObjectFinished;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Transform rotating;
    [SerializeField] private Vector3 targetEuler;
    [SerializeField, Range(0.0f, 100.0f)] private float fMinTolerance = 10.0f;
    [SerializeField] private float fLerpTime = 1.0f;
    [SerializeField] private float fSpeed = 10.0f;
    private Quaternion targetRotation;
    private Quaternion baseRotation;
    private float fProgress = 0.0f;
    private float fTimer = 0.0f;
    private bool bFinished = false;
    private bool bSelected = false;
    private bool bHover = false;

    private Quaternion rotation => rotating.rotation;
    public float LerpTime => fLerpTime;
    public float Progress => fProgress;
    
    private void Start()
    {
        targetRotation = Quaternion.Euler(targetEuler);
        baseRotation = rotation;
    }

    private void Update()
    {
        if (!bFinished)
        {
            Move();
            CheckProgress();
        }
        else
            GoToTargetRotation();
    }

    public void SetHover(bool _status)
    {
        if (bFinished || bSelected)
            return;
        
        if (bHover && !_status)
            RemoveMaterial(GameManager.Instance.LevelTemplate.OutlineMaterial);
        else if (!bHover && _status)
            AddMaterial(GameManager.Instance.LevelTemplate.OutlineMaterial);
        
        bHover = _status;
    }
    
    public void SetSelected(bool _status)
    {
        if (bFinished)
            return;
        
        if (bSelected && !_status)
            RemoveMaterial(GameManager.Instance.LevelTemplate.OutlineSelectedMaterial);
        else if (!bSelected && _status)
        {
            SetHover(false);
            AddMaterial(GameManager.Instance.LevelTemplate.OutlineSelectedMaterial);
        }
        
        bSelected = _status;
    }

    private void AddMaterial(Material _material)
    {
        List<Material> _materials = meshRenderer.sharedMaterials.ToList();
        _materials.Add(_material);
        meshRenderer.sharedMaterials = _materials.ToArray();
    }

    private void RemoveMaterial(Material _material)
    {
        List<Material> _materials = meshRenderer.sharedMaterials.ToList();
        _materials.Remove(_material);
        meshRenderer.sharedMaterials = _materials.ToArray();
    }
    
    private void Move()
    {
        if (!bSelected)
            return;
        
        float _xRotation = 0.0f;
        float _yRotation = 0.0f;
        float _zRotation = 0.0f;

        if (Input.GetKey(KeyCode.W))
            _xRotation += fSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            _xRotation -= fSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            _yRotation -= fSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            _yRotation += fSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Q))
            _zRotation -= fSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E))
            _zRotation += fSpeed * Time.deltaTime;

        Quaternion deltaRotation = Quaternion.Euler(_xRotation, _yRotation, _zRotation);
        transform.rotation *= deltaRotation;
    }
    
    private void OnDestroy()
    {
        OnObjectFinished = null;
    }

    private void CheckProgress()
    {
        baseRotation = rotation;
        float _angleDifference = Quaternion.Angle(baseRotation, targetRotation);
        fProgress = Mathf.InverseLerp(0, 180, _angleDifference);
        fProgress = (1 - fProgress) * 100.0f;
        
        if (fProgress + fMinTolerance < 100.0f)
            return;
        
        SetSelected(false);
        bFinished = true;
        fProgress = 100.0f;
    }

    private void GoToTargetRotation()
    {
        rotating.rotation = Quaternion.Slerp(baseRotation, targetRotation, fTimer / fLerpTime);
        fTimer += Time.deltaTime;

        if (fTimer < fLerpTime) 
            return;
        
        OnObjectFinished?.Invoke();
        Destroy(this);
    }
}
