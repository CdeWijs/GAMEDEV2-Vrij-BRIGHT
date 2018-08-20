using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    enum Level
    {
        INTRODUCTION,
        LEVEL1,
        LEVEL2,
    }

    private Level currentLevel;
    private static List<BaseController> controllers = new List<BaseController>();

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += ResetControllers;
    }

    private void ResetControllers(Scene scene, LoadSceneMode mode)
    {
        foreach(BaseController con in controllers)
        {
            controllers.Remove(con);
        }
    }

    private void Update()
    {
        if (controllers.Count > 0)
        {
            foreach (BaseController con in controllers)
            {
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Level03"))
                {
                    con.switchControls = true;
                }
                else
                {
                    con.switchControls = false;
                }
            }
        }
    }

    public static void AddController(BaseController controller)
    {
        BaseController existingController = null;
        if (controllers.Count > 0)
        {
            foreach (BaseController con in controllers)
            {
                if (controller == con)
                {
                    existingController = con;
                }
                Debug.Log(con + " is in controllers");
            }
        }

        if (existingController == null)
        {
            controllers.Add(controller);
            Debug.Log(controller + " added to controllers");
        }
    }
}
