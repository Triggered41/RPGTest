using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sfs2X;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Variables;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;

public class SFSConnection : MonoBehaviour
{
    UserVariable pos = new SFSUserVariable("pos", null);
    public static SFSConnection Instance {get; private set;}
    public int port = 9933;
    public string ip = "127.0.0.1"; 
    public SmartFox sfs;
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        ConfigData cfg = new ConfigData();
        cfg.Host = ip;
        cfg.Port = port;
        cfg.Zone = "testZone";
        // cfg.HttpPort = httpPort;
        // cfg.HttpsPort = httpsPort;
        // cfg.BlueBox.IsActive = useHttpTunnel;
        // cfg.BlueBox.UseHttps = encrypt;
        // cfg.Debug = debug;
        
        sfs = new SmartFox();
        sfs.ThreadSafeMode = true;
        sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        sfs.AddEventListener(SFSEvent.UDP_INIT, OnUDPInit);
        sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
        sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
        sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
        sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomError);
        sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnter);
        sfs.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnUserVarUpdate);

        sfs.Connect(cfg);
        
    }
    void OnRoomJoin(BaseEvent ev)
    {
        List<UserVariable> userVars = new List<UserVariable>();
        userVars.Add(new SFSUserVariable("Health", 100));
        sfs.Send(new SetUserVariablesRequest(userVars));
        Debug.Log("Room Joined");
        SceneManager.LoadScene("Game");
    }
    void OnRoomError(BaseEvent ev)
    {
        Debug.Log("Room Join Erros:");
        foreach (var item in ev.Params)
        {
            Debug.Log(item);
        }
    }
    void OnConnection(BaseEvent ev)
    {
        List<UserVariable> userVars = new List<UserVariable>();
        userVars.Add(new SFSUserVariable("x", 100));
        userVars.Add(new SFSUserVariable("y", 50));
        sfs.Send(new SetUserVariablesRequest(userVars));
        if ((bool)ev.Params["success"])
        {        
            Debug.Log("Succesfully Connected");
            return;
        }
        Debug.Log("Connection Failed");
    }

    // Update is called once per frame
    void Update()
    {
        sfs.ProcessEvents();
    }

    void OnApplicationQuit()
    {
        if (sfs.IsConnected)
        {
            sfs.Disconnect();
        }
        Debug.Log("Disconnected Succesfuly");
    }

    public void Login(string name){
        if (!sfs.IsConnected)
        {
            Debug.Log("Not Connected");
            return;  
        } 

        Debug.Log("Logging in...");
        sfs.Send(new LoginRequest(name));
    }

    void OnLogin(BaseEvent ev)
    {
        sfs.InitUDP(ip, port);
        // sfs.InitUDP();
        List<Room> r = sfs.JoinedRooms;
        foreach (var item in r)
        {
            Debug.Log("Room Name: ");
            Debug.Log(item.Name);
        }
        sfs.Send(new JoinRoomRequest("roomy"));
        Debug.Log(r.Count); 
        
    }
    void OnLoginError(BaseEvent ev)
    {
        Debug.Log("Login Failed");
        Debug.Log(ev.Params["errorMessage"]);
        Debug.Log(ev.Params["errorCode"]);
    }

    void OnUDPInit(BaseEvent ev)
    {
        if (sfs.UdpInited){

            Debug.Log("UDP Connected");
        }else{
            Debug.Log("Could Not connect UDP");
            foreach (var item in ev.Params)
            {
                Debug.Log(item);
            }
            sfs.InitUDP(ip, port);

        }
    }

    void OnUserEnter(BaseEvent ev)
    {
        Debug.Log("User Entered");
        Debug.Log(ev.Params["user"].GetType());
        foreach (var item in ev.Params.Keys)
        {
            Debug.Log(item);
        }
    }

    void OnUserVarUpdate(BaseEvent evt)
    {
        // User user = (User)evt.Params["user"];
        // Debug.Log(user.Name + " XPos: " + user.GetVariable("x"));
        // Debug.Log(user.Name + " YPos: " + user.GetVariable("y"));
    }
    
}