using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObstaculoComp : MonoBehaviour
{

    [Tooltip("Quanto tempo antes de reiniciar o jogo")]
    public float tempoEspera = 2.0f;

    [Tooltip("Object da explosao")]
    public GameObject explosao;

    [Tooltip("Acessor to mr of this class")]
    MeshRenderer mr = new MeshRenderer();

    [Tooltip("Acessor to box collider of this class")]
    BoxCollider bc = new BoxCollider();

    /// <summary>
    /// Variavel referencia para o jogador
    /// </summary>
    private GameObject jogador;

    private GameObject controladorJogo;

    private void OnCollisionEnter(Collision collision)
    {
        // Verifica se quem bateu no bloco foi o jogador
        if(collision.gameObject.GetComponent<JogadorComportamento>())
        {
            // Vamos esconder o jogo - ao inves de destruir
            StartCoroutine(PlaySound(collision.gameObject, true));
            //collision.gameObject.SetActive(false);
            jogador = collision.gameObject;
            //Destroy(collision.gameObject);
            //jogador.GetComponent<AudioSource>().Play();
            //this.gameObject.GetComponent<AudioSource>().Play();

            // Chama a funcao de resetar o jogo depois de um tempo
            Invoke("ResetaJogo", tempoEspera);
        }
    }

    /// <summary>
    /// Metodo invocado atraves do SendMessage(), para detectar o que foi
    /// tocado
    /// </summary>
    public void ObjetoTocado()
    {
        //print("Tocando Here!");
        if (explosao != null)
        {
            var particulas = Instantiate(explosao, transform.position, Quaternion.identity);

        }

        mr.enabled = false;
        bc.enabled = false;
        //if (controladorJogo != null)
        //{
        //    controladorJogo.GetComponent<ControladorJogo>().UpdatePoints(5);
        //}
        ControladorJogo.UpdatePoints(5);
        StartCoroutine(PlaySound(this.gameObject));
    }

    private IEnumerator PlaySound(GameObject gameObjectToPlay, bool isToHide = false)
    {
        gameObjectToPlay.GetComponent<AudioSource>().Play();
        yield return new WaitWhile(() => gameObjectToPlay.GetComponent<AudioSource>().isPlaying);
        if(isToHide)
        {
            gameObjectToPlay.SetActive(false);

        } else
        {
            Destroy(gameObjectToPlay);   
        }
    }

    private static void ClicarObjetos(Vector2 screen)
    {
        Ray toqueRay = Camera.main.ScreenPointToRay(screen);

        RaycastHit hit;

        if (Physics.Raycast(toqueRay, out hit))
        {
            hit.transform.SendMessage("ObjetoTocado", SendMessageOptions.DontRequireReceiver);
        }
    }


    /// <summary>
    /// Reinicia o jogo
    /// </summary>
    void ResetaJogo()
    {

        // Faz o menu gameover aparecer
        var gameOverMenu = GetGameOverMenu();
        gameOverMenu.SetActive(true);

        // Busca os botoes do MenuGameOver
        var botoes = gameOverMenu.transform.GetComponentsInChildren<Button>();

        Button botaoContinue = null;

        //Varre todos os botoes, em busca do botao continue
        foreach (var botao in botoes)
        {
            if (botao.gameObject.name.Equals("ContinueButton"))
            {
                botaoContinue = botao;
            }
        }

        if(botaoContinue)
        {
#if UNITY_ADS
            //botaoContinue.onClick.AddListener(UnityAdControle.ShowRewardAd);
            //UnityAdControle.obstaculo = this;

            StartCoroutine(ShowContinue(botaoContinue));

#else
            //Se nao existe ad, nao precisa mostrar o botao Continue
            botaoContinue.gameObject.setActive(false);

#endif
        }
        // Reinicia o jogo ( level - fase )
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public IEnumerator ShowContinue(Button botaoContinue)
    {
        var btnText = botaoContinue.GetComponentInChildren<Text>();

        while (true)
        {
            if(UnityAdControle.proxTimeReward.HasValue && (DateTime.Now < UnityAdControle.proxTimeReward.Value))
            {
                botaoContinue.interactable = false;

                TimeSpan restante = UnityAdControle.proxTimeReward.Value - DateTime.Now;

                var contagemRegressiva = string.Format("{0:D2}:{1:D2}", restante.Minutes, restante.Seconds);

                btnText.text = contagemRegressiva;

                yield return new WaitForSeconds(1f);


            } else
            {
                botaoContinue.interactable = true;
                botaoContinue.onClick.AddListener(UnityAdControle.ShowRewardAd);
                UnityAdControle.obstaculo = this;
                btnText.text = "Continue (AD)";
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        bc = GetComponent<BoxCollider>();
        controladorJogo = GameObject.FindGameObjectWithTag("ControladorJogo");
    }

    // Update is called once per frame
    void Update()
    {
        ClicarObjetos(Input.mousePosition);
    }

    /// <summary>
    /// Faz o reset do jogo
    /// </summary>
    public void Continue()
    {
        var go = GetGameOverMenu();
        go.SetActive(false);
        jogador.SetActive(true);

        // Explodir o obstaculo, caso o jogador resolva apertar Continue
        ObjetoTocado();
    }

    public GameObject GetGameOverMenu()
    {
        return GameObject.Find("CanvasPause").transform.Find("MenuPauseOver").gameObject;
    }


}
