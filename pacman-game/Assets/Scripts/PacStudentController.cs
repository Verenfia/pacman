using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 55f;
    [SerializeField] string lastInput = "";
    [SerializeField] string currentinput = "";
    [SerializeField] Vector2Int startArray = new Vector2Int(1,1);
    [SerializeField] float lerpDuration = 3f;
    [SerializeField] Vector2 startPosition;
    Vector2Int currentArray;
    Vector2 targetPos;
    LevelGenerator level;
    float travelPercent;
    Animator animator;
    AudioSource walkSound;
    private void Awake() {
        level = FindObjectOfType<LevelGenerator>();
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        walkSound = GetComponent<AudioSource>();
        startPosition = level.ArrayToPosition(startArray);
        currentArray = startArray;
        transform.position = startPosition;
    }
    void Update()
    {
        if(PlayerInput()){
            
        }
        Move(); 
        
    }
    void Move(){
        Vector2Int path;
        if(lastInput == ""){
            path = level.GetArrayFromDirection(startArray, "");
            targetPos = level.ArrayToPosition(path);
        }
        StartCoroutine(DoLerp(currentArray));
        if(new Vector2(transform.position.x,transform.position.y) == targetPos){   
            path = level.GetArrayFromDirection(currentArray, lastInput);
            if(level.GetArrayNode(path).isWalkable){
                currentArray = path;
                currentinput = lastInput;
                RotateSprite();
                if(travelPercent == 0){
                    animator.enabled = false;
                    walkSound.Stop();
                }else{
                    animator.enabled = true;
                    walkSound.PlayOneShot(walkSound.clip);
                }
            }
            else{
                path = level.GetArrayFromDirection(currentArray, currentinput);
                if(level.GetArrayNode(path).isWalkable){
                    currentArray=path;
                    
                }
            }           
        }
        
    }
    IEnumerator DoLerp(Vector2Int array){

        targetPos = level.ArrayToPosition(array);
        travelPercent = 0f;

        if(travelPercent < 1f && new Vector2(transform.position.x,transform.position.y) != targetPos)
        {
            travelPercent += Time.deltaTime * moveSpeed;
            transform.position = Vector2.Lerp(
                new Vector2(transform.position.x,transform.position.y), 
                targetPos, travelPercent / lerpDuration);
                yield return new WaitForEndOfFrame();
        }
        
    }
    void RotateSprite(){
        if(lastInput == "a"){
            transform.localEulerAngles = new Vector3(1,1,180);
        }else if (lastInput == "d"){
            transform.localEulerAngles = new Vector3(0,0,0);
        }else if (lastInput == "w"){
            transform.localEulerAngles = new Vector3(0,0,90);
        }else if (lastInput == "s"){
            transform.localEulerAngles = new Vector3(0,0,-90);
        }
    }
    bool PlayerInput(){
        if(Input.GetKeyDown(KeyCode.W)){
             lastInput = "w";
             return true;
        }
        if(Input.GetKeyDown(KeyCode.A)){
             lastInput = "a";
             return true;
        }
        if(Input.GetKeyDown(KeyCode.S)){
             lastInput = "s";
             return true;
        }
        if(Input.GetKeyDown(KeyCode.D)){
             lastInput = "d";
             return true;
        }
        return false;
    }

}
