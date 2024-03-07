using UnityEngine;

public class ShadowObject : MonoBehaviour
{
    [SerializeField] 
    private Vector3 validRotation = Vector3.zero;
    [SerializeField, Range(0.0f, 180.0f)] 
    private float acceptanceRange = 5.0f;
    [SerializeField, Range(0.0f, 360.0f)] 
    private float randomRange = 50.0f;
    [SerializeField, Range(0.0f, 100.0f)]
    private float fRotationSpeed = 30.0f;
    [SerializeField] 
    private bool bIsValid = false;

    public bool IsValid => EulerIsValid(acceptanceRange);
    
    private void Start()
    {
        do
        {
            transform.rotation = Random.rotation;
        } while (EulerIsValid(randomRange));
    }

    private void Update() => bIsValid = EulerIsValid(acceptanceRange);
    
    public void Rotate(Vector3 _rotation)
    {
        Transform _transform = transform;
        Vector3 _euler = _transform.eulerAngles * Time.deltaTime * fRotationSpeed;
        _euler += _rotation;
        _transform.eulerAngles = _euler;
    }

    private bool EulerIsValid(float range)
    {
        Vector3 _euler = transform.eulerAngles;
        
        if (_euler.x < validRotation.x - range || _euler.x > validRotation.x + range)
            return false;
        if (_euler.y < validRotation.y - range || _euler.y > validRotation.y + range)
            return false;
        if (_euler.z < validRotation.z - range || _euler.z > validRotation.z + range)
            return false;
        return true;
    }
}
