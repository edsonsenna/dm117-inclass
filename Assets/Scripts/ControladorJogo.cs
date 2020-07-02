using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControladorJogo : MonoBehaviour
{

    [Tooltip("Referencia para o TileBasico")]
    public Transform tile;

    [Tooltip("Referencia para o obstaculo")]
    public Transform obstaculo;

    [Tooltip("Ponto para colocar o TileBasicoInicial")]
    public Vector3 pontoInicial = new Vector3(0, 0, -5);

    [Tooltip("Quantidade de Tiles Iniciais")]
    public int numSpawnIni;

    [Tooltip("Numero de tiles sem obstaculos")]
    public int numTilesSemOBS = 4;

    public Text placar = null;

    private static int points = 0;

    /// <summary>
    /// Local para spawn do proximo Tile
    /// </summary>
    private Vector3 proxTilePos;

    /// <summary>
    /// Rotação do próximo Tile
    /// </summary>
    private Quaternion proxTileRot;

    // Start is called before the first frame update
    void Start()
    {
        //Text tempText = Text.FindObjectOfType<Text>();
        //placar = tempText && tempText.CompareTag("Placar") ? tempText : null;
        points = 0;
#if UNITY_ADS
        UnityAdControle.InitAds();
#endif
        proxTilePos = pontoInicial;
        proxTileRot = Quaternion.identity;

        for(int i = 0; i < numSpawnIni; i++)
        {
            SpawnProxTile(i >= numTilesSemOBS);
        }
    }

    public void SpawnProxTile(bool spawnObstaculos)
    {
        var novoTile = Instantiate(tile, proxTilePos, proxTileRot); 
        var proxTile = novoTile.Find("Ponto_Spawn");
        proxTilePos = proxTile.position;
        proxTileRot = proxTile.rotation;

        if (!spawnObstaculos)
            return;

        // Podemos criar obstaculos
        var pontosObstaculo = new List<GameObject>();

        // Varrer os pontos GOs filhos buscando os pontos de spawn
        foreach(Transform filho in novoTile)
        {
            if (filho.CompareTag("ObsSpawn"))
                pontosObstaculo.Add(filho.gameObject);
        }

        if(pontosObstaculo.Count > 0)
        {
            // Pegando ponto aleatorio
            var pontoSpawn = pontosObstaculo[Random.Range(0, pontosObstaculo.Count)];

            // Guarda posicao do ponto para spawn
            var obsSpawnPos = pontoSpawn.transform.position;

            // Cria um novo obstaculo
            var novoObs = Instantiate(obstaculo, obsSpawnPos, Quaternion.identity);

            // Attach o novo obstaculo ao ponto de spawn como filho
            novoObs.SetParent(pontoSpawn.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Text tempText = Text.FindObjectOfType<Text>();
        //placar = tempText && tempText.CompareTag("Placar") ? tempText : null;
    }

    public static void UpdatePoints(int value)
    {
        points += value;
    }

    public static int GetPoints()
    {
        return points;
    }
}
