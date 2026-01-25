using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // player's components
    Transform playerTransfrom;
    Animator animator;
    CharacterController characterController;

    // camera 
    Transform cameraTransform;

    // animation's Threshold
    float standThreshold = 1f;
    float jumpThreshold = 2.1f;
    float landingThreshold = 1f;

    // animator hash
    int postureHash;
    int moveSpeedHash;
    int turnSpeedHash;
    int verticalSpeedHash;
    int feetTweenHash;

    // move
    Vector2 moveInput;
    Vector3 playerMovement = Vector3.zero;
    float walkSpeed = 2.5f;
    float runSpeed = 5.5f;

    // rotate
    float turnRad;

    // jump
    public float gravity = -9.8f;
    private float verticalSpeed;
    public float maxJumpHeight = 1.5f;
    float feetTween;
    float landingTimer;
    float landingTime = 0.15f;
    // fall
    bool isLanding;
    bool canFall;
    bool isGround;
    float fallMultiplier = 1.5f;
    float groundCheckOffset = 0.5f;
    float fallHeight = 1f;

    // jump horizatal velocity
    static readonly int CACHE_SIZE = 3;
    Vector3[] velocityCache = new Vector3[CACHE_SIZE];
    int currentCacheIndex = 0;
    Vector3 averageVelocity = Vector3.zero;

    //状態関連
    bool isRunning;
    bool isAttacking;
    bool isJumping;
    PlayerPosture playerPosture = PlayerPosture.Standing;
    PlayerLocomotion locomotionState = PlayerLocomotion.Idle;
    PlayerAttack attackState = PlayerAttack.Normal;

    private UnitStatus _unitStatus;
    private bool _isDead;
    private bool _isPlayedDeadAnim;
    private float _lastAttackTime;

    [Header("weapon")]
    [SerializeField] private Transform _weaponSlot;
    public Transform WeaponSlot => _weaponSlot;


    void OnEnable()
    {
        _unitStatus = GetComponent<UnitStatus>();
        GameManager.Instance.RigisterPlayer(_unitStatus);
    }

    // Start is called before the first frame update
    void Start()
    {
        // load data
        SaveManager.Instance.LoadPlayerData();

        //  components and camera
        playerTransfrom = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        // animation hash 
        postureHash = Animator.StringToHash("Posture");
        moveSpeedHash = Animator.StringToHash("MoveSpeed");
        turnSpeedHash = Animator.StringToHash("TurnSpeed");
        verticalSpeedHash = Animator.StringToHash("VerticalSpeed");
        feetTweenHash = Animator.StringToHash("FeetTween");

        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead)
        {
            GameManager.Instance.NotifyObservers();
            return;
        }

        _isDead = _unitStatus.CurrentHealth <= 0;

        _lastAttackTime -= Time.deltaTime;

        CheckGround();
        UpdatePlayerPosture();
        UpdatePlayerLocomotion();
        UpdatePlayerAttack();

        CalculateGravity();
        CalculateJump();
        CalculateInputDirection();
        SetupAnimator();
        UpdatePlayerRotation();
    }

    /// <summary>
    /// for player input component
    /// </summary>
    /// <param name="ctx"></param>
    public void GetMoveInput(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void GetRunInput(InputAction.CallbackContext ctx)
    {
        isRunning = ctx.ReadValueAsButton();
    }

    public void GetJumpInput(InputAction.CallbackContext ctx)
    {
        isJumping = ctx.ReadValueAsButton();
    }

    public void GetAttackInput(InputAction.CallbackContext ctx)
    {
        isAttacking = ctx.ReadValueAsButton();
    }

    /// <summary>
    /// check ground
    /// </summary>
    private void CheckGround()
    {
        var checkPosition = playerTransfrom.position + (Vector3.up * groundCheckOffset);
        var checkMaxDistance = groundCheckOffset - characterController.radius + 2 * characterController.skinWidth;

        if (Physics.SphereCast(checkPosition, characterController.radius, Vector3.down, out RaycastHit hit, checkMaxDistance))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
            canFall = !Physics.Raycast(playerTransfrom.position, Vector3.down, 100);
        }
    }

    /// <summary>
    /// update player's posture
    /// </summary>
    private void UpdatePlayerPosture()
    {
        // on air
        if (!isGround)
        {
            float peakThreshold = 0.5f;

            // jumping　→　peaking　→　falling
            if (verticalSpeed > peakThreshold)
            {
                playerPosture = PlayerPosture.Jumping;
            }
            else if (verticalSpeed < -peakThreshold && canFall)
            {
                playerPosture = PlayerPosture.Falling;
            }
            else
            {
                playerPosture = PlayerPosture.Peaking;
            }
        }
        // ground
        else
        {
            if (playerPosture == PlayerPosture.Jumping || playerPosture == PlayerPosture.Peaking || playerPosture == PlayerPosture.Falling)
            {
                playerPosture = PlayerPosture.Landing;
                landingTimer = landingTime;
                landingThreshold = Mathf.Clamp(verticalSpeed, -10, 0);
                landingThreshold *= 0.05f; // -0.5~0
                landingThreshold += 1f; // 0.5~1
                verticalSpeed = 0;
                return;
            }

            if (playerPosture == PlayerPosture.Landing)
            {
                landingTimer -= Time.deltaTime;

                if (isJumping)
                {
                    playerPosture = PlayerPosture.Jumping;
                    landingTimer = 0;
                }
                else if (landingTimer <= 0)
                {
                    playerPosture = PlayerPosture.Standing;
                }
            }
            else
            {
                if (isJumping)
                {
                    playerPosture = PlayerPosture.Jumping;
                }
                else
                {
                    playerPosture = PlayerPosture.Standing;
                }
            }
        }
    }

    /// <summary>
    /// update player's locomotion
    /// </summary>
    void UpdatePlayerLocomotion()
    {
        if (moveInput.magnitude == 0)
        {
            locomotionState = PlayerLocomotion.Idle;
        }
        else if (isRunning)
        {
            locomotionState = PlayerLocomotion.Run;
        }
        else
        {
            locomotionState = PlayerLocomotion.Walk;
        }
    }

    /// <summary>
    /// update player's attack
    /// </summary>
    void UpdatePlayerAttack()
    {
        if (_isDead)
        {
            return;
        }

        if (isAttacking)
        {
            if (_lastAttackTime <= 0 && !_unitStatus.IsTalking && !_unitStatus.IsTransing)
            {
                attackState = PlayerAttack.Attack;
                _unitStatus.IsCritical = UnityEngine.Random.value < _unitStatus.RuntimeAttackData.criticalChance;
                _lastAttackTime = _unitStatus.RuntimeAttackData.coolDownTime;
                isAttacking = false;
            }
        }
        else
        {
            attackState = PlayerAttack.Normal;
        }
    }

    /// <summary>
    /// cacultate gravity
    /// </summary>
    private void CalculateGravity()
    {
        if (isGround && verticalSpeed <= 0)
        {
            verticalSpeed = -2f;
        }
        else
        {
            bool isFalling = verticalSpeed <= 0;
            bool isShortJump = verticalSpeed > 0 && !isJumping;

            if (isFalling || isShortJump)
            {
                // falling or short jump
                verticalSpeed += gravity * fallMultiplier * Time.deltaTime;
            }
            else
            {
                verticalSpeed += gravity * Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// calculate jump
    /// </summary>
    private void CalculateJump()
    {
        if (isGround && isJumping)
        {
            // jump execute
            verticalSpeed = Mathf.Sqrt(maxJumpHeight * -2 * gravity);

            // calculate feetTween 
            feetTween = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);

            //feet by animation process, 0 ～ 0.49.9 = right feet, 0.5 ～ 1 = left feet
            feetTween = feetTween < 0.5f ? 1 : -1;
            if (locomotionState == PlayerLocomotion.Run)
            {
                feetTween *= 3;// -3 or 3
            }
            else if (locomotionState == PlayerLocomotion.Walk)
            {
                feetTween *= 2;// -2 or 2
            }
            else
            {
                feetTween = Random.Range(-1f, 1f);// -1 ~ 1
            }
        }
    }

    /// <summary>
    /// calculate input direction
    /// </summary>
    private void CalculateInputDirection()
    {
        Vector3 cameraForwardProjection = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        playerMovement = cameraForwardProjection * moveInput.y + cameraTransform.right * moveInput.x;
        playerMovement = playerTransfrom.InverseTransformVector(playerMovement);

        turnRad = playerMovement.magnitude > 0.01f ? Mathf.Atan2(playerMovement.x, playerMovement.z) : 0;
    }

    /// <summary>
    /// switch animation
    /// </summary>
    private void SetupAnimator()
    {
        if (_isDead)
        {
            characterController.enabled = false;
            animator.SetBool("IsDead", true);
            return;
        }

        switch (_unitStatus.HurtState)
        {
            case HurtState.CriticalHurt:
                animator.SetTrigger("IsGetCriticalHurt");
                _unitStatus.HurtState = HurtState.Nothing;
                break;

            case HurtState.DizzyHurt:
                animator.SetTrigger("IsGetDizzyHurt");
                _unitStatus.HurtState = HurtState.Nothing;
                break;
        }

        var tempSpeed = 0f;
        if (locomotionState == PlayerLocomotion.Run)
        {
            tempSpeed = runSpeed;
        }
        else if (locomotionState == PlayerLocomotion.Walk)
        {
            tempSpeed = walkSpeed;
        }
        float moveSpeed = playerMovement.magnitude * tempSpeed;

        switch (playerPosture)
        {
            case PlayerPosture.Standing:
                animator.SetFloat(postureHash, standThreshold, 0.1f, Time.deltaTime);
                animator.SetFloat(moveSpeedHash, moveSpeed, 0.1f, Time.deltaTime);
                break;

            case PlayerPosture.Jumping:
                animator.SetFloat(postureHash, jumpThreshold);
                animator.SetFloat(verticalSpeedHash, verticalSpeed);
                animator.SetFloat(feetTweenHash, feetTween);
                break;

            case PlayerPosture.Peaking:
                animator.SetFloat(postureHash, jumpThreshold);
                animator.SetFloat(verticalSpeedHash, verticalSpeed);
                break;

            case PlayerPosture.Falling:
                animator.SetFloat(postureHash, jumpThreshold);
                animator.SetFloat(verticalSpeedHash, verticalSpeed);
                break;

            case PlayerPosture.Landing:
                animator.SetFloat(postureHash, landingThreshold, 0.03f, Time.deltaTime);
                animator.SetFloat(moveSpeedHash, moveSpeed, 0.1f, Time.deltaTime);
                break;
        }

        //attack
        if (attackState == PlayerAttack.Attack)
        {
            animator.SetTrigger("IsAttacking");
            animator.SetBool("IsCritting", _unitStatus.IsCritical);
        }

        // rotate animation
        if (playerPosture != PlayerPosture.Jumping && playerMovement.magnitude > 0.01f)
        {
            animator.SetFloat(turnSpeedHash, turnRad, 0.1f, Time.deltaTime);
        }
    }

    /// <summary>
    /// update rotation
    /// </summary>
    private void UpdatePlayerRotation()
    {
        if (playerPosture != PlayerPosture.Jumping && playerMovement.magnitude > 0.01f)
        {
            playerTransfrom.Rotate(0, turnRad * 200f * Time.deltaTime, 0f);
        }
    }

    /// <summary>
    /// move by apply root motion
    /// </summary>
    private void OnAnimatorMove()
    {
        if (_isDead)
        {
            return;
        }

        bool isInAir = (playerPosture == PlayerPosture.Jumping || playerPosture == PlayerPosture.Peaking || playerPosture == PlayerPosture.Falling);
        Vector3 finalMoveVector;

        if (!isInAir)
        {
            finalMoveVector = animator.deltaPosition;
            averageVelocity = CalculateAverageVelocity(animator.velocity);
        }
        else
        {
            finalMoveVector = averageVelocity * Time.deltaTime;
        }

        finalMoveVector.y = verticalSpeed * Time.deltaTime;
        characterController.Move(finalMoveVector);
    }

    /// <summary>
    /// calculate average velocity
    /// </summary>
    Vector3 CalculateAverageVelocity(Vector3 newVelocity)
    {
        velocityCache[currentCacheIndex] = newVelocity;
        currentCacheIndex++;
        currentCacheIndex %= CACHE_SIZE;

        Vector3 averageVelocity = Vector3.zero;
        foreach (Vector3 velocity in velocityCache)
        {
            averageVelocity += velocity;
        }

        return averageVelocity / CACHE_SIZE;
    }
}
