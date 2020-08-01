using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : ScriptableObject
{
    #region SerializableFields

    [SerializeField] int playerLives = 3;    

    #endregion

    #region Fields

    private int playerScore = 0;
    private int currentPlayerLives;

    #endregion

    #region Properties

    private static GameSession _instance;
    public static GameSession Instance 
    {
        get 
        {
            if (!_instance)
            {
                GameSession[] gameSessions = Resources.FindObjectsOfTypeAll<GameSession>();
                if (gameSessions != null && gameSessions.Length > 0)
                    _instance = gameSessions[0];
            }
            if (!_instance)                
                _instance = new GameSession();
            return _instance;
        }
    }

    #endregion

    #region Score

    public void AddScore(int pScore)
    {
        this.playerScore += pScore;
    }

    public int GetScore()
    {
        return this.playerScore;
    }

    #endregion

#region Player Lives

    public void AddPlayerLives(int pLives)
    {
        this.currentPlayerLives += pLives;
    }

    public int GetPlayerLives()
    {
        return this.currentPlayerLives;
    }
#endregion

    #region ScriptableObject
    void OnEnable ()
	{
        //Para que el Garbage Collector no se lo coma
		hideFlags = HideFlags.HideAndDontSave;
        this.playerScore = 0;
        this.currentPlayerLives = this.playerLives;
    }

    #endregion

}
