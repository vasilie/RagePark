using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{

    public float sizeX = 16f;
    public float sizeY = 16f;
    public GameObject cellPrefab;
    public GameObject housePrefab;
    public GameObject businessPrefab;
    public GameObject mRdPrefab;
    public GameObject mainRdPrefab;
    public GameObject rdPrefab;

    private Vector2[] housePositions;
    public int housesNbr = 16;
    public int businessNbr = 16;
    public string[,] mapGrid;

    public int cellSize = 1;

    // Use this for initialization
    void Start(){
        mapGrid = new string[(int)sizeX, (int)sizeY];

        CreateRoads();
        GenerateHouses();
        GenerateBusinesess();
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }
   
    void CreateRoads(){
        
        //Main road
        bool horizontal = UnityEngine.Random.Range(0, 1f) > 0.5f ? true : false;
        Debug.Log("HORIZONTAL");
        Debug.Log(horizontal);
        int offset = Mathf.FloorToInt(UnityEngine.Random.Range(-4f, 4f));

        if (!horizontal){ //MAIN VERTICAL ROAD
            for (int i = 0; i < sizeY; i++)
            {   
                bool intersection = UnityEngine.Random.Range(0, 1f) > 0.9f ? true : false;
                if (intersection){
                    mapGrid[Mathf.FloorToInt(sizeX / 2f) + offset, i] = "MRHI";
                } else {
                    mapGrid[Mathf.FloorToInt(sizeX / 2f) + offset, i] = "MMRH";
                }

            }
        } else {//MAIN HORIZONTAL ROAD
            for (int i = 0; i < sizeX; i++)
            {
                bool intersection = UnityEngine.Random.Range(0, 1f) > 0.9f ? true : false;
                if (intersection)
                {
                    mapGrid[i, Mathf.FloorToInt(sizeY / 2f) + offset] = "MRVI";
                }
                else
                {
                    mapGrid[i, Mathf.FloorToInt(sizeY / 2f) + offset] = "MMRV";
                }

            }
        }
        // Find main intersections 
        // Generate main intersection roads
        if (!horizontal){
            List<Vector2> intersectionPos = searchMap(mapGrid, "MRHI");
            foreach (Vector2 position in intersectionPos)
            {
                for (int i = 0; i < sizeY; i++)
                {
                    bool intersection = UnityEngine.Random.Range(0, 1f) > 0.96f ? true : false;
                    if (intersection){
                        mapGrid[i, (int)position.y] = "RVI";
                    } else {
                        mapGrid[i, (int)position.y] = "MRV"; 
                    }
                   
                }
            }
            //Find vertical road intersections 
            //Generate roads
            List<Vector2> roadIntersectionPos = searchMap(mapGrid, "RVI");
            foreach (Vector2 position in roadIntersectionPos)
            {
                for (int i = 0; i < sizeX; i++)
                {
                    mapGrid[(int)position.x, i ] = "RH";

                }
            }
        } else {
            List<Vector2> intersectionPos = searchMap(mapGrid, "MRVI");
            foreach (Vector2 position in intersectionPos)
            {
                for (int i = 0; i < sizeX; i++)
                {
                    bool intersection = UnityEngine.Random.Range(0, 1f) > 0.96f ? true : false;
                    if (intersection){
                        mapGrid[(int)position.x, i] = "RHI";
                    } else {
                        mapGrid[(int)position.x, i] = "MRH";
                    }

                }
            }
            //Find horizontal road intersections 
            //Generate roads
            List<Vector2> roadIntersectionPos = searchMap(mapGrid, "RHI");
            foreach (Vector2 position in roadIntersectionPos)
            {
                for (int i = 0; i < sizeX; i++)
                {
                    mapGrid[  i, (int)position.y] = "RV";

                }
            }
        }

       


    }
    void GenerateHouses()
    {
        for (int i = 0; i < housesNbr; i++)
        {
            Vector2 point = GeneratePoint();
            mapGrid[(int)point.x, (int)point.y] = "H";
        }
    }
    void GenerateBusinesess()
    {
        for (int i = 0; i < businessNbr; i++)
        {
            Vector2 point = GeneratePoint();
            mapGrid[(int)point.x, (int)point.y] = "B";
        }
    }
    //Generates random point, checks if exists in mapGrid if yes creates new one again
    //Try to get the point based on attributes
    Vector2 GeneratePoint(){
        Vector2 finalPoint;
        Vector2 point = new Vector2(Mathf.Floor(UnityEngine.Random.Range(0, sizeX-1)), Mathf.Floor(UnityEngine.Random.Range(0, sizeY-1)));
        
        if (mapGrid[(int)point.x, (int)point.y] == null ){
            finalPoint = point;
        } else {
            finalPoint = GeneratePoint();
            //Debug.Log("checking for nwq point");
        }
        if (finalPoint != null){
            return finalPoint;
        }
    }
    void CreateMap(){
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                switch (mapGrid[i,j]){
                    case "MMRH":
                        Instantiate(mRdPrefab, new Vector3(i * cellSize, j * cellSize, 0), Quaternion.identity, transform);
                        break;
                    case "MMRV":
                        Instantiate(mRdPrefab, new Vector3(i * cellSize, j * cellSize, 0), Quaternion.Euler(0, 0, 90), transform);
                        break;
                    case "MRH":
                        Instantiate(mainRdPrefab, new Vector3(i * cellSize, j * cellSize, 0), Quaternion.identity, transform);
                        break;
                    case "RH":
                        Instantiate(rdPrefab, new Vector3(i * cellSize, j * cellSize, 0), Quaternion.identity, transform);
                        break;
                    case "MRV":
                        Instantiate(mainRdPrefab, new Vector3(i * cellSize, j * cellSize, 0), Quaternion.Euler(0, 0, 90), transform);
                        break;
                    case "RV":
                        Instantiate(rdPrefab, new Vector3(i * cellSize, j * cellSize, 0), Quaternion.Euler(0, 0, 90), transform);
                        break;
                    case"H":
                        Instantiate(housePrefab, new Vector3(i * cellSize, j * cellSize, 0), Quaternion.identity, transform);
                        break;
                    case "B":
                        Instantiate(businessPrefab, new Vector3(i * cellSize, j * cellSize, 0), Quaternion.identity, transform);
                        break;
                    default:
                        Instantiate(cellPrefab, new Vector3(i * cellSize, j * cellSize, 0), Quaternion.identity, transform);
                        break;
                }
            }
        }
    }
    List<Vector2> searchMap(string[,] map, string value ){
        List<Vector2> positions = new List<Vector2>();
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                if (map[i,j] == value){
                    positions.Add(new Vector2(i, j));
                }
            }
        }
        return positions;
    }
}

