using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMove : PlayerController2
{

    private float trailSpeed = 100f;

    private const float UP = 0f;
    private const float DOWN = 1f;
    private const float LEFT = 2f;
    private const float RIGHT = 3f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.Translate(Vector3.right * Time.deltaTime * trailSpeed);
        Destroy(gameObject, 1f);
    }
}
