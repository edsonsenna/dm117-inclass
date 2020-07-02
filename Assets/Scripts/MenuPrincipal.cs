using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

#if UNITY_ADS
        if(UnityAdControle.showAds)
        {
            print("Enter Ad");
            UnityAdControle.ShowAd();
        }
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        UnityAdControle.InitAds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
