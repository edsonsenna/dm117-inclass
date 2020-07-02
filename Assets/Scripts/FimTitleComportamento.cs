using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FimTitleComportamento : MonoBehaviour
{

    [Tooltip("Tempo esperado antes de destruir o tile basico")]
    private float tempoDestruir = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Vamos ver se foi a bola que passou pelo fim do tile basico
        if (other.GetComponent<JogadorComportamento>())
        {
            // Como foi a bola, vamos criar um TileBasico no prox ponto
            // Mas esse proixmo ponto esta depois do ultimo TileBasico
            // presente na cena
            GameObject.FindObjectOfType<ControladorJogo>().SpawnProxTile(true) ;

            //Destroi o TileBasico depois do tempo definido
            Destroy(transform.parent.gameObject, tempoDestruir);
        }
    }
}
