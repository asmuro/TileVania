using UnityEngine;
using System.Collections;

public class DontGoThroughThings: IDontGoThroughThingsData
{
    #region Fields
    // Careful when setting this to true - it might cause double
    // events to be fired - but it won't pass through the trigger
    private bool sendTriggerMessage = false;

	private LayerMask layerMask = -1; //make sure we aren't in this layer 
	private float skinWidth = 0.1f; //probably doesn't need to be changed 

	private float minimumExtent;
	private float partialExtent;
	private float sqrMinimumExtent;
	private Vector2 previousPosition;
	private Rigidbody2D myRigidbody;
	private CapsuleCollider2D myCollider;

    #endregion

    #region Constructors

    public DontGoThroughThings(DontGoThroughThingsData pData)
	{
		this.myRigidbody = pData.MyRigidbody;
		this.myCollider = pData.MyCollider;
		this.sendTriggerMessage = pData.SendTriggerMessage;
		this.layerMask = pData.LayerMask;
		this.skinWidth = pData.SkinWidth;
		this.Initialization();
	}

    #endregion

    #region Initialization

    void Initialization()
	{		
		this.previousPosition = this.myRigidbody.position;
		this.minimumExtent = Mathf.Min(this.myCollider.bounds.extents.x, this.myCollider.bounds.extents.y);
		this.partialExtent = this.minimumExtent * (1.0f - this.skinWidth);
		this.sqrMinimumExtent = this.minimumExtent * this.minimumExtent;
	}

    #endregion

    void IDontGoThroughThingsData.DontGoThroughtThings()
	{
		//have we moved more than our minimum extent? 
		Vector2 movementThisStep = this.myRigidbody.position - this.previousPosition;
		float movementSqrMagnitude = movementThisStep.sqrMagnitude;

		if (movementSqrMagnitude > this.sqrMinimumExtent)
		{
			float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
			//RaycastHit hitInfo;
			Vector2 normalizedMovementThisStep = new Vector2(movementThisStep.x, movementThisStep.y);
			normalizedMovementThisStep.Normalize();
			RaycastHit2D rayCastHit = Physics2D.Raycast(this.previousPosition, normalizedMovementThisStep);
			//check for obstructions we might have missed 
			if (rayCastHit)
			{
				if (!rayCastHit.collider)
					return;

				if (rayCastHit.collider.isTrigger)
					rayCastHit.collider.SendMessage("OnTriggerEnter", this.myCollider);

				if (!rayCastHit.collider.isTrigger)
					this.myRigidbody.position = rayCastHit.point - (movementThisStep / movementMagnitude) * this.partialExtent;

			}
		}

		this.previousPosition = this.myRigidbody.position;
	}
}
