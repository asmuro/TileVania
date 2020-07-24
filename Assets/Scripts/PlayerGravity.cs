using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    #region Fields

    private BoxCollider2D bottomColliderBox;    

    private Player player;
    #endregion

    #region MonoBehaviour

    // Start is called before the first frame update
    void Start()
    {
        this.player = GetComponent<Player>();
        this.bottomColliderBox = this.transform.Find("BottomCollider").gameObject.GetComponent<BoxCollider2D>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.bottomColliderBox.IsTouchingLayers(LayerMask.GetMask("Climbing")) && this.bottomColliderBox.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            this.player.SetInitialGravity();
        }
        else if(this.bottomColliderBox.IsTouchingLayers(LayerMask.GetMask("Climbing")) && !this.bottomColliderBox.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            this.player.SetZeroGravity();
        }
        else
            this.player.SetInitialGravity();
    }

    #endregion 
}
