using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBall : MonoBehaviour {

    [SerializeField] private Rigidbody rigidbody;

    protected Vector3 Startpos;
    protected bool ResetIt;

	// Use this for initialization
	void Start () {
        Startpos = transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (transform.position.y < 0.01f)
            gameObject.SetActive(false);

        if (ResetIt)
        {
            ResetIt = false;
            transform.position = Startpos;
            rigidbody.velocity = Vector3.zero;
        }
        
	}
    private void Update()
    {
        if (transform.position.y > 0.083f)
        {
            transform.position = new Vector3(transform.position.x, 0.076f, transform.position.z);
        }
        if (rigidbody.velocity.magnitude<0.01)
        {
            rigidbody.velocity = Vector3.zero;

        }
    }
    public void ResetBall()
    {
        ResetIt = true;
    }
}
