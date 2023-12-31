using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NativeWebSocket;
using UnityEngine.UI;
using System;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;


public class ws_manager : MonoBehaviour
{
    private WebSocket ws;
    private Dictionary<string, GameObject> joinedPlayers = new Dictionary<string, GameObject>();
    private Queue<Vector3> playerQueue = new Queue<Vector3>();
    private Queue<Vector3> enemyQueue = new Queue<Vector3>();
    private Queue<float> enemyQuater = new Queue<float>();
    private Queue<string> enemyId = new Queue<string>();
    public GameObject player;
    public GameObject enemy;
    public GameObject bullet;
    public Slider healthBar;
    public Slider magazineBar;
    private bool joined;
    Weapon mygun;
    Guid id;

    // Start is called before the first frame update
    async void Start()
    {
        string addres = join.serverAddres;
        ws = new WebSocket("ws://" + addres + "/ws/chat/room1/");
        joined = false;
        // サーバーに接続
        ws.OnOpen += () =>
        {
            id = Guid.NewGuid();
            Debug.Log("WebSocket Open");
            Dictionary<String, String> message
                = new Dictionary<string, string>(){{"method", "join"}, {"name", join.playerName}, {"id", id.ToString()}};
            ws.SendText(JsonConvert.SerializeObject(message));
        };
        // サーバから何かを受信したとき
        ws.OnMessage += (bytes) =>
        {
            var message_in = System.Text.Encoding.UTF8.GetString(bytes);
            JObject message = JObject.Parse(message_in);
            // 誰かが参加した
            if(message["method"].ToString().Equals("join"))
            {
                float x = float.Parse(message["data"]["pos"]["pos_x"].ToString());
                float y = 1;
                float z = float.Parse(message["data"]["pos"]["pos_z"].ToString());
                JArray players = (JArray)message["data"]["players"];
                if(message["id"].ToString().Equals(id.ToString())){
                    Debug.Log("you joined!");
                    playerQueue.Enqueue(new Vector3(x, y, z));
                    for(int i = 0; i < players.Count(); i++)
                    {
                        float tmpx = float.Parse(message["data"]["joined"][players[i].ToString()][0].ToString());
                        float tmpy = float.Parse(message["data"]["joined"][players[i].ToString()][1].ToString());
                        float tmpz = float.Parse(message["data"]["joined"][players[i].ToString()][2].ToString());
                        float rotatey = float.Parse(message["data"]["joined"][players[i].ToString()][4].ToString());
                        enemyQueue.Enqueue(new Vector3(tmpx, tmpy, tmpz));
                        enemyQuater.Enqueue(rotatey);
                        Debug.Log(players[i].ToString() + "was alredy joined!");
                        enemyId.Enqueue(players[i].ToString());
                    }
                    
                }
                else{
                    Debug.Log("a new player joined!");
                    enemyQueue.Enqueue(new Vector3(x, y, z));
                    enemyQuater.Enqueue(0f);
                    enemyId.Enqueue(message["id"].ToString());
                }
            }
            // 誰かの座標の更新
            if(message["method"].ToString().Equals("updata"))
            {
                if(!message["id"].ToString().Equals(id.ToString()))
                {
                    string eid = message["id"].ToString();
                    if(joinedPlayers.ContainsKey(eid))
                    {
                        JArray pos_rot = (JArray)message["data"];
                        float tmpx = float.Parse(pos_rot[0].ToString());
                        float tmpy = float.Parse(pos_rot[1].ToString());
                        float tmpz = float.Parse(pos_rot[2].ToString());
                        float rotatey = float.Parse(pos_rot[4].ToString());
                        joinedPlayers[eid].transform.position = new Vector3(tmpx, tmpy, tmpz);
                        joinedPlayers[eid].transform.localEulerAngles = new Vector3(0, rotatey, 0);
                    }
                }
            }
            // 誰かがゲームから抜けた
            if(message["method"].ToString().Equals("leave"))
            {
                string leaveId = message["id"].ToString();
                Destroy(joinedPlayers[leaveId]);
                joinedPlayers.Remove(leaveId);

            }
            // 誰かが弾を撃った
            if(message["method"].ToString().Equals("shot"))
            {
                string shoter_id = message["id"].ToString();
                Vector3 pos = new Vector3(
                    float.Parse(message["data"]["pos_x"].ToString()),
                    float.Parse(message["data"]["pos_y"].ToString()),
                    float.Parse(message["data"]["pos_z"].ToString())
                );
                string gun_name = message["data"]["gun_name"].ToString();
                GameObject gun = (GameObject)Resources.Load("weapons/" + gun_name);
                float rotate_y = float.Parse(message["data"]["rotate_y"].ToString());
                gun.GetComponent<Weapon>().enemyShot(pos, rotate_y, joinedPlayers[shoter_id]);
                

            }
            // 誰かが死んだ
            if(message["method"].ToString().Equals("dead"))
            {
                Debug.Log("someone dead!");
                string dead_id = message["id"].ToString();
                GameObject dead = joinedPlayers[dead_id];
                Destroy(joinedPlayers[dead_id]);
                joinedPlayers.Remove(dead_id);
            }
        };

        ws.OnClose += (e) =>
        {
            Debug.Log("WebSocket Close");
        };
        // InvokeRepeating("SendWebSocketMessage", 0.0f, Time.deltaTime);
        await ws.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        
        // 参加する
        if(playerQueue.Count > 0)
        {
            GameObject tmp = Instantiate(player, playerQueue.Dequeue(), Quaternion.identity);
            joinedPlayers.Add(id.ToString(), tmp);
            player = joinedPlayers[id.ToString()];
            joined = true;
            mygun = player.GetComponent<Player>().weapon.GetComponent<Weapon>();
            mygun.init();
        }
        if(enemyQueue.Count > 0)
        {
            GameObject tmp = Instantiate(enemy, enemyQueue.Dequeue(), Quaternion.Euler(0, enemyQuater.Dequeue(), 0));
            joinedPlayers.Add(enemyId.Dequeue(), tmp);
        }
        #if !UNITY_WEBGL || UNITY_EDITOR
            ws.DispatchMessageQueue();
        #endif
        
        if(joined)
        {
            // 自分の位置更新
            Dictionary<String, String> updataMessage = new Dictionary<string, string>()
            {
                {"method", "updata"}, 
                {"id", id.ToString()},
                {"pos_x" , player.transform.position.x.ToString()},
                {"pos_y" , player.transform.position.y.ToString()},
                {"pos_z" , player.transform.position.z.ToString()},
                {"rotate_y" , player.transform.localEulerAngles.y.ToString()},
            };
            ws.SendText(JsonConvert.SerializeObject(updataMessage));

            mygun.MainUpdate();
            healthBar.value = (float)player.GetComponent<Player>().health / player.GetComponent<Player>().maxHealth;
            if(mygun.reloading)
            {
                magazineBar.value = (float)(mygun.maxRelodeTime-mygun.reloadTime)/mygun.maxRelodeTime;

            }
            else
            {
                magazineBar.value =(float)mygun.magazine/mygun.maxMagazine;
            }
            
            //player.GetComponent<Player>().health;
        
            // 自分が弾を撃つ
            if(Input.GetMouseButton(0) && mygun.CanShot())
            {
                Vector3 pos = player.transform.position + player.transform.forward * 3.0f;
                float rotate_y = mygun.Shot(player.transform.localEulerAngles.y);
                player.transform.localEulerAngles = new Vector3(0, rotate_y, 0);
                Dictionary<String, String> shotMessage = new Dictionary<string, string>()
                {
                    {"method", "shot"}, 
                    {"id", id.ToString()},
                    {"pos_x" , pos.x.ToString()},
                    {"pos_y" , pos.y.ToString()},
                    {"pos_z" , pos.z.ToString()},
                    {"rotate_y" , rotate_y.ToString()},
                    {"gun_name", mygun.gunName}
                };
                ws.SendText(JsonConvert.SerializeObject(shotMessage));
            }
            if(Input.GetMouseButtonDown(1))
            {
                player.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0, 0.44f, 1.57f);
                mygun.ADS = true;
                player.GetComponent<Player>()._speed = 2f;
            }
            if(Input.GetMouseButtonUp(1)){
                player.transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0, 1, -2.4f);
                mygun.ADS = false;
                player.GetComponent<Player>()._speed = 10f;
            }
            // 自分が死んだ
            if(joinedPlayers[id.ToString()].GetComponent<Player>().dead){
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadSceneAsync("Gameover");
                Dictionary<String, String> deadMessage = new Dictionary<string, string>()
                {
                    {"method", "dead"}, 
                    {"id", id.ToString()},
                };
                ws.SendText(JsonConvert.SerializeObject(deadMessage));
            }
        }
    }

    // async void SendWebSocketMessage()
    // {
    //     if (ws.State == WebSocketState.Open)
    //     {
    //         // Sending plain text
           
    //     }
    // }

     void OnDestroy() {
        joined = false;
        Dictionary<String, String> message = new Dictionary<string, string>()
            {
                {"method", "leave"}, 
                {"id", id.ToString()},
            };
            ws.SendText(JsonConvert.SerializeObject(message));
        ws.Close();
        ws = null;    
    }
}

