using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pacStudent : MonoBehaviour
{
    public float speed = 0.0001f;
    public float reachThreshold = 0.1f;
    void Start()
    {
        StartCoroutine(MoveBject());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator MoveBject()
    {
        float distance = Vector3.Distance(transform.position, new Vector3(10, 0));

        while (distance > reachThreshold)
        {
            // 2 - Movement
            Vector3 movement = new Vector3(
                0,
                0,
                speed * 10);

            //movement *= Time.deltaTime;
            transform.Translate(movement);

            //Wait a frame
            yield return null;
        }
    }
}
