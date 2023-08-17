using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadSinglePlayerMode()
    {
        // Start the game, config on the button component
        SceneManager.LoadScene(1);
    }

    public void LoadCoOpMode()
    {
        SceneManager.LoadScene(2);
    }
}
