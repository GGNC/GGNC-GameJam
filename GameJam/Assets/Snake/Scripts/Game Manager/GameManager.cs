using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Grass_Prefab, Tomato_Prefab, UI, End_Screen, Cabbage_Prefab, Play_Screen;
    private GameObject food;
    [SerializeField]
    private Transform Grass_Transform,Tomato_Transform,Cabbage_Transform;
    [SerializeField]
    private TextMeshProUGUI Score_Text, High_Score_Text,End_Score_Text,End_High_Score_Text;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Animator Animation;
    public int Score;
    void Start()
    {
        CreateMeal();
        Time.timeScale = 1;
        Animation.SetBool("Game_Start", false);
        Play_Screen.SetActive(true);
        UI.SetActive(false);
        End_Screen.SetActive(false);
        Score = 0;
    }
    void Update()
    {
        Score_Text.text = Score.ToString();
        End_Score_Text.text = Score.ToString();
        if (Score>PlayerPrefs.GetInt("High_Score",0))
        {
            PlayerPrefs.SetInt("High_Score", Score);
            High_Score_Text.text = PlayerPrefs.GetInt("High_Score").ToString();
        }
        else
        {
            High_Score_Text.text = PlayerPrefs.GetInt("High_Score").ToString();
        }
    }
    public void CreateMeal()
    {
        Vector3 Grass_Position = new Vector3(Random.Range(-9, 10),(float) 1, Random.Range(-9, 10));
        Vector3 Tomato_Position = new Vector3(Random.Range(-9, 10), (float)1.25, Random.Range(-9, 10));
        Vector3 Cabbage_Position = new Vector3(Random.Range(-9, 10), (float)1.25, Random.Range(-9, 10));
        float n = Random.Range(1,11);
        if (n == 1)
        {
            food = (GameObject)Instantiate(Cabbage_Prefab, Tomato_Position, Cabbage_Transform.rotation);
            food.tag = "Cabbage";
        }
        else if (n==2||n==3)
        { 
            food = (GameObject)Instantiate(Tomato_Prefab, Tomato_Position, Tomato_Transform.rotation);
            food.tag = "Tomato";
        }
        else
        {
            food = (GameObject)Instantiate(Grass_Prefab, Grass_Position, Grass_Transform.rotation);
            food.tag="Grass";
        }
        if((Grass_Position.x==player.Player_Position.x && Grass_Position.z==player.Player_Position.y)||(Tomato_Position.x == player.Player_Position.x && Tomato_Position.z == player.Player_Position.y)|| (Cabbage_Position.x == player.Player_Position.x && Cabbage_Position.z == player.Player_Position.y))
        {
            Destroy(food);
            CreateMeal();
        }
            
    }
    public void End()
    {
        Time.timeScale = 0;
        if (Score > PlayerPrefs.GetInt("High_Score", 0))
        {
            PlayerPrefs.SetInt("High_Score", Score);
        }
        End_High_Score_Text.text = PlayerPrefs.GetInt("High_Score", 0).ToString();
        Play_Screen.SetActive(false);
        UI.SetActive(false);
        End_Screen.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }
    public void Start_Game()
    {
        player.Game_Start = true;
        Play_Screen.SetActive(false);
        UI.SetActive(true);
        End_Screen.SetActive(false);
        Animation.SetBool("Game_Start", true);
    }
}
