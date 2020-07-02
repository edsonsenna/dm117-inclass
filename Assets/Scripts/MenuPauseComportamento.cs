using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseComportamento : MonoBehaviour
{

    public static bool stopped;

    public GameObject stopMenuPanel;
    /// <summary>
    /// Metodo para reiniciar a scene
    /// </summary>
    public void Restart()
    {
        Pause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Metodo para pausar o jogo
    /// </summary>
    public void Pause(bool isStopped)
    {
        stopped = isStopped;

        Time.timeScale = (stopped) ? 0 : 1;

        stopMenuPanel.SetActive(stopped);
    }

    /// <summary>
    /// Metodo para carregar uma scene
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Start is called before the first frame update
    void Start()
    {

        //Pause(false);
#if !UNIT_ADS
        Pause(false);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
