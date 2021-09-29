using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;
    private float timeToAppear = 2f;
    private float timeWhenDisappear;
    public string notification = "";

    [Header("Panels")]
    [SerializeField] GameObject UI_Alive;
    [SerializeField] GameObject UI_Death;
    [SerializeField] GameObject UI_End;

    [Header("HUD")]
    //public TextMeshProUGUI AmmoCountText;
    public TextMeshProUGUI HPText;
    public Transform HPBar;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rttText;
    public TextMeshProUGUI grenadesText;
    public TextMeshProUGUI notificationsText;

    [Header("Death screen")]
    public TextMeshProUGUI deathCount;
    public TextMeshProUGUI endText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
            return;
        }
        HideUI();
    }

    public void Update()
    {
        updateRtt(Client.instance.ping.ToString());
        if (notification != "" && (Time.time >= timeWhenDisappear))
        {
            notification = "";
            AddNotification(notification);
        }
    }

    public void HideUI()
    {
        UI_Alive.SetActive(false);
        UI_Death.SetActive(false);
        UI_End.SetActive(false);
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        Client.instance.ConnectToServer();

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Debug.Log("Change scene!!!");
    }

    public void ChangePlayerState(bool isAlive, int deaths)
    {
        //Debug.Log("isAlive: " + isAlive.ToString());
        UI_Alive.SetActive(isAlive);
        UI_Death.SetActive(!isAlive);

        //Update stats ui
        if (!isAlive)
        {
            //deathCount.text = "Deaths: " + localPlayer.Deaths;
            deathCount.text = "Deaths: " + deaths;
        }
    }

    public void UpdateHP(float currentHP, float maxHP)
    {

        float curHPPerc = (float)currentHP / (float)maxHP;
        HPBar.localScale = new Vector3(curHPPerc, 1, 1);


        HPText.text = currentHP.ToString("0") + "/" + maxHP.ToString("0");
    }

    public void EndUI(int num_all_killed)
    {
        Debug.Log("EndUI");
        UI_Alive.SetActive(false);
        UI_Death.SetActive(false);
        UI_End.SetActive(true);
        endText.text = "You beat all the bots.";
        Window_Confetti_End.startConfetti = true;
        StartCoroutine(EndForAll());
    }

    public void updateRtt(string myRtt)
    {
        rttText.text = "RTT: " + myRtt + "ms";
    }

    public void UpdateGrenades(int count)
    {
        grenadesText.text = "Grenades: " + count.ToString();
    }

    public void AddNotification(string message)
    {
        notification = message;
        notificationsText.text = message;
        timeWhenDisappear = Time.time + timeToAppear;
    }

    private IEnumerator EndForAll()
    {
        yield return new WaitForSeconds(10f);
        Application.Quit();

    }
}
