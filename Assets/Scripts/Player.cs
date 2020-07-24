using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region SeriealizedFields

    [SerializeField] float moveSpeed;    
    [SerializeField] float climbingSpeed;    
    [SerializeField] float maxSpeed;    
    [SerializeField] float jumpSpeed;
    [SerializeField] float deathDramaticJump;

    #endregion

    #region Fields

    private Vector2 newMovement;
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private Collider2D myCollider;
    private Collider2D myBottomCollider;
    private IDontGoThroughThingsData dontGoThroughThings;
    private float initialGravity;
    private bool isAlive;
    private bool canMove = true;

    private bool isExternalMovementActivated = false;
    private float externalMovementSpeed;
    private Vector2 externalMovementDestination;
    private bool isRotating = false;
    private bool isShrinking = false;

    public event EventHandler externalMovementFinishedEvent;
    #endregion

    #region Fields DontGoThroughtThings

    // Careful when setting this to true - it might cause double
    // events to be fired - but it won't pass through the trigger
    public bool sendTriggerMessage = false;
    public LayerMask layerMask = -1; //make sure we aren't in this layer 
    public float skinWidth = 0.1f; //probably doesn't need to be changed 
 
    #endregion

    #region MonoBehaviour

    // Start is called before the first frame update
    void Start()
    {
        this.myRigidbody = GetComponent<Rigidbody2D>();
        this.myAnimator = GetComponent<Animator>();
        this.myCollider = GetComponent<Collider2D>();        
        this.myBottomCollider = this.transform.Find("BottomCollider").gameObject.GetComponent<BoxCollider2D>();
        this.InitDontGoThroughThings();
        this.initialGravity = this.myRigidbody.gravityScale;
        this.isAlive = true;
    }

    private void Update()
    {
        if (this.isExternalMovementActivated)
            this.ExternalMove();
    }

    void FixedUpdate()
    {
        if (this.canMove)
        {
            this.MoveHorizontal(this.newMovement);
            this.ClimbLadder(this.newMovement);
        }
        //this.dontGoThroughThings.DontGoThroughtThings();        
    }

    #endregion

    #region Movement

    public void OnMove(InputAction.CallbackContext context)
    {
        if (this.canMove)
        {
            this.Flip(this.newMovement);
            this.newMovement = context.ReadValue<Vector2>();
            if (this.newMovement.x != 0)
                this.myAnimator.SetBool("IsRunning", true);
            else
                this.myAnimator.SetBool("IsRunning", false);

            if (this.myCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")) && this.newMovement.y != 0)
                this.myAnimator.SetBool("IsClimbing", true);
            else
                this.myAnimator.SetBool("IsClimbing", false);
        }
    }
        

    private void MoveHorizontal(Vector2 pDirection)
    {
        if (pDirection.sqrMagnitude < 0.01)
            return;
        
       
        float scaledMoveSpeed = this.moveSpeed * Time.deltaTime;
        Vector3 move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(pDirection.x, 0, pDirection.y);
        //this.myRigidbody.velocity = new Vector2(move.x * scaledMoveSpeed, this.myRigidbody.velocity.y);
        if (Math.Abs(this.myRigidbody.velocity.x) < maxSpeed )
            this.myRigidbody.AddForce(new Vector2(move.x * scaledMoveSpeed, this.myRigidbody.velocity.y));
    }

    public void ClimbLadder(Vector2 pDirection)
    {
        if (this.myCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            if (pDirection.sqrMagnitude < 0.01)
            {
                this.myRigidbody.velocity = new Vector2(this.myRigidbody.velocity.x, 0);
                return;
            }

            float scaledMoveSpeed = this.climbingSpeed * Time.deltaTime;
            Vector3 move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(pDirection.x, pDirection.y, 0);

            if (Math.Abs(this.myRigidbody.velocity.x) < maxSpeed)
                this.myRigidbody.velocity = new Vector2(this.myRigidbody.velocity.x, move.y * scaledMoveSpeed);
        }
    }

    public void SetExternalMovementPoint(Vector2 pDestination)
    {
        this.isExternalMovementActivated = true;
        this.externalMovementDestination = pDestination;
    }

    public void SetExternalMovementSpeed(float pSpeed)
    {
        this.externalMovementSpeed = pSpeed;
    }

    private void ExternalMove()
    {
        float step = this.externalMovementSpeed * Time.deltaTime; // calculate distance to move
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.externalMovementDestination, step);
        if (this.isRotating)
            this.transform.Rotate(5f, 5f, 5.0f, Space.Self);
        if (this.isShrinking)
            this.transform.localScale = new Vector3(this.transform.localScale.x * 0.991f, this.transform.localScale.y * 0.991f, this.transform.localScale.z);
        if (Vector2.Distance(this.transform.position, this.externalMovementDestination) < 0.001f)
        {
            if (this.externalMovementFinishedEvent != null)
                this.externalMovementFinishedEvent.Invoke(null, EventArgs.Empty);
        }
    }

    #endregion

    #region Jump

    public void OnJump(InputAction.CallbackContext pContext)
    {
        if (this.canMove && pContext.phase == InputActionPhase.Started && this.myBottomCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            float scaledJumpSpeed = this.jumpSpeed * Time.deltaTime;
            this.myRigidbody.velocity = new Vector2(this.myRigidbody.velocity.x, scaledJumpSpeed);
        }
    }

    #endregion

    #region Flip

    private bool IsHorizontalMove(Vector2 pDirection)
    {
        return (this.IsGoingRight(pDirection) || this.IsGoingLeft(pDirection));
    }

    private void Flip(Vector2 pDirection)
    {
        if (this.IsGoingRight(pDirection))
        {
            if (!this.IsFacingRight())
                this.Turn();
        }
        else if(this.IsGoingLeft(pDirection))
        {
            if (this.IsFacingRight())
                this.Turn();
        }
    }

    private bool IsFacingRight()
    {
        if (this.transform.localScale.x == 1)
            return true;
        return false;
    }

    private bool IsGoingRight(Vector2 pDirection)
    {
        if (pDirection.normalized.x > 0)
            return true;
        return false;
    }

    private bool IsGoingLeft(Vector2 pDirection)
    {
        if (pDirection.normalized.x < 0)
            return true;
        return false;
    }

    private void Turn()
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
    }

    #endregion

    #region Gravity

    public void SetInitialGravity()
    {
        if (this.canMove)
            this.myRigidbody.gravityScale = this.initialGravity;
    }

    public void SetZeroGravity()
    {
        this.myRigidbody.gravityScale = 0f;
    }

    public void SetNoCollision()
    {
        this.myCollider.isTrigger = true;
    }

    public void StartRotating()
    {
        this.isRotating = true;
    }

    public void StartShrinking()
    {
        this.isShrinking = true;
    }

    #endregion

    #region Handlers

    private void OnTriggerEnter2D(Collider2D pCollision)
    {
        if (this.isAlive)
        {
            if (this.myBottomCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")) || pCollision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            this.Die();            
        }
    }

    #endregion

    #region Die

    private void Die()
    {
        this.isAlive = false;
        this.canMove = false;
        this.myAnimator.SetTrigger("Die");
        this.myRigidbody.AddForce(new Vector2(0, this.deathDramaticJump));
    }

    #endregion

    #region Pause

    public void Freeze()
    {
        this.canMove = false;
        this.SetZeroGravity();
        this.myRigidbody.velocity = new Vector2(0, 0);
    }

    #endregion

    #region DontGoThroughThings

    private void InitDontGoThroughThings()
    {
        DontGoThroughThingsData data = new DontGoThroughThingsData();
        data.MyRigidbody = this.myRigidbody;
        data.MyCollider = GetComponent<CapsuleCollider2D>();
        data.LayerMask = this.layerMask;
        data.SendTriggerMessage = this.sendTriggerMessage;
        data.SkinWidth = this.skinWidth;
        this.dontGoThroughThings = DontGoThroughThingsService.GetDontGoThroughThingsService(data);
    }

    #endregion
}
