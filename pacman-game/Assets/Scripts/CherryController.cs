using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    [SerializeField] GameObject cherry;
    [SerializeField] GameObject[] cherrySpawner;
    [SerializeField] float speed = 5f;
    [SerializeField] float timeBetweenSpawn = 10f;
    static float elapsedTime;

    GameObject spawnedCherry;
    Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = timeBetweenSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime -= Time.deltaTime;
        if(elapsedTime <= 0){
            if(spawnedCherry != null){
                Destroy(spawnedCherry);
            }
            spawnedCherry=SpawnCherry();
            elapsedTime = timeBetweenSpawn;
        }
        spawnedCherry.transform.Translate(direction * Time.deltaTime * speed);
        
    }

    GameObject SpawnCherry(){
        int i = UnityEngine.Random.Range(0, cherrySpawner.Length);
        float spawnPosY = UnityEngine.Random.Range(cherrySpawner[i].transform.position.y - 2f, cherrySpawner[i].transform.position.y + 2f);
        Mathf.Clamp(spawnPosY, -0.31f, -8.68f);
        GameObject go = Instantiate(cherry, new Vector3(cherrySpawner[i].transform.position.x, spawnPosY, 0), Quaternion.identity);
        if(i == 0){
            direction = Vector2.right;
        }else{
            direction = Vector2.left;
        }
        return go;
    }

}
