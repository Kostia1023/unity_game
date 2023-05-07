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
    private int seed = 1;

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
    GameObject playerPrefab;
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    GameObject chestPrefab;
    [SerializeField]
    GameObject HomesPrefab1;
    [SerializeField]
    GameObject HomesPrefab2;
    [SerializeField]
    GameObject HomesPrefab3;
    [SerializeField]
    GameObject TreePrefab1;
    [SerializeField]
    GameObject TreePrefab2;
    [SerializeField]
    GameObject TreePrefab3;
    [SerializeField]
    GameObject GrassPrefab1;
    [SerializeField]
    GameObject GrassPrefab2;
    [SerializeField]
    GameObject FlowerPrefab1;
    [SerializeField]
    GameObject FlowerPrefab2;
    [SerializeField]
    GameObject FlowerPrefab3;
    [SerializeField]
    GameObject PlantsPrefab1;
    [SerializeField]
    GameObject PlantsPrefab2;
    [SerializeField]
    GameObject RockWall1;
    [SerializeField]
    GameObject RockWall2;

    //Rand
    System.Random randMain;
    System.Random random = new System.Random();

    //Size
    public WorldSize sizeWorld;

    public int cellSize = 25;

    public int smallSizeWidth = 10;
    public int smallSizeHeight = 10;
    public int mediumSizeWidth = 15;
    public int mediumSizeHeight = 15;
    public int bigSizeWidth = 20;
    public int bigSizeHeight = 20;

    private float maxWidth;
    private float maxHeight;

    //World params
    public int maxCountEnemyInCell = 10;
    public int minCountEnemyInCell = 4;
    public int maxCountChestInCell = 4;
    public int minCountChestInCell = 1;
    public int minCountTreeInCell = 6;
    public int maxCountTreeInCell = 15;
    public LayerMask checkLayerForGrass;

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
        CreateGrassAndFlowers(width, height);
        GenerateWalls(width, height);
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

                if (isPossiblePlace(enemysPos, pos, 2))
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

    bool isPossiblePlace(List<int[]> positions, int[] position, int minDistance)
    {
        foreach (int[] pos in positions)
        {
            if ((position[0] + minDistance >= pos[0] && position[0] - minDistance <= pos[0]) && (position[1] + minDistance >= pos[1] && position[1] - minDistance <= pos[1]))
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
        int Deg = randMain.Next(200, 360);
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
        Quaternion quaternion = Quaternion.identity;
        switch (randMain.Next(0, 3))
        {
            case 0:
                quaternion = Quaternion.Euler(0f, 90f, 0f);
                break;
            case 1:
                quaternion = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 2:
                quaternion = Quaternion.Euler(0f, -90f, 0f);
                break;
        }
        switch (randMain.Next(0, 3))
        {
            case 0:
                Instantiate(HomesPrefab1, new Vector3(position[0], 0, position[1]), quaternion);
                break;
            case 1:
                Instantiate(HomesPrefab2, new Vector3(position[0], 0, position[1]), quaternion);
                break;
            case 2:
                Instantiate(HomesPrefab3, new Vector3(position[0], 0, position[1]), quaternion);
                break;
        }

    }
    void CreateForestCell(int widthPos, int heightPos)
    {
        GameObject cube = createDemoCube(widthPos, heightPos, out float[] position);
        cube.GetComponent<Renderer>().material = cellForestMaterial;

        int numberTrees = randMain.Next(minCountTreeInCell, maxCountTreeInCell + 1);
        int[] TreesCenterPos = {
            randMain.Next(Convert.ToInt32(position[0] - cellSize / 2 + 10), Convert.ToInt32(position[0] + cellSize / 2 - 10)),
            randMain.Next(Convert.ToInt32(position[1] - cellSize / 2 + 10), Convert.ToInt32(position[1] + cellSize / 2 - 10)),
        };

        List<int[]> treesPos = new List<int[]>();

        for (int i = 7; i < numberTrees + 7; i++)
        {

            while (true)
            {
                int[] pos = new int[] {
                    randMain.Next(Convert.ToInt32(TreesCenterPos[0] - i), Convert.ToInt32(TreesCenterPos[0] + i)),
                    randMain.Next(Convert.ToInt32(TreesCenterPos[1] - i), Convert.ToInt32(TreesCenterPos[1] + i))
                };

                if (isPossiblePlace(treesPos, pos, 5))
                {
                    GameObject tree = null;
                    switch (randMain.Next(0, 3))
                    {
                        case 0:
                            tree = Instantiate(TreePrefab1, new Vector3(position[0], 0, position[1]), Quaternion.identity);
                            break;
                        case 1:
                            tree = Instantiate(TreePrefab2, new Vector3(position[0], 0, position[1]), Quaternion.identity);
                            break;
                        case 2:
                            tree = Instantiate(TreePrefab3, new Vector3(position[0], 0, position[1]), Quaternion.identity);
                            break;
                    }
                    Instantiate(tree, new Vector3(
                        pos[0],
                        1,
                        pos[1]
                        ), Quaternion.identity);
                    treesPos.Add(pos);
                    break;
                }
            }

        }
    }

    void CreateGrassAndFlowers(int width, int height)
    {
        for (int i = 0; i < width * cellSize - 2; i += 2)
        {
            for (int j = 0; j < height * cellSize - 3; j += 3)
            {
                if (!CheckPlacement()) { continue; }
                Vector3 position = new Vector3(
                    (float)randMain.NextDouble() * 2 + i - maxWidth - cellSize / 2,
                    0,
                    (float)randMain.NextDouble() * 3 + j - maxHeight - cellSize / 2);
                switch (randMain.Next(0, 15))
                {
                    case 0:
                        Instantiate(GrassPrefab1, position, Quaternion.identity);
                        break;
                    case 1:
                        Instantiate(GrassPrefab2, position, Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(FlowerPrefab1, position, Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(FlowerPrefab2, position, Quaternion.identity);
                        break;
                    case 4:
                        Instantiate(FlowerPrefab3, position, Quaternion.identity);
                        break;
                    case 5:
                        Instantiate(PlantsPrefab2, position, Quaternion.identity);
                        break;
                    case 6:
                        Instantiate(PlantsPrefab2, position, Quaternion.identity);
                        break;
                    default: break;
                }
            }

        }
    }

    private bool CheckPlacement()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1, checkLayerForGrass);

        if (colliders.Length > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void GenerateWalls(int width, int height)
    {
        for (int j = 0; j < width; j++)
        {
            switch (randMain.Next(0, 2))
            {
                case 0:
                    Instantiate(RockWall1, new Vector3(j * cellSize - maxWidth, 0,-(maxHeight + cellSize / 2)), Quaternion.identity);
                    break;
                case 1:
                    Instantiate(RockWall2, new Vector3(j * cellSize - maxWidth, 0, -(maxHeight + cellSize / 2)), Quaternion.identity);
                    break;
            }

        }
        for (int j = 0; j < width; j++)
        {
            switch (randMain.Next(0, 2))
            {
                case 0:
                    Instantiate(RockWall1, new Vector3(j * cellSize - maxWidth, 0, (maxHeight - cellSize / 2)), Quaternion.identity);
                    break;
                case 1:
                    Instantiate(RockWall2, new Vector3(j * cellSize - maxWidth, 0, (maxHeight - cellSize / 2)), Quaternion.identity);
                    break;
            }

        }
        for (int j = 0; j < height; j++)
        {
            switch (randMain.Next(0, 2))
            {
                case 0:
                    Instantiate(RockWall1, new Vector3(-(maxWidth + cellSize / 2), 0, j * cellSize - maxHeight), Quaternion.Euler(0f, 90f, 0f));
                    break;
                case 1:
                    Instantiate(RockWall2, new Vector3(-(maxWidth + cellSize / 2), 0, j * cellSize - maxHeight), Quaternion.Euler(0f, 90f, 0f));
                    break;
            }
        }
        for (int j = 0; j < height; j++)
        {
            switch (randMain.Next(0, 2))
            {
                case 0:
                    Instantiate(RockWall1, new Vector3((maxWidth - cellSize / 2), 0, j * cellSize - maxHeight), Quaternion.Euler(0f, 90f, 0f));
                    break;
                case 1:
                    Instantiate(RockWall2, new Vector3((maxWidth - cellSize / 2), 0, j * cellSize - maxHeight), Quaternion.Euler(0f, 90f, 0f));
                    break;
            }
        }
    }
}
