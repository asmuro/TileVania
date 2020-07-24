using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    #region SerializedFields

    [SerializeField] float timeBetweenScenes= 2;

    #endregion

    #region Fields

    private Animator myAnimator;

    #endregion

    #region Monobehaviour

    void Start()
    {
        this.myAnimator = GetComponentInChildren<Animator>();
    }    

    #endregion

    #region Loader

    public void LoadNext(float pSecondsToWait)
    {
        StartCoroutine(LoadNextScene(pSecondsToWait));
    }

    public void LoadNext()
    {
        this.LoadNext(this.timeBetweenScenes);        
    }

    IEnumerator LoadNextScene(float pSecondsToWait)
    {
        this.myAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(pSecondsToWait);
        SceneManager.LoadScene("Level" + " " + (Convert.ToInt32(SceneManager.GetActiveScene().name.Split(' ')[1]) + 1));
    }

    #endregion
}
