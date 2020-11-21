using UnityEngine;

[AddComponentMenu("Hitman GO/Managers/Game Manager")]
public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public Map map { get; private set; }
    public Player player { get; private set; }
    public LevelManager LevelManager { get; private set; }
    public ShowPath showPath { get; private set; }
    public UIManager uiManager { get; private set; }

    int lastLevel = -1;
    System.Type hintToActivate;

    void Awake()
    {
        //singleton
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //set references
        instance.SetReferences();
    }

    void SetReferences()
    {
        //set references
        map = FindObjectOfType<Map>();
        player = FindObjectOfType<Player>();
        LevelManager = FindObjectOfType<LevelManager>();
        showPath = FindObjectOfType<ShowPath>();
        uiManager = FindObjectOfType<UIManager>();

        //check last level
        if (LevelManager)
        {
            CheckLastLevel();
        }
        else
        {
            //if there is not level manager (play menu music)
            AudioManager.instance.PlayMusic();
        }

        //check hint
        if (hintToActivate != null)
            ActivateHint();
    }

    void CheckLastLevel()
    {
        int currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        //set if restart this level or not
        if (lastLevel == currentLevel)
        {
            LevelManager.isAgainSameLevel = true;
        }
        else
        {
            LevelManager.isAgainSameLevel = false;
        }

        //set last level
        lastLevel = currentLevel;
    }

    #region hint

    void ActivateHint()
    {
        //foreach achievement in scene
        Achievement[] achievements = FindObjectsOfType<Achievement>();
        foreach(Achievement achievement in achievements)
        {
            //find one of this type and activate
            if(achievement.GetType() == hintToActivate)
            {
                achievement.ActivateHints();
                break;
            }
        }

        //remove reference
        hintToActivate = null;
    }

    public void StartHints<T>(T hintToActivate)
    {
        //save reference
        this.hintToActivate = hintToActivate.GetType();

        //reload scene
        FunctionsUI.instance.ReloadScene();
    }

    #endregion
}
