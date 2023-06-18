using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Array de prefabs dos tiles
    public int mapWidth = 10; // Largura do mapa
    public int mapHeight = 10; // Altura do mapa
    public Vector3 mapPosition; // Posi��o do mapa

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // Gerar um tile aleat�rio
                int randomTileIndex = Random.Range(0, tilePrefabs.Length);
                GameObject tilePrefab = tilePrefabs[randomTileIndex];

                // Calcular a posi��o do tile
                Vector3 tilePosition = new Vector3(x, 0, y) + mapPosition;

                // Instanciar o tile na posi��o calculada
                GameObject tileInstance = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                tileInstance.transform.SetParent(transform); // Definir o objeto pai como o objeto MapGenerator
            }
        }
    }
}