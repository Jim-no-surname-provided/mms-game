using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;

    // Store our Actions
    private InputAction jumpAction;
    private InputAction movement1dAction;
    private InputAction fireAction;


    private void Start()
    {

        playerInput = GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jump"];
        fireAction = playerInput.actions["Fire"];
        movement1dAction = playerInput.actions["Movement1d"];


        fireAction.started += (context) =>
        {
            Debug.Log("Fire!");
        };

        movement1dAction.performed += UpdateX;
        movement1dAction.canceled += CancelX;
        jumpAction.started += StartJump;
        jumpAction.canceled += CancelJump;

    }


    // Public for external hooks
    public Vector3 Velocity { get; private set; }
    //// public FrameInput Input { get; private set; }
    // public bool JumpingThisFrame { get; private set; }
    public bool LandingThisFrame { get; private set; }
    public Vector3 RawMovement { get; private set; }
    public bool Grounded => _colDown;
    private Vector3 _lastPosition;

    private float _currentHorizontalSpeed, _currentVerticalSpeed;
    private bool moving = false;
    private float inputX = 0;

    // This is horrible, but for some reason colliders are not fully established when update starts...
    private bool _active;
    void Awake() => Invoke(nameof(Activate), 0.5f);
    void Activate() => _active = true;

    private void Update()
    {
        if (!_active) return;
        // Calculate velocity
        Velocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;

        // GatherInput();
        RunCollisionChecks();

        CalculateWalk(); // Horizontal movement
        CalculateJumpApex(); // Affects fall speed, so calculate before gravity
        CalculateGravity(); // Vertical movement
        CalculateJump(); // Possibly overrides vertical
        CalculateWallSlide();
        CalculateWallJump();

        MoveCharacter(); // Actually perform the axis movement
    }


    #region Gather Input

    private void StartJump(InputAction.CallbackContext context)
    {
        if (CanUseCoyote)
        {
            SetJump();
        }

        _lastJumpPressed = Time.time;

        // Wall jump
        if (wallSliding)
        {
            wallJumping = true;
            wallJumpDirection = facingLeft ? 1f : -1f;
            wallSliding = false;
        }
    }

    private void CancelJump(InputAction.CallbackContext context)
    {
        // End the jump early if button released
        if (!_colDown && !_endedJumpEarly && Velocity.y > 0)
        {
            // _currentVerticalSpeed = 0;
            _endedJumpEarly = true;
        }

    }


    private void CancelX(InputAction.CallbackContext context)
    {
        moving = false;

    }
    private void UpdateX(InputAction.CallbackContext context)
    {
        moving = true;
        inputX = context.ReadValue<float>();
        updateFacing();
    }

    #endregion

    #region Rendering - Probably will migrate into another script
    private bool facingLeft = false;
    private void updateFacing()
    {
        if (inputX == 0)
        {
            return;
        }
        if (inputX < 0 != facingLeft) // if it is different as it was
        {
            facingLeft = !facingLeft;
            GetComponent<SpriteRenderer>().flipX = facingLeft;
        }

    }
    #endregion

    #region Collisions

    [Header("COLLISION")][SerializeField] private Bounds _characterBounds;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private int _detectorCount = 3;
    [SerializeField] private float _detectionRayLength = 0.1f;
    [SerializeField][Range(0.1f, 0.3f)] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

    private RayRange _raysUp, _raysRight, _raysDown, _raysLeft;
    private bool _colUp, _colRight, _colDown, _colLeft;

    private float _timeLeftGrounded;

    // We use these raycast checks for pre-collision information
    private void RunCollisionChecks()
    {
        // Generate ray ranges. 
        CalculateRayRanged();

        // Ground
        LandingThisFrame = false;
        var groundedCheck = RunDetection(_raysDown);
        if (_colDown && !groundedCheck) _timeLeftGrounded = Time.time;// Only trigger when first leaving
        else if (!_colDown && groundedCheck)
        {
            _coyoteUsable = true; // Only trigger when first touching
            LandingThisFrame = true;
        }

        _colDown = groundedCheck;

        // Wall Slide
        if (_colLeft && Velocity.y < 0)
        {
            wallSliding = true;
        }
        else if (_colRight && Velocity.y < 0)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        // The rest
        _colUp = RunDetection(_raysUp);
        _colLeft = RunDetection(_raysLeft);
        _colRight = RunDetection(_raysRight);

        bool RunDetection(RayRange range)
        {
            return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
        }
    }

    private void CalculateRayRanged()
    {
        // This is crying out for some kind of refactor. 
        var b = new Bounds(transform.position + _characterBounds.center, _characterBounds.size);

        _raysDown = new RayRange(b.min.x + _rayBuffer, b.min.y, b.max.x - _rayBuffer, b.min.y, Vector2.down);
        _raysUp = new RayRange(b.min.x + _rayBuffer, b.max.y, b.max.x - _rayBuffer, b.max.y, Vector2.up);
        _raysLeft = new RayRange(b.min.x, b.min.y + _rayBuffer, b.min.x, b.max.y - _rayBuffer, Vector2.left);
        _raysRight = new RayRange(b.max.x, b.min.y + _rayBuffer, b.max.x, b.max.y - _rayBuffer, Vector2.right);
    }


    private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
    {
        for (var i = 0; i < _detectorCount; i++)
        {
            var t = (float)i / (_detectorCount - 1);
            yield return Vector2.Lerp(range.Start, range.End, t);
        }
    }

    private void OnDrawGizmos()
    {
        // Bounds
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

        // Rays
        if (!Application.isPlaying)
        {
            CalculateRayRanged();
            Gizmos.color = Color.blue;
            foreach (var range in new List<RayRange> { _raysUp, _raysRight, _raysDown, _raysLeft })
            {
                foreach (var point in EvaluateRayPositions(range))
                {
                    Gizmos.DrawRay(point, range.Dir * _detectionRayLength);
                }
            }
        }

        if (!Application.isPlaying) return;

        // Draw the future position. Handy for visualizing gravity
        Gizmos.color = Color.red;
        var move = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.deltaTime;
        Gizmos.DrawWireCube(transform.position + _characterBounds.center + move, _characterBounds.size);
    }

    #endregion

    #region Walk

    [Header("WALKING")][SerializeField] private float _acceleration = 90;
    [SerializeField] private float _moveClamp = 13;
    [SerializeField] private float _deAcceleration = 60f;
    [SerializeField] private float _apexBonus = 2;

    private void CalculateWalk()
    {
        if (moving)
        {
            // Set horizontal move speed
            _currentHorizontalSpeed += inputX * _acceleration * Time.deltaTime;

            // clamped by max frame movement
            _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_moveClamp, _moveClamp);

            // Apply bonus at the apex of a jump
            var apexBonus = Mathf.Sign(inputX) * _apexBonus * _apexPoint;
            _currentHorizontalSpeed += apexBonus * Time.deltaTime;
        }
        else
        {
            // No input. Let's slow the character down
            _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
        }

        if (_currentHorizontalSpeed > 0 && _colRight || _currentHorizontalSpeed < 0 && _colLeft)
        {
            // Don't walk through walls
            _currentHorizontalSpeed = 0;
        }
    }

    #endregion

    #region Gravity

    [Header("GRAVITY")][SerializeField] private float _fallClamp = -40f;
    [SerializeField] private float _minFallSpeed = 80f;
    [SerializeField] private float _maxFallSpeed = 120f;
    private float _fallSpeed;

    private void CalculateGravity()
    {
        if (_colDown)
        {
            // Move out of the ground
            if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
        }
        else
        {
            // Add downward force while ascending if we ended the jump early
            var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

            // Fall
            _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

            // Clamp
            if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;
        }
    }

    #endregion

    #region Jump

    [Header("JUMPING")][SerializeField] private float _jumpHeight = 30;
    [SerializeField] private float _jumpApexThreshold = 10f;
    [SerializeField] private float _coyoteTimeThreshold = 0.1f;
    [SerializeField] private float _jumpBuffer = 0.1f;
    [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
    private bool _coyoteUsable;
    private bool _endedJumpEarly = true;
    private float _apexPoint; // Becomes 1 at the apex of a jump
    private float _lastJumpPressed;
    private bool CanUseCoyote => _coyoteUsable && !_colDown && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
    private bool HasBufferedJump => _colDown && _lastJumpPressed + _jumpBuffer > Time.time;

    private void CalculateJumpApex()
    {
        if (!_colDown)
        {
            // Gets stronger the closer to the top of the jump
            _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(Velocity.y));
            _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
        }
        else
        {
            _apexPoint = 0;
        }
    }

    private void SetJump()
    {
        _currentVerticalSpeed = _jumpHeight;
        _endedJumpEarly = false;
        _coyoteUsable = false;
        _timeLeftGrounded = float.MinValue;
    }

    private void CalculateJump()
    {
        // Jump if: grounded or within coyote threshold || sufficient jump buffer
        if (HasBufferedJump)
        {
            SetJump();
        }


        if (_colUp)
        {
            if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
        }
    }

    #endregion

    #region Move

    [Header("MOVE")]
    [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
    private int _freeColliderIterations = 10;

    // We cast our bounds before moving to avoid future collisions
    private void MoveCharacter()
    {
        Vector3 pos = transform.position + _characterBounds.center;
        RawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
        Vector3 move = RawMovement * Time.deltaTime;
        Vector3 furthestPoint = pos + move;

        // check furthest movement. If nothing hit, move and don't do extra checks
        Collider2D hitCollider = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _groundLayer);
        if (!hitCollider)
        {
            transform.position += move;
            return;
        }

        // otherwise increment away from current pos; see what closest position we can move to
        Vector3 positionToMoveTo = pos;
        for (int i = 1; i < _freeColliderIterations; i++)
        {
            // increment to check all but furthestPoint - we did that already
            float t = (float)i / _freeColliderIterations;
            Vector2 posToTry = Vector2.Lerp(pos, furthestPoint, t);

            // if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _groundLayer))
            RaycastHit2D hit = Physics2D.BoxCast(posToTry, _characterBounds.size, 0, Vector2.up, 0, _groundLayer);
            if (hit)
            {
                transform.position = positionToMoveTo - _characterBounds.center; //! Added by me ,.,

                // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                if (i == 1)
                {
                    // Physics2D.BoxCast(posToTry, _characterBounds.size, 0, Vector2.up, 0, _groundLayer);
                    if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
                    Vector3 dir = transform.position - new Vector3(hit.point.x, hit.point.y, 0);
                    Debug.DrawLine(transform.position, new Vector3(hit.point.x, hit.point.y, 0), Color.white, 10);
                    transform.position += dir.normalized * move.magnitude;
                }

                return;
            }

            positionToMoveTo = posToTry;
        }
        #endregion
    }

    #region Wall Slide

    [Header("Wall Slide")]
    [SerializeField] private float wallSlideSpeed = 3f; // speed can't be modified --> somehow always the same
    private bool wallSliding = false;

    private void CalculateWallSlide()
    {
        if (wallSliding)
        {
            _currentVerticalSpeed = -wallSlideSpeed;
        }
    }

    #endregion

    #region Wall Jump

    [Header("Wall Jump")]
    private bool wallJumping = false;
    private float wallJumpDirection = 0f;
    private float wallJumpForce = 12f;
    private float wallJumpVerticalForce = 10f;
    private float wallJumpTime = 0.25f;
    private float wallJumpTimer = 0f;

    private void CalculateWallJump()
    {
        if (wallJumping)
        {
            _currentHorizontalSpeed = wallJumpDirection * wallJumpForce;
            _currentVerticalSpeed = wallJumpVerticalForce;
            wallJumpTimer += Time.deltaTime;
            if (wallJumpTimer >= wallJumpTime)
            {
                wallJumping = false;
                wallJumpTimer = 0f;
            }
        }
    }

    #endregion

    private struct RayRange
    {
        public RayRange(float x1, float y1, float x2, float y2, Vector2 dir)
        {
            Start = new Vector2(x1, y1);
            End = new Vector2(x2, y2);
            Dir = dir;
        }

        public readonly Vector2 Start, End, Dir;

    }
}
