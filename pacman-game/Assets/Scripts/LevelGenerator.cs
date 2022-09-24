using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
public class LevelGenerator : MonoBehaviour
{
    protected float tile = 1;
    [SerializeField]
    private GameObject level01Manual;
    [SerializeField]
    private GameObject emptyObject;
    [SerializeField]
    private GameObject outsideCorner;
    [SerializeField]
    private GameObject outsideWall;
    [SerializeField]
    private GameObject insideCorner;
    [SerializeField]
    private GameObject insideWall;
    [SerializeField]
    private GameObject standardPellet;
    [SerializeField]
    private GameObject powerPellet;
    [SerializeField]
    private GameObject tJunction;

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



    void Start()
      {
        Destroy(level01Manual);
        level01();
       
    }

    private void rotateGameObject(GameObject gameObject, int row, int column)
    {
        gameObject.transform.Rotate(0f, 0f, rotationMap[row, column]);
      
    }

    GameObject generateGameObject(int map)
    {
        switch (map)
        {
            case 0:
                return emptyObject;
                break;
            case 1:
                return outsideCorner;
                break;
            case 2:
                return outsideWall;
                break;
            case 3:
                return insideCorner;
                break;
            case 4:
                return insideWall;
                break;
            case 5:
                return standardPellet;
                break;
            case 6:
                return powerPellet;
                break;
            case 7:
                return tJunction;
                break;
            default:
                return emptyObject;
        }
       
    }

    GameObject generateGameObject(int[,] map)
    {
        GameObject groupGameObject = new GameObject();
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int column = 0; column < map.GetLength(1); column++)
            {
                GameObject gameObject;
                float posX = ((column + 0.32f) * tile * 0.32f);
                float posY = ((row + 0.32f) * -tile * 0.32f);
                gameObject = Instantiate<GameObject>(generateGameObject(levelMap[row, column]), groupGameObject.transform);
                gameObject.transform.position = new Vector3(posX, posY, 0);
                rotateGameObject(gameObject, row, column);
            }
        }

        return groupGameObject;
    }

    private void level01()
    {
        GameObject gameObject1 = generateGameObject(levelMap);
        GameObject gameObject2 = generateGameObject(levelMap);
        GameObject gameObject3 = generateGameObject(levelMap);
        GameObject gameObject4 = generateGameObject(levelMap);
        gameObject2.transform.localScale = new Vector3(-1, 1, 1);
        gameObject2.transform.position = new Vector3(8.84f, 0, 0);

        gameObject3.transform.localScale = new Vector3(1, -1, 1);
        gameObject3.transform.position = new Vector3(0, -9.48f, 0);
        gameObject4.transform.localScale = new Vector3(-1, -1, 1);
        gameObject4.transform.position = new Vector3(8.84f, -9.48f, 0);
    }



}
