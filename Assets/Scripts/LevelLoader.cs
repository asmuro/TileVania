using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    #region Constants
    private int MAIN_MENU_BUILD_INDEX = 0;
    private int FIRST_LEVEL_BUILD_INDEX = 1;
    private int LAST_LEVEL_BUILD_INDEX = 3;
    private int THE_END_SCENE_BUILD_INDEX = 4;

    #endregion

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
        this.LoadNext(pSecondsToWait, -1);
    }

    private void LoadNext(float pSecondsToWait, int pSceneBuildIndex)
    {
        StartCoroutine(LoadNextScene(pSecondsToWait, pSceneBuildIndex));
    }

    public void LoadNext()
    {
        this.LoadNext(this.timeBetweenScenes);        
    }

    public void LoadFirstLevel()
    {
        this.LoadNext(this.timeBetweenScenes, FIRST_LEVEL_BUILD_INDEX);
    }

    public void LoadMainMenu()
    {
        this.LoadNext(this.timeBetweenScenes, MAIN_MENU_BUILD_INDEX);
    }

    IEnumerator LoadNextScene(float pSecondsToWait, int pSceneBuildIndex)
    {
        this.myAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(pSecondsToWait);
        if (pSceneBuildIndex > -1)
        {
            SceneManager.LoadScene(pSceneBuildIndex);    
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == LAST_LEVEL_BUILD_INDEX)
                SceneManager.LoadScene(THE_END_SCENE_BUILD_INDEX);    
            else
                SceneManager.LoadScene("Level" + " " + (Convert.ToInt32(SceneManager.GetActiveScene().name.Split(' ')[1]) + 1));        
        }
    }

    #endregion
}
