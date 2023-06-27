using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    [SerializeField] private Vector3 shift;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //transform.position = new 
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - 5);
    }
}
