using UnityEngine;

public class MouseRaycaster : MonoBehaviour
{
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private Camera cam;
    private ObjectRotated objectRotated = null;
    
    private void Update()
    {
        Raycast();
        Select();
    }

    private void Select()
    {
        if (!Input.GetMouseButtonDown(0))
            return;
        
        if (objectRotated)
            objectRotated.SetSelected(true);
    }
    
    private void Raycast()
    {
        if (!cam.gameObject.activeSelf)
            return;

        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit _hit, Mathf.Infinity,
                raycastMask))
        {
            ResetObject();
            return;
        }
        
        ObjectRotated _objectRotated = _hit.collider.GetComponentInParent<ObjectRotated>();
        
        if (_objectRotated)
        {
            if (objectRotated == _objectRotated)
                return;
                
            ResetObject();
            _objectRotated.SetHover(true);
            objectRotated = _objectRotated;
        }
        else   
            ResetObject();
    }

    private void ResetObject()
    {
        if (!objectRotated)
            return;
        
        objectRotated.SetHover(false);
        objectRotated = null;
    }
}
