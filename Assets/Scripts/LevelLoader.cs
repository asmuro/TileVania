using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    #region Fields

    private Animator myAnimator;
    [SerializeField] public Slider slider;
    [SerializeField] public GameObject loadingScreen;
    [SerializeField] public Text textPercentage;



    #endregion    

    #region Properties

    private static LevelLoader _instance;
    public static LevelLoader Instance 
    {
        get 
        {
            if (!_instance)
            {
                LevelLoader[] levelsLoadersAux = Resources.FindObjectsOfTypeAll<LevelLoader>();
                if (levelsLoadersAux != null && levelsLoadersAux.Length > 0)
                    _instance = levelsLoadersAux[0];
            }
            if (!_instance)                
                _instance = new LevelLoader();
            return _instance;
        }
    }

    #endregion

    #region Monobehaviour

    private void Awake() 
    {
        LevelLoaderSingleton.Instance.SetLevelLoader(this);
        slider.enabled = false;
    }
    void Start() 
    {
        this.myAnimator = GetComponentInChildren<Animator>();
    }

    #endregion

    #region Animations

    public void SetTrigger(string pTriggerName)
    {
        this.myAnimator.SetTrigger(pTriggerName);
    }

    #endregion

}
