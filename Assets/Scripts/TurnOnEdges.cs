using UnityEngine;

public class TurnOnEdges : MonoBehaviour
{
    #region Fields

    private BoxCollider2D myLeftCollider;
    private BoxCollider2D myRightCollider;
    private Enemy myEnemy;

    #endregion

    #region Monobehaviour

    void Start()
    {
        this.myLeftCollider = this.transform.Find("LeftCollider").GetComponent<BoxCollider2D>();
        this.myRightCollider = this.transform.Find("RightCollider").GetComponent<BoxCollider2D>();
        this.myEnemy = this.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (this.myEnemy.IsHeadingLeft() && !this.myLeftCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        //    this.myEnemy.StropAndTurn();
        //else if (!this.myEnemy.IsHeadingLeft() && !this.myRightCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        //    this.myEnemy.StropAndTurn();


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        this.myEnemy.StropAndTurn();
    }

    #endregion
}
