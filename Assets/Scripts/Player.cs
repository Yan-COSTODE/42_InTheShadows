using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float fLerpTime = 1.0f;
    [SerializeField] private float fCooldown = 0.5f;
    [SerializeField] private int iMaxLevel = 1;
    [SerializeField] private float fPathLength = 3.0f;
    [SerializeField, Range(0.0f, 1080.0f)] private float fSwipeThreshold = 50.0f;
    [SerializeField] private Animator animator;
    private MapTile currentTile;
    private bool bCanMove = false;
    private bool bMoving = false;
    private float fTimer = 0.0f;
    private Vector3 nextPosition;
    private Vector3 basePosition;
    private Vector2 startSwipe;
    private Vector2 endSwipe;

    private void Start()
    {
        SelectTile();
    }

    private void Update()
    {
        if (!bCanMove)
            return;
        
        GetInput(Input.GetAxis("Horizontal"));
        DetectSwipe();
        Move();
        TryGettingTile();
        TryLaunchingTile();
    }

    public void SetCanMove(bool _status) => bCanMove = _status;

    private void DetectSwipe()
    {
        if (Input.GetMouseButtonDown(0))
            startSwipe = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endSwipe = Input.mousePosition;
            ProcessSwipe();
        }
    }

    private void ProcessSwipe()
    {
        Vector2 swipeDirection = endSwipe - startSwipe;

        if (Mathf.Abs(swipeDirection.x) > fSwipeThreshold && Mathf.Abs(swipeDirection.y) < fSwipeThreshold)
            GetInput(swipeDirection.x);
    }
    
    private void TryGettingTile()
    {
        if (!bMoving && !currentTile)
            SelectTile();
    }
    
    private void TryLaunchingTile()
    {
        if (!currentTile)
            return;

        if (!Input.GetKey(KeyCode.Return) && !Input.GetKey(KeyCode.KeypadEnter)) 
            return;
        
        bCanMove = false;
        currentTile.SelectTile();
    }
    
    private void SelectTile()
    {
        if (currentTile)
            return;
        
        if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit _hit, 5.0f))
            return;
        
        currentTile = _hit.transform.GetComponent<MapTile>();
        
        if (currentTile)
            currentTile.SetUIStatus(true);
    }
    
    private void GetInput(float _axis)
    {
        if (bMoving || !bCanMove)
            return;
        
        switch (_axis)
        {
            case < 0 when transform.position.x == 0:
                return;
            case < 0:
                GoTo(-3, -90);
                break;
            case > 0 when transform.position.x == iMaxLevel * fPathLength:
                return;
            case > 0:
                if (currentTile && currentTile.Status)
                    GoTo(3, 90);
                break;
        }
    }

    private void GoTo(int _posX, int _rotationY)
    {
        basePosition = transform.position;
        nextPosition = basePosition;
        nextPosition.x += _posX;
        transform.eulerAngles = new Vector3(0, _rotationY, 0);
        bMoving = true;
        animator.SetBool("bMoving", true);

        if (!currentTile)
            return;
        
        currentTile.SetUIStatus(false);
        currentTile = null;
    }
    
    private void Move()
    {
        if (!bMoving)
            return;

        fTimer += Time.deltaTime;
        transform.position = Vector3.Lerp(basePosition, nextPosition, Easing.Ease(fTimer / fLerpTime, EEasing.EASE_OUT_QUAD));

        if (fTimer >= fLerpTime)
        {
            animator.SetBool("bMoving", false);
            SelectTile();
        }
        
        if (fTimer < fLerpTime + fCooldown)
            return;
        
        fTimer = 0.0f;
        bMoving = false;
    }
}
