using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerateMap : MonoBehaviour
{
    //Material
    [SerializeField]
    public Material cellEnemyMaterial;
    [SerializeField]
    public Material cellNoneMaterial;
    [SerializeField]
    public Material cellPlayerMaterial;
    [SerializeField]
    public Material cellChestMaterial;
    [SerializeField]
    public Material cellHomesMaterial;
    [SerializeField]
    public Material cellForestMaterial;
    [SerializeField]
    private int seed = 1122;

    //Weight
    [SerializeField]
    private int noneWeight = 9;
    [SerializeField]
    private int enemyWeight = 2;
    [SerializeField]
    private int chestWeight = 1;
    [SerializeField]
    private int homesWeight = 3;
    [SerializeField]
    private int forestWeight = 5;

    //Prefabs
    [SerializeField]
    public GameObject playerPrefab;
    [SerializeField]
    public GameObject enemyPrefab;
    [SerializeField]
    public GameObject chestPrefab;

    //Rand
    System.Random randMain;
    System.Random random = new System.Random();

    //Size
    public WorldSize sizeWorld;

    public int cellSize = 25;

    private int smallSizeWidth = 10;
    private int smallSizeHeight = 10;
    private int mediumSizeWidth = 15;
    private int mediumSizeHeight = 15;
    private int bigSizeWidth = 15;
    private int bigSizeHeight = 15;

    private float maxWidth;
    private float maxHeight;

    //World params
    private int maxCountEnemyInCell = 10;
    private int minCountEnemyInCell = 4;
    private int maxCountChestInCell = 4;
    private int minCountChestInCell = 1;

    private void Start()
    {

        randMain = new System.Random(seed);
        GenerateWorld();
    }

    void GenerateWorld()
    {
        switch (sizeWorld)
        {
            case WorldSize.Small: CreateMap(smallSizeWidth, smallSizeHeight); break;
            case WorldSize.Medium: CreateMap(mediumSizeWidth, mediumSizeHeight); break;
            case WorldSize.Big: CreateMap(bigSizeWidth, bigSizeHeight); break;
        }
    }

    TypeCell[,] CreateBaseGrid(int width, int height)
    {
        int[] spawn = { randMain.Next(width / 2 - 2, width / 2 + 3), randMain.Next(height / 2 - 2, height / 2 + 3) };
        int maxWeight = noneWeight + enemyWeight + chestWeight + homesWeight + forestWeight;
        TypeCell[,] mapGrid = new TypeCell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if ((i >= spawn[0] - 1 && i <= spawn[0] + 1) && (j >= spawn[1] - 1 && j <= spawn[1] + 1))
                {
                    if (i == spawn[0] && j == spawn[1])
                    {
                        mapGrid[i, j] = TypeCell.Player;
                        continue;
                    }
                    mapGrid[i, j] = TypeCell.None;
                    continue;
                }
                switch (randMain.Next(0, maxWeight))
                {
                    case int n when (n < noneWeight):
                        mapGrid[i, j] = TypeCell.None; break;
                    case int n when (n < noneWeight + enemyWeight):
                        mapGrid[i, j] = TypeCell.Enemy; break;
                    case int n when (n < noneWeight + enemyWeight + chestWeight):
                        mapGrid[i, j] = TypeCell.Chest; break;
                    case int n when (n < noneWeight + enemyWeight + chestWeight + homesWeight):
                        mapGrid[i, j] = TypeCell.Homes; break;
                    default:
                        mapGrid[i, j] = TypeCell.Forest; break;
                }
            }
        }
        return mapGrid;
    }

    void CreateMap(int width, int height)
    {
        TypeCell[,] typeCells = CreateBaseGrid(width, height);
        maxWidth = (width * cellSize) / 2;
        maxHeight = (height * cellSize) / 2;
        for (int i = 0; i < typeCells.GetLength(0); i++)
        {
            for (int j = 0; j < typeCells.GetLength(1); j++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.localScale = new Vector3(cellSize, 1, cellSize);
                cube.transform.position = new Vector3(i * cellSize - maxWidth, 0, j * cellSize - maxHeight);
                switch (typeCells[i, j])
                {
                    case TypeCell.None:
                        CreateNoneCell(i, j);
                        break;
                    case TypeCell.Enemy:
                        CreateEnemyCell(i, j);
                        break;
                    case TypeCell.Chest:
                        CreateChestCell(i, j);
                        break;
                    case TypeCell.Homes:
                        CreateHomesCell(i, j);
                        break;
                    case TypeCell.Forest:
                        CreateForestCell(i, j);
                        break;
                    case TypeCell.Player:
                        CreatePlayerCell(i, j);
                        break;
                }
            }
        }
    }

    void CreatePlayerCell(int widthPos, int heightPos)
    {
        GameObject cube = createDemoCube(widthPos, heightPos, out float[] position);
        cube.GetComponent<Renderer>().material = cellPlayerMaterial;
        GameObject player = Instantiate(playerPrefab, new Vector3(widthPos * cellSize - maxWidth, 1, heightPos * cellSize - maxHeight), Quaternion.identity);
        player.GetComponent<ThirdPersonMove>().cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().target = player.transform;
        player.GetComponent<PlayerHealth>().healthSlider = GameObject.FindGameObjectWithTag("PlayerHPSlider").GetComponent<UnityEngine.UI.Slider>();
    }

    void CreateEnemyCell(int widthPos, int heightPos)
    {
        GameObject cube = createDemoCube(widthPos, heightPos, out float[] position);
        cube.GetComponent<Renderer>().material = cellEnemyMaterial;
        int numberEnemies = randMain.Next(minCountEnemyInCell, maxCountEnemyInCell + 1);
        int[] enemysCenterPos = {
            randMain.Next(Convert.ToInt32(position[0] - cellSize / 2 + 5), Convert.ToInt32(position[0] + cellSize / 2 - 5)),
            randMain.Next(Convert.ToInt32(position[1] - cellSize / 2 + 5), Convert.ToInt32(position[1] + cellSize / 2 - 5)),
        };

        List<int[]> enemysPos = new List<int[]>();

        for (int i = 2; i < numberEnemies + 2; i++)
        {

            while (true)
            {
                int[] pos = new int[] {
                    randMain.Next(Convert.ToInt32(enemysCenterPos[0] - i), Convert.ToInt32(enemysCenterPos[0] + i)),
                    randMain.Next(Convert.ToInt32(enemysCenterPos[1] - i), Convert.ToInt32(enemysCenterPos[1] + i))
                };

                if (isPossiblePlace(enemysPos, pos))
                {
                    Instantiate(enemyPrefab, new Vector3(
                        pos[0],
                        1,
                        pos[1]
                        ), Quaternion.identity);
                    enemysPos.Add(pos);
                    break;
                }
            }

        }
    }

    bool isPossiblePlace(List<int[]> positions, int[] position)
    {
        foreach (int[] pos in positions)
        {
            if ((position[0] + 2 >= pos[0] && position[0] - 2 <= pos[0]) && (position[1] + 2 >= pos[1] && position[1] - 2 <= pos[1]))
            {
                return false;
            }
        }
        return true;
    }

    void CreateChestCell(int widthPos, int heightPos)
    {
        GameObject cube = createDemoCube(widthPos, heightPos, out float[] position);
        cube.GetComponent<Renderer>().material = cellChestMaterial;
        int numberChests = randMain.Next(minCountChestInCell, maxCountChestInCell + 1);
        int R = randMain.Next(4, 9);
        int Deg = randMain.Next(120, 360);
        PlaceChest(numberChests, R, Deg, new Vector3(position[0], 0, position[1]));

    }

    void PlaceChest(int n, float R, float Deg, Vector3 center)
    {

        float angle = Deg / n;

        for (int i = 0; i < n; i++)
        {
            float x = center.x + R * Mathf.Sin(angle * i * Mathf.Deg2Rad);
            float z = center.z + R * Mathf.Cos(angle * i * Mathf.Deg2Rad);

            Vector3 pos = new Vector3(x, 0.5f, z);
            Instantiate(chestPrefab, pos, Quaternion.identity);
        }
    }

    GameObject createDemoCube(int widthPos, int heightPos, out float[] position)
    {
        position = new float[] { widthPos * cellSize - maxWidth, heightPos * cellSize - maxHeight };
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(cellSize, 1, cellSize);
        cube.transform.position = new Vector3(position[0], 0, position[1]);
        return cube;
    }

    void CreateNoneCell(int widthPos, int heightPos)
    {
        GameObject cube = createDemoCube(widthPos, heightPos, out float[] position);
        cube.GetComponent<Renderer>().material = cellNoneMaterial;
    }

    void CreateHomesCell(int widthPos, int heightPos)
    {
        GameObject cube = createDemoCube(widthPos, heightPos, out float[] position);
        cube.GetComponent<Renderer>().material = cellHomesMaterial;
        Homes homes = new();
        homes.Create(position, cellSize, randMain);
    }
    void CreateForestCell(int widthPos, int heightPos)
    {
        GameObject cube = createDemoCube(widthPos, heightPos, out float[] position);
        cube.GetComponent<Renderer>().material = cellForestMaterial;
        Forest forest = new();
        forest.Create(position, cellSize, randMain);
    }
}
