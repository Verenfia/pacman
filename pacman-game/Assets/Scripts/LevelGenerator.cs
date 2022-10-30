using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
public class LevelGenerator : MonoBehaviour
{
    protected float tile = 1;
    public Vector2 snapSize = new Vector2(.31f,.31f);
    [SerializeField] private GameObject level01Manual;
    [SerializeField] private GameObject emptyObject;
    [SerializeField] private GameObject outsideCorner;
    [SerializeField] private GameObject outsideWall;
    [SerializeField] private GameObject insideCorner;
    [SerializeField] private GameObject insideWall;
    [SerializeField] private GameObject tJunction;
    [SerializeField] private GameObject standardPellet;
    [SerializeField] private GameObject powerPellet;
    [SerializeField] Vector2Int[] forbiddenPath;
    GameObject toSpawn;
    Vector2Int[] directions = {Vector2Int.right, Vector2Int.down, Vector2Int.left, Vector2Int.up};
    Dictionary<Vector2Int, ArrayNode> grid = new Dictionary<Vector2Int, ArrayNode>();
    
    public static int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };


    public static int[,] rotationMap =
    {
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {90,0,0,0,0,0,0,0,0,0,0,0,0,270},
        {90,0,0,180,180,270,0,0,180,180,180,270,0,270},
        {90,0,270,0,0,90,0,270,0,0,0,90,0,270},
        {90,0,90,0,0,180,0,90,0,0,0,180,0,90},
        {90,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {90,0,0,180,180,270,0,0,270,0,0,180,180,180},
        {90,0,90,0,0,180,0,270,90,0,90,0,0,270},
        {90,0,0,0,0,0,0,270,90,0,0,0,0,270},
        {90,0,0,0,0,270,0,270,90,180,180,270,0,270},
        {0,0,0,0,0,90,0,270,0,0,0,180,0,90},
        {0,0,0,0,0,90,0,270,90,0,0,0,0,0},
        {0,0,0,0,0,90,0,270,90,0,0,180,180,0},
        {0,0,0,0,0,180,0,90,180,0,270,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,270,0,0,0},
      };

    private void Awake() 
    {
        Destroy(level01Manual);
        GenerateChunks(levelMap);
        ForbiddPath();
    }
    public Vector2 ArrayToPosition(float x, float y){
        float posX = x * snapSize.x * tile;
        float posY = y * -snapSize.y * tile;
        return new Vector2(posX,posY);
    }
    public Vector2 ArrayToPosition(Vector2Int array){
        float posX = array.x * snapSize.x * tile;
        float posY = array.y * -snapSize.y * tile;
        return new Vector2(posX,posY);
    }
    public Vector2Int PositionToArray(Vector2 position){
        int arrayX = Mathf.RoundToInt(position.y / (snapSize.y * tile));
        int arrayY = -Mathf.RoundToInt(position.x / (snapSize.x * tile));
        return new Vector2Int(arrayX,arrayY);
    }
    public ArrayNode GetArrayNode(Vector2Int array){
        if(grid.ContainsKey(array)){
            return grid[array];
        }
        return null;
    }
    public Vector2Int GetArrayFromDirection(Vector2Int array,string directionInput){
        if(directionInput.Equals("w") && grid.ContainsKey(array + new Vector2Int(0,-1))){
            return grid[array].coordinate + new Vector2Int(0,-1);
        }
        if(directionInput.Equals("a") && grid.ContainsKey(array + new Vector2Int(-1,0))){
            return grid[array].coordinate + new Vector2Int(-1,0);
        }
        if(directionInput.Equals("s") && grid.ContainsKey(array + new Vector2Int(0,1))){
            return grid[array].coordinate + new Vector2Int(0,1);
        }
        if(directionInput.Equals("d") && grid.ContainsKey(array + new Vector2Int(1,0))){
            return grid[array].coordinate + new Vector2Int(1,0);
        }
        return grid[array].coordinate;
        
    }
    void GenerateChunks(int[,] map){
        grid.Clear();
        //chunk1
        GameObject chunk1 = new GameObject();
        chunk1.transform.position = Vector3.zero;

        for(int y = 0; y <= map.GetLength(0)-1; y++){
            for(int x = 0; x<= map.GetLength(1)-1; x++){
                float posX = x * snapSize.x * tile;
                float posY = -y * snapSize.y * tile;
                GenerateObject(map[y,x]);
                GameObject go = Instantiate<GameObject>(toSpawn, chunk1.transform);
                go.transform.localPosition = new Vector3(posX, posY, 0);
                go.transform.localEulerAngles = new Vector3(0,0,rotationMap[y,x]);
                go.transform.name = $"({x},{y})";
            }
        }
        
        //create grid for chunk 1
        for(int y = 0; y <= map.GetLength(0)-1; y++){
            for(int x = 0; x<= map.GetLength(1)-1; x++){
                if(map[y,x] == 0){ 
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), true, false));
                }else if(map[y,x] == 5 || map[y,x] ==6){
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), true, true));
                }else{
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), false, false));
                }
            }
        }

        //chunk2
        GameObject chunk2 = new GameObject();
        chunk2.transform.position = new Vector3(27 * snapSize.x * tile,0,0);

        for(int y = 0; y <= map.GetLength(0)-1; y++){
            for(int x = 14; x<= 27; x++){
                int arrayX = x-14;
                float posX = arrayX * snapSize.x * tile;
                float posY = -y * snapSize.y * tile;
                GenerateObject(map[y,arrayX]);
                GameObject go = Instantiate<GameObject>(toSpawn, chunk2.transform);
                go.transform.localPosition = new Vector3(posX, posY, 0);
                go.transform.localEulerAngles = new Vector3(0,0,rotationMap[y,arrayX]);
                go.transform.name = $"({x},{y})";
                arrayX--;
            }
        }
        chunk2.transform.localScale = new Vector3(-1,1,1);
        
        //create grid for chunk 2
        for(int y = 0; y <= map.GetLength(0)-1; y++){
            for(int x = 14; x<=27; x++){
                int arrayX = 27-x;
                if(map[y,arrayX] == 0){ 
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), true, false));
                }else if(map[y,arrayX] == 5 || map[y,arrayX] ==6){
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), true, true));
                }else{
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), false, false));
                }
            }
        }

        //chunk3
        GameObject chunk3 = new GameObject();
        chunk3.transform.position = new Vector3(0,-29 * snapSize.y * tile,0);

        for(int y = 15; y <= 29; y++){
            int arrayY = y-15;
            for(int x = 0; x <= map.GetLength(1)-1; x++){
                float posX = x * snapSize.x * tile;
                float posY = -arrayY * snapSize.y * tile;
                GenerateObject(map[arrayY,x]);
                GameObject go = Instantiate<GameObject>(toSpawn, chunk3.transform);
                go.transform.localPosition = new Vector3(posX, posY, 0);
                go.transform.localEulerAngles = new Vector3(0,0,rotationMap[arrayY,x]);
                go.transform.name = $"({x},{y})";
            }
            arrayY++;
        }
        chunk3.transform.localScale = new Vector3(1,-1,1);

        //create grid for chunk 3
        for(int y = 15; y <= 29; y++){
            int arrayY = 29-y;
            for(int x = 0; x <= map.GetLength(1)-1; x++){
                if(map[arrayY,x] == 0){ 
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), true, false));
                }else if(map[arrayY,x] == 5 || map[arrayY,x] ==6){
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), true, true));
                }else{
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), false, false));
                }
            }
        }

        //chunk4
        GameObject chunk4 = new GameObject();
        chunk4.transform.position = new Vector3(27* snapSize.x * tile,-29 * snapSize.y * tile,0);

        for(int y = 15; y <= 29; y++){
            int arrayY = y-15;
            for(int x = 14; x<=27; x++){
                int arrayX = x-14;
                float posX = arrayX * snapSize.x * tile;
                float posY = -arrayY * snapSize.y * tile;
                GenerateObject(map[arrayY,arrayX]);
                GameObject go = Instantiate<GameObject>(toSpawn, chunk4.transform);
                go.transform.localPosition = new Vector3(posX, posY, 0);
                go.transform.localEulerAngles = new Vector3(0,0,rotationMap[arrayY,arrayX]);
                go.transform.name = $"({x},{y})";
            }
        }
        chunk4.transform.localScale = new Vector3(-1,-1,1);

        //create grid for chunk 4
        for(int y = 15; y <= 29; y++){
            int arrayY = 29-y;
            for(int x = 14; x<=27; x++){
                int arrayX = 27-x;
                if(map[arrayY,arrayX] == 0){ 
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), true, false));
                }else if(map[arrayY,arrayX] == 5 || map[arrayY,arrayX] ==6){
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), true, true));
                }else{
                    grid.Add(new Vector2Int(x,y), new ArrayNode(new Vector2Int(x,y), false, false));
                }
            }
        }
    }
    void GenerateObject(int map)
    {
        switch (map)
        {
            case 0:
                toSpawn = emptyObject;
                break;
            case 1:
                toSpawn = outsideCorner;
                break;
            case 2:
                toSpawn = outsideWall;
                break;
            case 3:
                toSpawn = insideCorner;
                break;
            case 4:
                toSpawn = insideWall;
                break;
            case 5:
                toSpawn = standardPellet;
                break;
            case 6:
                toSpawn = powerPellet;
                break;
            case 7:
                toSpawn = tJunction;
                break;
            default:
                toSpawn = emptyObject;
                break;
        }
       
    }
    void ForbiddPath(){
        if(forbiddenPath.Length == 0) {return;}
        foreach(Vector2Int path in forbiddenPath){
            if(grid.ContainsKey(path)){
                grid[path].isWalkable = false;
            }
        }
    }
}

[System.Serializable]
public class ArrayNode{

    public Vector2Int coordinate;
    public bool isWalkable;
    public bool pickup;

    public ArrayNode(Vector2Int coordinate, bool isWalkable, bool pickup){
        this.coordinate = coordinate;
        this.isWalkable = isWalkable;
        this.pickup = pickup;
    }
}
