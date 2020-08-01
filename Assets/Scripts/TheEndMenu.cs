using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TheEndMenu : MonoBehaviour
{
    #region SerializedFields

    [SerializeField] List<GameObject> MenuItems;    

    #endregion

    #region Fields

    private Vector2 newMovement;
    private int selectedMenuItem = -1;    

    #endregion

    #region Monobehaviour

    void Start()
    {
        this.MenuItems.ForEach(m =>
        { if (m.GetComponent<ItemMenu>() != null)
                m.GetComponent<ItemMenu>().ItemMenuPressed += OnItemMenuPressed;
        });
        this.SelectFirstItem();
    }   

    void Update()
    {
        this.Move(this.newMovement);
    }

    #endregion

    #region Movement

    private void SelectFirstItem()
    {
        this.MenuItems[0].GetComponent<Animator>().SetBool("Selected", true);
        this.selectedMenuItem = 0;
    }

    public void OnMenuMove(InputAction.CallbackContext context)
    {
        if (context.started == true)
            this.newMovement = context.ReadValue<Vector2>();
    }

    private void Move(Vector2 pDirection)
    {
        if (pDirection.sqrMagnitude < 0.01)
            return;

        if (pDirection.y > 0 || pDirection.x < 0) //up
        {
            this.SelectPreviousItem();
        }
        else if (pDirection.y < 0 || pDirection.x > 0)//down
        {
            this.SelectNextItem();
        }
        
        this.newMovement = new Vector2(0, 0);
    }

    #endregion

    #region Selection

    private void SelectNextItem()
    {
        if (this.selectedMenuItem >= (this.MenuItems.Count - 1))
            this.selectedMenuItem = 0;
        else
            this.selectedMenuItem += 1;

        this.Select();
    }

    private void SelectPreviousItem()
    {
        if (this.selectedMenuItem <= 0)
            this.selectedMenuItem = this.MenuItems.Count - 1;
        else
            this.selectedMenuItem -= 1;

        this.Select();
    }

    private void Select()
    {
        this.MenuItems.ForEach(e => e.GetComponent<Animator>().SetBool("Selected", false));
        this.MenuItems[this.selectedMenuItem].GetComponent<Animator>().SetBool("Selected", true);
    }

    #endregion

    #region Pressed

    public void OnPressed(InputAction.CallbackContext context)
    {
        this.MenuItems[this.selectedMenuItem].GetComponent<Animator>().SetTrigger("Pressed");
    }    

    #endregion

    #region Handlers

    private void OnItemMenuPressed(object sender, EventArgs e)
    {
        switch (this.selectedMenuItem)
        {
            case 0://restart
                LevelLoaderSingleton.Instance.LoadFirstLevel();
                break;
            case 1://mainmenu
                LevelLoaderSingleton.Instance.LoadMainMenu();
                break;
            default://exit
                Application.Quit();
                break;
        }
    }

    #endregion
}
