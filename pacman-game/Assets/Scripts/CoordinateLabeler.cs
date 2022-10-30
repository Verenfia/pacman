using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoordinateLabeler : MonoBehaviour
{
    TextMeshProUGUI label;
    LevelGenerator level;
    private void Awake() {
        label = GetComponent<TextMeshProUGUI>();
        level = FindObjectOfType<LevelGenerator>();

    }

    public void DisplayCoordinates() 
    {
        if(level == null) { return; }

        label.text = "";//transform.parent.parent.name;
    }
    // Start is called before the first frame update
    void Start()
    {
        DisplayCoordinates();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
