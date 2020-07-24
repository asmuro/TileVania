using UnityEngine;
using System.Collections;

public class DontGoThroughThingsData
{
	#region Properties

	public bool SendTriggerMessage = false;
	public LayerMask LayerMask = -1; 
	public float SkinWidth = 0.1f; 
	public Rigidbody2D MyRigidbody;
	public CapsuleCollider2D MyCollider;

	#endregion	
}
