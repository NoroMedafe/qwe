using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBall : MonoBehaviour {

    [SerializeField] private Rigidbody rigidbody;
    protected Vector3 Startpos;
    protected bool ResetIt;

    // Use this for initialization
    void Start()
    {
        Startpos = transform.position;
    }

    // Update is called once per frame
    void Update () {
        if (transform.position.y < -0.1f)
        {
            transform.position = new Vector3(0, 0.08f, 0);
            rigidbody.velocity = Vector3.zero;
        }

        if (ResetIt)
        {
            ResetIt = false;
            transform.position = Startpos;
            rigidbody.velocity = Vector3.zero;
        }
        if (rigidbody.velocity.magnitude <0.4)
        {
            rigidbody.velocity = Vector3.zero;
        }
        if (transform.position.y > 0.083f)
        {
            transform.position = new Vector3(transform.position.x, 0.076f, transform.position.z);
        }
    }
    public void ResetBall()
    {
        ResetIt = true;
    }
}
