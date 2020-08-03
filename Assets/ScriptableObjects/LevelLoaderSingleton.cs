using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderSingleton : ScriptableObject
{
    #region Constants
    private int MAIN_MENU_BUILD_INDEX = 0;
    private int FIRST_LEVEL_BUILD_INDEX = 1;
    private int LAST_LEVEL_BUILD_INDEX = 4;
    private int THE_END_SCENE_BUILD_INDEX = 5;

    #endregion

    #region SerializedFields

    [SerializeField] float timeBetweenScenes= 2;

    private LevelLoader myLevelLoader;

    #endregion    

    #region Properties

    private static LevelLoaderSingleton _instance;
    public static LevelLoaderSingleton Instance 
    {
        get 
        {
            if (!_instance)
            {
                LevelLoaderSingleton[] levelsLoaders = Resources.FindObjectsOfTypeAll<LevelLoaderSingleton>();
                if (levelsLoaders != null && levelsLoaders.Length > 0)
                    _instance = levelsLoaders[0];
            }
            if (!_instance)                
                _instance = new LevelLoaderSingleton();
            return _instance;
        }
    }

    #endregion

    #region Loader

    public void SetLevelLoader(LevelLoader pLevelLoader)
    {
        this.myLevelLoader = pLevelLoader;
    }
    public void LoadNext(float pSecondsToWait)
    {
        this.LoadNext(pSecondsToWait, -1);
    }

    private void LoadNext(float pSecondsToWait, int pSceneBuildIndex)
    {
        this.myLevelLoader.StartCoroutine(LoadNextScene(pSecondsToWait, pSceneBuildIndex));        
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

    public void LoadGameOver()
    {
        this.LoadNext(this.timeBetweenScenes, THE_END_SCENE_BUILD_INDEX);
    }

    IEnumerator LoadNextScene(float pSecondsToWait, int pSceneBuildIndex)
    {
        this.myLevelLoader.SetTrigger("Start");
        yield return new WaitForSeconds(pSecondsToWait);
        int sceneToLoad = 0;
        if (pSceneBuildIndex > -1)
        {
            sceneToLoad = pSceneBuildIndex;
            // SceneManager.LoadScene(pSceneBuildIndex);    
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex == LAST_LEVEL_BUILD_INDEX)
            {
                sceneToLoad = THE_END_SCENE_BUILD_INDEX;
                // SceneManager.LoadScene(THE_END_SCENE_BUILD_INDEX);    
            }
            else
            {
                sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
                // SceneManager.LoadScene("Level" + " " + (Convert.ToInt32(SceneManager.GetActiveScene().name.Split(' ')[1]) + 1));        
            }
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!operation.isDone)
        {
            if (this.myLevelLoader != null)
            {
                myLevelLoader.loadingScreen.SetActive(true);
                float progress = Mathf.Clamp01(operation.progress/0.9f);
                myLevelLoader.slider.value = progress;
                myLevelLoader.textPercentage.text = String.Format("{0}%", progress * 100);
            }
            yield return null;
        }
        
    }    

    #endregion

    #region Reestart Level

    public void ReStartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
