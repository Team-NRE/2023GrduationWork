using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public int moveSpeed;
    public float move;
    public float moveVertical;

    public int rotSpeed;
    public float rotate;
    public float rotHorizon;


    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 5;
        rotSpeed = 120;
    }

    // Update is called once per frame
    void Update()
    {
        move = moveSpeed * Time.deltaTime;
        rotate = rotSpeed * Time.deltaTime;

        moveVertical = Input.GetAxis("Vertical");
        rotHorizon = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * move * moveVertical);
        transform.Rotate(new Vector3(0.0f, rotate*rotHorizon,0.0f));

    }
}
