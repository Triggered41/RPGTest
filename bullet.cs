using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Requests;
using Sfs2X.Entities.Variables;

public class bullet : MonoBehaviour
{
    private SmartFox sfs = SFSConnection.Instance.sfs;
    public Rigidbody2D rb;
    public float force = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(transform.right*force, ForceMode2D.Impulse);
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            List<UserVariable> vars = new List<UserVariable>();
            UserVariable HealthVar = sfs.MySelf.GetVariable("Health");
            int hp = HealthVar.GetIntValue();
            hp -= 10; 
            vars.Add(new SFSUserVariable("Health", hp));
            sfs.Send(new SetUserVariablesRequest(vars));

            if (hp <= 0)
            {
                Debug.Log("Dead");
                Destroy(col.gameObject);
            }

        }
        Destroy(gameObject);
    }
}
