using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using Sfs2X.Entities.Data;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController Controller;
    public Rigidbody2D rb;
    public GameObject bullet;
    public Transform fp;
    public Camera Cam;
    public Animator Anim;
    public float speed = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        
        Cam = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    public float r = .0f;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ISFSObject obj = new SFSObject();
            SFSConnection.Instance.sfs.Send(new ExtensionRequest("Fire", obj));
            
        }
    }

    public Vector2 Direction = Vector2.zero;
    void FixedUpdate()
    {
        
        Direction.x = Input.GetAxisRaw("Horizontal");
        Direction.y = Input.GetAxisRaw("Vertical");
        
        Vector3 mp =  Cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mp - transform.position;
        r = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(.0f, .0f, r);

        rb.MovePosition(rb.position + Direction.normalized*speed*Time.fixedDeltaTime);
        // rb.AddForce();
        // Controller.Move();
    }

    public void Shoot()
    {
        Debug.Log("Fired");
        GameObject a = Instantiate(bullet, fp.transform.position, fp.transform.rotation);
        Object.Destroy(a, 5.0f);
    }
}
