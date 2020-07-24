using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    #region Serialized Fields

    [SerializeField] float moveSpeed = 1;
    [SerializeField] float maxSpeed = 5;
    [SerializeField] float idleTime = 3;

    #endregion

    #region Constants

    private string PATROL_ANIMATOR_PARAMETER = "IsPatrolling";

    #endregion

    #region Fields

    private int currentDirection;
    private Animator myAnimator;
    private Rigidbody2D myRigidbody;
    private GameObject myBody;

    #endregion

    #region MonoBehaviour

    // Start is called before the first frame update
    void Start()
    {
        while (this.currentDirection == 0)
        {
            this.currentDirection = UnityEngine.Random.Range(1, -1);
        }
        this.myAnimator = GetComponent<Animator>();
        this.myAnimator.SetBool(PATROL_ANIMATOR_PARAMETER, false);
        this.StartCoroutine(nameof(this.WaitAndPatrol));
        this.myRigidbody = GetComponent<Rigidbody2D>();
        this.myBody = this.transform.Find("Body").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (this.myAnimator.GetBool(PATROL_ANIMATOR_PARAMETER))
            this.Move();
    }

    #endregion

    #region Move

    //private void Move()
    //{
    //    Vector2 direction = new Vector2(currentDirection, 0);
    //    if (direction.sqrMagnitude < 0.01)
    //        return;
    //    float scaledMoveSpeed = this.moveSpeed * Time.deltaTime;
    //    Vector3 move = new Vector3(direction.x, 0, 0);
    //    this.transform.position += move * scaledMoveSpeed;
    //}

    private void Move()
    {
        Vector2 direction = new Vector2(currentDirection, 0);
        if (direction.sqrMagnitude < 0.01)
            return;


        float scaledMoveSpeed = this.moveSpeed * Time.deltaTime;
        Vector3 move = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0) * new Vector3(direction.x, direction.y, 0);        
        if (Math.Abs(this.myRigidbody.velocity.x) < maxSpeed)
            this.myRigidbody.AddForce(new Vector2(move.x * scaledMoveSpeed, move.y));
    }

    private IEnumerator WaitAndPatrol()
    {
        yield return new WaitForSeconds(this.idleTime);
        this.myAnimator.SetBool(PATROL_ANIMATOR_PARAMETER, true);
    }

    public bool IsHeadingLeft()
    {
        return this.currentDirection == -1;
    }

    public void StropAndTurn()
    {
        this.myRigidbody.velocity = new Vector2 (0, this.myRigidbody.velocity.y);
        this.myBody.transform.localScale = new Vector3(this.myBody.transform.localScale.x * -1, this.myBody.transform.localScale.y, this.myBody.transform.localScale.z);
        this.currentDirection = this.currentDirection * -1;
        
    }

    #endregion
}
