using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JogadorComportamento : MonoBehaviour
{
    public enum TipoMovimentoHorizontal
    {
        Acelerometro,
        Touch
    }

    [Tooltip("Tipo de movimentacao")]
    public TipoMovimentoHorizontal movimentoHorizontal = TipoMovimentoHorizontal.Acelerometro;

    /// <summary>
    /// Um referebcua para o componente Rigidbody
    /// </summary>
    // Reference to rigibody component
    private Rigidbody rb;

    // Velocidade que a bola ira se esquivar
    [Tooltip("Velocidade que a bola ira se esquivar")]
    [Range(0, 10)]
    public float velocidadeEsquiva = 5.0f;

    // Velocidade que a bola ira se deslocar
    [Tooltip("Velocidade que a bola ira se deslocar")]
    [Range(0, 10)]
    public float velocidadeDeslocamento = 5.0f;

    [Tooltip("Object da explosao")]
    public GameObject explosao;

    [Header("Atributos responsaveis pelo swipe")]
    [Tooltip("Determina qual a distancia que o dedo do jogador deve swipe")]
    public float minDisSwipe = 2.0f;

    /// <summary>
    /// Distacia que a bola ir movimentar atraves do swipe
    /// </summary>
    private float swipeMove = 2.0f;

    [Tooltip("Ponto inicial do swap")]
    private Vector2 toqueInicio;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Equals("Parede_Esquerda") || collision.gameObject.name.Equals("Parede_Direita"))
        {
            this.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Obter acesso ao componente Rigidbody associado ao
        // GameObject
        rb = GetComponent<Rigidbody>();

    }

    /// <summary>
    /// Metodo para calcular para onde o jogar se descolacara na horizonta
    /// </summary>

    private float CalculaMovimento(Vector2 screenSpaceCord)
    {
        var pos = Camera.main.ScreenToViewportPoint(screenSpaceCord);

        float direcaoX = 0;

        if (pos.x < 0.5)
            direcaoX = -1;
        else
            direcaoX = 1;

        return direcaoX * velocidadeEsquiva;
    }

    private void SwipeTeleport(Touch toque)
    {

        // Verifica se e o ponto onde tudo comecou
        if(toque.phase == TouchPhase.Began)
        {
            toqueInicio = toque.position;
        }
        else if(toque.phase == TouchPhase.Ended)
        {
            Vector2 toqueFim = toque.position;
            Vector3 direcaoMov;

            float dif = toqueFim.x - toqueInicio.x;

            if(Mathf.Abs(minDisSwipe) >= dif)
            {
                if(dif < 0)
                {
                    direcaoMov = Vector3.left;
                } else
                {
                    direcaoMov = Vector3.right;
                }
            } else
            {
                return;
            }

            RaycastHit hit;

            if(!rb.SweepTest(direcaoMov, out hit, swipeMove))
            {
                rb.MovePosition(rb.position + (direcaoMov * swipeMove));
            }
        }
    }

    /// <summary>
    /// Metodo para identificar se objetos foram tocados
    /// </summary>
    /// <param name="toque">O toque ocorrido no frame</param>
    private static void TocarObjetos(Touch toque)
    {

        // Convertemos a posicao do toque( Screen Space ) para um ray
        Ray toqueRay = Camera.main.ScreenPointToRay(toque.position);

        RaycastHit hit;

        if(Physics.Raycast(toqueRay, out hit))
        {
            hit.transform.SendMessage("ObjetoTocado", SendMessageOptions.DontRequireReceiver);
        }
    }

    /// <summary>
    /// Metodo invocado atraves do SendMessage(), para detectar o que foi
    /// tocado
    /// </summary>
    public void ObjetoTocado()
    {
        if (explosao != null)
        {
            
            //var particulas = Instantiate(explosao, transform.position, Quaternion.identity);
            //Destroy(particulas, 1.0f);
        }
        //Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        var velocidadeHorizontal = Input.GetAxis("Horizontal") * velocidadeEsquiva;

        if(!MenuPauseComportamento.stopped)
        {


    #if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
            // Detecta clica na tela ou touch
            // Clique -
            // 0 - Botao Esq
            // 1 - Botao diR
            // 2 - Bota Scroll
            // Touch -
            // 0 - Touch
            if (Input.GetMouseButton(0))
            {
                velocidadeHorizontal = CalculaMovimento(Input.mousePosition);
                Touch toque = new Touch();
                toque.position = Input.mousePosition;
                //print("Pos" + Input.mousePosition);
                //TocarObjetos(toque);
            }

#elif UNITY_IOS || UNITY_ANDROID
            if(movimentoHorizontal == TipoMovimentoHorizontal.Acelerometro)
            {
                velocidadeHorizontal = Input.acceleration.x * velocidadeDeslocamento;
            } else
            {
                // Detectar movimento exclusivamente via touch
                if (Input.touchCount > 0)
                {
                    Touch toque = Input.touches[0];
                    velocidadeHorizontal = CalculaMovimento(toque.position);
                    SwipeTeleport(toque);
                }
            }
        
#endif


            var forcaMovimento = new Vector3(velocidadeHorizontal, 0, velocidadeDeslocamento);

            forcaMovimento *= (Time.deltaTime * 60);

            rb.AddForce(velocidadeHorizontal, 0, velocidadeDeslocamento);
        }
    }
}
