using System;
using UnityEngine;
using UnityEngine.Serialization;


public class PlayerMove : MonoBehaviour
{
    public float hForce, vForce, camSpeed, shiftBoost, vBoost;
    public int minYValue;
    public bool isGrounded, vShift;
    
    [FormerlySerializedAs("Cam")] [SerializeField]
    private Camera cam;
    [FormerlySerializedAs("SpawnPoint")] [SerializeField]
    private Transform spawnPoint;
    [FormerlySerializedAs("LayerSolidGroundMask")] [SerializeField]
    private LayerMask layerSolidGroundMask;

    private float _smoothX, _smoothY, _smoothZ,_smoothEndZ;
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;


    private void Start()
    {
        _rigidBody = transform.GetComponent<Rigidbody2D>();
        _boxCollider = transform.GetComponent<BoxCollider2D>();
        _smoothEndZ = cam.transform.position.z;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround())
            _rigidBody.velocity = Vector2.up * vForce;
    }

    private void FixedUpdate()
    {
        vShift = Input.GetKey(KeyCode.LeftShift);
        
        var transformPosition = transform.position;

        if (transform.position.y < minYValue)
        {
            var spawnPointTransformPos = spawnPoint.transform.position;
            transformPosition = new Vector3(spawnPointTransformPos.x, spawnPointTransformPos.y, transformPosition.z);
        }

        isGrounded = isOnGround();

        

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _rigidBody.transform.position = new Vector3(transformPosition.x, transformPosition.y + vBoost, transformPosition.z);
            if ((!IsWallAround(Vector2.right) || isOnGround()) && !Input.GetKey(KeyCode.LeftArrow))
                _rigidBody.velocity = new Vector2(hForce + shiftBoost * Convert.ToInt32(vShift), _rigidBody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rigidBody.transform.position = new Vector3(transformPosition.x, transformPosition.y + vBoost, transformPosition.z);
            if ((!IsWallAround(Vector2.left) || isOnGround()) && !Input.GetKey(KeyCode.RightArrow))
                _rigidBody.velocity = new Vector2(-(hForce + shiftBoost * Convert.ToInt32(vShift)), _rigidBody.velocity.y);
        }
        else
        {
            var velocity = _rigidBody.velocity;
            velocity = new Vector2(velocity.x, velocity.y);
            _rigidBody.velocity = velocity;
        }


        const int camStepZ = 5;
        if (Input.GetAxis("Mouse ScrollWheel") > 0) _smoothEndZ = cam.transform.position.z + camStepZ;
        if (Input.GetAxis("Mouse ScrollWheel") < 0) _smoothEndZ = cam.transform.position.z - camStepZ;


        var position = cam.transform.position;
        _smoothZ = Mathf.SmoothStep(position.z, _smoothEndZ, camSpeed);
        
        
        _smoothX = Mathf.SmoothStep(position.x, transformPosition.x, camSpeed);
        _smoothY = Mathf.SmoothStep(position.y, transformPosition.y, camSpeed);
        
        position = new Vector3(_smoothX, _smoothY, _smoothZ);
        cam.transform.position = position;
    }

    public bool isOnGround()
    {
        const float rayMaxLength = .05f;
        var bounds = _boxCollider.bounds;
        RaycastHit2D rCastHitLeft = Physics2D.Raycast(new Vector2(bounds.min.x, bounds.center.y), Vector2.down, bounds.extents.y + rayMaxLength, layerSolidGroundMask);
        RaycastHit2D rCastHitRight = Physics2D.Raycast(new Vector2(bounds.max.x, bounds.center.y), Vector2.down, bounds.extents.y + rayMaxLength, layerSolidGroundMask);
        RaycastHit2D rCastHitMiddle = Physics2D.Raycast(new Vector2(bounds.center.x, bounds.center.y), Vector2.down, bounds.extents.y + rayMaxLength, layerSolidGroundMask);

        return ((rCastHitLeft.collider != null) || (rCastHitRight.collider != null) || (rCastHitMiddle.collider != null));
    }

    private bool IsWallAround(Vector2 vec2)
    {
        const float rayMaxLength = .02f;
        var bounds = _boxCollider.bounds;
        RaycastHit2D rCastHitUp = Physics2D.Raycast(new Vector2(bounds.center.x, bounds.max.y + rayMaxLength), vec2, bounds.extents.x + rayMaxLength, layerSolidGroundMask);
        RaycastHit2D rCastHitDown = Physics2D.Raycast(new Vector2(bounds.center.x, bounds.min.y - rayMaxLength), vec2, bounds.extents.x + rayMaxLength, layerSolidGroundMask);
        RaycastHit2D rCastHitMiddle = Physics2D.Raycast(new Vector2(bounds.center.x, bounds.center.y), vec2, bounds.extents.x + rayMaxLength, layerSolidGroundMask);

        return ((rCastHitUp.collider != null) || (rCastHitDown.collider != null) || (rCastHitMiddle.collider != null));
    }
 
}