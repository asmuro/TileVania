using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pickups : MonoBehaviour
{
    private int startingSceneIndex = 0;
    private void Awake() {
        int numberOfInstances = FindObjectsOfType<Pickups>().Length;
        if (numberOfInstances > 1)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        this.startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update() 
    {
            if (this.startingSceneIndex != SceneManager.GetActiveScene().buildIndex) 
                Destroy(this.gameObject);
    }
}
