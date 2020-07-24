using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{
    #region SerializedFields

    [SerializeField] float portalAnimationSpeed = 1;    

    #endregion

    #region Fields
    
    private bool hasEntered = false;
    private bool isPlayerRight = false;
    private bool isPlayerUp = false;    
    private Vector2 contact1 = new Vector2(0,0);
    private Vector2 contact2 = new Vector2(0,0);
    private Player myPlayer;    
    private CapsuleCollider2D myCollider;
    private Transform myPortalHole;
    private PortalAnimationState state = PortalAnimationState.Starting;
    private LevelLoader myLevelLoader;

    #endregion

    #region Monobehaviour
    private void Start()
    {
        this.myLevelLoader = FindObjectOfType<LevelLoader>();        
        this.myPlayer = FindObjectOfType<Player>();        
        this.myCollider = GetComponent<CapsuleCollider2D>();
        this.myPortalHole = this.gameObject.transform.GetChild(0);
    }

    private void Update()
    {
        if (this.hasEntered)
            this.PortalAnimation();
        
        if (contact1 != null && contact2 != null)
            Debug.DrawRay(contact1, contact2, Color.blue);
    }

    #endregion

    #region Portal Animation

    #region Start

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[5];
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && this.state == PortalAnimationState.Starting)
        {
            this.myPlayer.externalMovementFinishedEvent += OnExternalMovementFinishedEvent;
            this.hasEntered = true;
            this.myPlayer.Freeze();            
        }
    }

    private void PortalAnimation()
    {
        if (this.myPlayer != null && this.state == PortalAnimationState.Starting)
        {
            if (this.myPlayer.transform.position.x > this.transform.position.x)
                this.isPlayerRight = true;
            else
                this.isPlayerRight = false;
            
            if (this.myPlayer.transform.position.y > this.transform.position.y)
                this.isPlayerUp = true;
            else
                this.isPlayerUp = false;

            this.myPlayer.SetExternalMovementSpeed(portalAnimationSpeed);
            this.myPlayer.SetExternalMovementPoint(this.GenerateFirstDestinationPoint());
            this.myPlayer.SetZeroGravity();
            this.myPlayer.SetNoCollision();
            this.myPlayer.StartRotating();
            this.myPlayer.StartShrinking();
            this.state = PortalAnimationState.FirstMovement;
        }       
    }    

    #endregion

    #region Continue

    private void OnExternalMovementFinishedEvent(object sender, EventArgs e)
    {
        switch (this.state)
        {
            case PortalAnimationState.FirstMovement:                
                this.myPlayer.SetExternalMovementPoint(this.GenerateSecondDestinationPoint());
                this.state = PortalAnimationState.SecondMovement;
                break;
            case PortalAnimationState.SecondMovement:                
                this.myPlayer.SetExternalMovementPoint(this.GenerateThirdDestinationPoint());
                this.state = PortalAnimationState.ThirdMovement;
                break;
            case PortalAnimationState.ThirdMovement:
                this.myPlayer.SetExternalMovementPoint(this.GenerateFourthDestinationPoint());
                this.state = PortalAnimationState.FourthMovement;
                break;
            case PortalAnimationState.FourthMovement:
                this.myPlayer.SetExternalMovementPoint(this.GenerateFifthDestinationPoint());
                this.state = PortalAnimationState.FifthMovement;
                break;
            case PortalAnimationState.FifthMovement:
                this.myLevelLoader.LoadNext();                
                break;
            default:
                break;
        }
    }

    #endregion

    #region X And Y calculations

    private Vector2 GenerateFirstDestinationPoint()
    {
        float xPos = 0;
        float yPos = 0;
        if (this.isPlayerRight)
            xPos = (this.transform.position.x - (float)this.myCollider.size.x / 2);                                
        else
            xPos = (this.transform.position.x + (float)this.myCollider.size.x / 2);
        
        if (this.isPlayerUp)
            yPos = (this.transform.position.y - (float)this.myCollider.size.y / 4) ;
        else
            yPos = (this.transform.position.y + (float)this.myCollider.size.y / 4) ;

        return new Vector2(xPos, yPos);
    }

    private Vector2 GenerateSecondDestinationPoint()
    {
        float xPos = 0;
        float yPos = 0;
        if (this.isPlayerRight)
            xPos = (this.transform.position.x);
        else
            xPos = (this.transform.position.x);

        if (this.isPlayerUp)
            yPos = (this.transform.position.y - (float)this.myCollider.size.y/2);
        else
            yPos = (this.transform.position.y + (float)this.myCollider.size.y/2);

        return new Vector2(xPos, yPos);
    }

    private Vector2 GenerateThirdDestinationPoint()
    {
        float xPos = 0;
        float yPos = 0;
        if (this.isPlayerRight)
            xPos = (this.transform.position.x + (float)this.myCollider.size.x / 4);
        else
            xPos = (this.transform.position.x - (float)this.myCollider.size.x / 4);

        if (this.isPlayerUp)
            yPos = (this.transform.position.y - (float)this.myCollider.size.y / 4);
        else
            yPos = (this.transform.position.y + (float)this.myCollider.size.y / 4);

        return new Vector2(xPos, yPos);
    }

    private Vector2 GenerateFourthDestinationPoint()
    {
        float xPos = 0;
        float yPos = 0;
        if (this.isPlayerRight)
            xPos = (this.transform.position.x + (float)this.myCollider.size.x / 6);
        else
            xPos = (this.transform.position.x - (float)this.myCollider.size.x / 6);

        if (this.isPlayerUp)
            yPos = (this.transform.position.y + (float)this.myCollider.size.y / 6);
        else
            yPos = (this.transform.position.y - (float)this.myCollider.size.y / 6);

        return new Vector2(xPos, yPos);
    }

    private Vector2 GenerateFifthDestinationPoint()
    {
        return new Vector2(myPortalHole.position.x, myPortalHole.position.y);
    }

    #endregion

    #endregion

    #region Scene

    private void LoadNextScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log(String.Format(" Loading Scene  {0}", "Level" + " " + (Convert.ToInt32(currentSceneName.Split(' ')[1]) + 1)));
        SceneManager.LoadScene("Level" + " " + (Convert.ToInt32(currentSceneName.Split(' ')[1]) + 1));
    }

    #endregion
}
