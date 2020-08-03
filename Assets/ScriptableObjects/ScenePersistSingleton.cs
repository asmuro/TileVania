using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersistSingleton : ScriptableObject
{
    #region Fields

    private int sceneBuildIndex = 0;
    private List<Transform> pickups;

    #endregion
    
    #region Properties

    private static ScenePersistSingleton _instance;

    public static ScenePersistSingleton Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType<ScenePersistSingleton>();
            if (!_instance)
                _instance = new ScenePersistSingleton();
            return _instance;
        }
    }
        
    #endregion

    #region SceneBuildIndex

    public void SetSceneBuildIndex(int pSceneBuildIndex)
    {
        this.sceneBuildIndex = pSceneBuildIndex;
    }

    public int GetSceneBuildIndex()
    {
        return this.sceneBuildIndex;
    }

    #endregion

    #region Pickups

    public void SetPickups(List<Transform> pPickups)
    {
        this.pickups = pPickups;
    }

    public List<Transform> GetPickups()
    {
        return this.pickups;
    }

    #endregion
}
