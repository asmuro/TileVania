using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAndLives : MonoBehaviour
{
    #region SerializeField

    [SerializeField] Text score;
    [SerializeField] GameObject FirstUp;
    [SerializeField] GameObject SecondUp;
    [SerializeField] GameObject ThirdUp;

    #endregion
    
    #region Monobehaviour

    // Update is called once per frame
    void Update()
    {
        this.score.text = GameSession.Instance.GetScore().ToString("D6");
        this.RefreshLives();
    }

    #endregion

    #region Lives

    private void RefreshLives()
    {
        if (GameSession.Instance.GetPlayerLives() == 3)
        {
            this.FirstUp.SetActive(true);
            this.SecondUp.SetActive(true);
            this.ThirdUp.SetActive(true);
        }
        else if (GameSession.Instance.GetPlayerLives() == 2)
        {
            this.FirstUp.SetActive(true);
            this.SecondUp.SetActive(true);
            this.ThirdUp.SetActive(false);
        }
        else if (GameSession.Instance.GetPlayerLives() == 1)
        {
            this.FirstUp.SetActive(true);
            this.SecondUp.SetActive(false);
            this.ThirdUp.SetActive(false);
        }
        else
        {
            this.FirstUp.SetActive(false);
            this.SecondUp.SetActive(false);
            this.ThirdUp.SetActive(false);
        }
    }

    #endregion
}
