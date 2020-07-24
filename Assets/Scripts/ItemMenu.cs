using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenu : MonoBehaviour
{
    #region Fields

    public event EventHandler ItemMenuPressed;

    #endregion

    #region Pressed

    public void ItemMenuPressedAnimationFinished()
    {
        if (this.ItemMenuPressed != null)
            this.ItemMenuPressed.Invoke(null, EventArgs.Empty);
    }

    #endregion
}
