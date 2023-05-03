using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    [SerializeField]
    GameObject enemy;

    [SerializeField]
    public Material cellEnemyMaterial;
    [SerializeField]
    public Material cellNoneMaterial;
    [SerializeField]
    public Material cellPlayerMaterial;
    [SerializeField]
    public Material cellChestMaterial;
    [SerializeField]
    public Material cellHomeMaterial;
    [SerializeField]
    public Material cellForestMaterial;
    [SerializeField]
    private int seed = 1122;

    [SerializeField]
    private int noneWeight = 9;
    [SerializeField]
    private int enemyWeight = 2;
    [SerializeField]
    private int chestWeight = 1;
    [SerializeField]
    private int homeWeight = 3;
    [SerializeField]
    private int forestWeight = 5;


    public WorldSize sizeWorld;

    System.Random randMain;
    System.Random random = new System.Random();

    private int cellSize = 35;

    private int smallSizeWidth = 10;
    private int smallSizeHeight = 10;
    private int mediumSizeWidth = 15;
    private int mediumSizeHeight = 15;
    private int bigSizeWidth = 15;
    private int bigSizeHeight = 15;

    private float maxWidth;
    private float maxHeight;

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
                switch (randMain.Next(0, 20))
                {
                    case int n when (n < 9):
                        mapGrid[i, j] = TypeCell.None; break;
                    case int n when (n < 11):
                        mapGrid[i, j] = TypeCell.Enemy; break;
                    case int n when (n < 12):
                        mapGrid[i, j] = TypeCell.Chest; break;
                    case int n when (n < 15):
                        mapGrid[i, j] = TypeCell.Home; break;
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
                cube.transform.position = new Vector3(i * cellSize - maxWidth + i, 0, j * cellSize - maxHeight + j);
                switch (typeCells[i, j])
                {
                    case TypeCell.None:
                        cube.GetComponent<Renderer>().material = cellNoneMaterial;
                        break;
                    case TypeCell.Enemy:
                        cube.GetComponent<Renderer>().material = cellEnemyMaterial;
                        break;
                    case TypeCell.Chest:
                        cube.GetComponent<Renderer>().material = cellChestMaterial;
                        break;
                    case TypeCell.Home:
                        cube.GetComponent<Renderer>().material = cellHomeMaterial;
                        break;
                    case TypeCell.Forest:
                        cube.GetComponent<Renderer>().material = cellForestMaterial;
                        break;
                    case TypeCell.Player:
                        cube.GetComponent<Renderer>().material = cellPlayerMaterial;
                        break;
                }
            }
        }
    }

    void CreatePlayerCell(int width, int height)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(cellSize, 1, cellSize);
        cube.transform.position = new Vector3(width * cellSize - maxWidth + width, 0, height * cellSize - maxHeight + height);
        cube.GetComponent<Renderer>().material = cellPlayerMaterial;



    }


}
