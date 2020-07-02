using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.Advertisements;

public class UnityAdControle : MonoBehaviour
{

    public static bool showAds = true;

    public static DateTime? proxTimeReward = null;

    public static string gameId = "3653570";

    public static bool testMode = true;

    public static ObstaculoComp obstaculo;

    /// <summary>
    /// Metodo para mostrar ad com recompensa
    /// </summary>
    public static void ShowRewardAd()
    {
#if UNITY_ADS

        proxTimeReward = DateTime.Now.AddSeconds(15);

        if (Advertisement.IsReady())
        {
            // Pausar o jogo
            MenuPauseComportamento.stopped = true;
            Time.timeScale = 0f;

            var options = new ShowOptions
            {
                resultCallback = TratarMostrarResultado
            };

            Advertisement.Show(options);
        }
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    ///
#if UNITY_ADS
    public static void TratarMostrarResultado(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                // Anuncio mostrado. Continue o jogo
                obstaculo.Continue();
                break;
            case ShowResult.Skipped:
                Debug.Log("Ad Pulado.");
                break;
            case ShowResult.Failed:
                Debug.LogError("Erro no ad");
                break;
        }

        MenuPauseComportamento.stopped = false;
        Time.timeScale = 1f;
    }
#endif


    public static void ShowAd()
    {
#if UNITY_ADS

        ShowOptions options = new ShowOptions();

        options.resultCallback = Unpause;

        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }

        ///MenuPauseComportamento.stopped = true;
        //Time.timeScale = 0;
#endif
    }

    public static void Unpause(ShowResult result)
    {
        MenuPauseComportamento.stopped = false;
        Time.timeScale = 1f;
    }


    public static void InitAds()
    {
        Advertisement.Initialize(gameId, testMode);
    }

    // Start is called before the first frame update
    void Start()
    {
        showAds = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
