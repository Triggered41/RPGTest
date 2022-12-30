using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Entities;
using Sfs2X.Requests;
using Sfs2X.Entities.Variables;
public class GameManager : MonoBehaviour
{
    public float speed = 3.0f;
    public Camera cam;
    public GameObject player;
    private Dictionary<string, GameObject> players  = new Dictionary<string, GameObject>();
    public GameObject playerPrefab;
    SmartFox sfs = SFSConnection.Instance.sfs;
    // Start is called before the first frame update
    void Start()
    {
        players.EnsureCapacity(5);
        players.Add(sfs.MySelf.Name, player);
        sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
        sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtentionResponse);
        sfs.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnUserVarUpdate);
        
        sfs.Send(new ExtensionRequest("PlayersListReq", new SFSObject()));
        
    }
    List<UserVariable> userVars = new List<UserVariable>();
    // Update is called once per frame
    int i = 0;
    void FixedUpdate()
    {
        if (i >= 1)
        {
            i = 0;
            userVars.Clear();
            userVars.Add(new SFSUserVariable("x", (double)player.GetComponent<PlayerMovement>().Direction.normalized.x));
            userVars.Add(new SFSUserVariable("y", (double)player.GetComponent<PlayerMovement>().Direction.normalized.y));
            userVars.Add(new SFSUserVariable("r", (double)player.transform.eulerAngles.z));
            SFSConnection.Instance.sfs.Send(new SetUserVariablesRequest(userVars));  
            if (Input.GetKey(KeyCode.Z))
            {
                foreach (var item in players)
                {   
                    item.Value.transform.position = Vector3.zero;
                }
            }
        }
        else
        {
            i++;
        }
        // Debug.Log("Sent Location");
        // Debug.Log(player.transform.position);

    }
    void OnUserEnterRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];
        Debug.Log("User Joined");
        Debug.Log(user.Name);
        if (user.Name != sfs.MySelf.Name)
        {
            var i = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            players.Add(user.Name, i);
            i.GetComponent<PlayerMovement>().enabled = false;
            i.GetComponentInChildren<NameTextUpdate>().SetName(user.Name);
        }
    }

    void OnExtentionResponse(BaseEvent evt)
    {
        string cmd = (string)evt.Params["cmd"];
        Debug.Log("GM Response: "+ cmd);
        if (cmd == "Fire")
        {
            ISFSObject obj = (SFSObject)evt.Params["params"];
            string Shooter = obj.GetText("Shooter");
            if (players.ContainsKey(Shooter))
            {
                players[Shooter].GetComponent<PlayerMovement>().Shoot();
            }
        }
        if (cmd == "PlayersList")
        {
            Debug.Log("PlayersList Rec.");
            addPlayers(evt);
        }
    }
    Vector2 dir = Vector2.zero;
    void OnUserVarUpdate(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];
        if (user.Name != sfs.MySelf.Name)
        {
            double x = (double)user.GetVariable("x").Value;
            double y = (double)user.GetVariable("y").Value;
            double r = (double)user.GetVariable("r").Value;
            
            dir.x = (float)x;
            dir.y = (float)y;
            
            if (players.ContainsKey(user.Name))
            {
                players[user.Name].transform.eulerAngles = new Vector3(.0f,.0f,(float)r);
                Rigidbody2D rb = players[user.Name].GetComponent<Rigidbody2D>();
                rb.MovePosition(rb.position + dir*speed*Time.fixedDeltaTime);
                // players[user.Name].transform.position = pos;
            }
        }
    }

    void addPlayers(BaseEvent evt)
    {
        ISFSObject playersObj = (SFSObject)evt.Params["params"];
        Debug.Log("PLAYERS LIST: ");
        string[] name = playersObj.GetUtfStringArray("List");
        foreach (var item in name)
        {
            Debug.Log(item);
            if (!players.ContainsKey(item)){
                Debug.Log(item);
                var i = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                players.Add(item, i);
                i.GetComponent<PlayerMovement>().enabled = false;
                i.GetComponentInChildren<NameTextUpdate>().SetName(item);
            }
        } 
    }
}
