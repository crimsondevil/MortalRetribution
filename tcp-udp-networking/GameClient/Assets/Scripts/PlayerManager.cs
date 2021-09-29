using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public int deaths = 0;

    public float maxHealth = 100f;
    public int itemCount = 0;
    public MeshRenderer model;
    public SkinnedMeshRenderer animModel;
    public GameObject weapon;
    public bool isAlive = true;
    public bool isWalking = false;
    public Animator animator = null;

    public float pingCountdown = 1f;
    public float pingCountdownLimit = 3f;

    private Vector3 fromPos = Vector3.zero;
    private Vector3 toPos = Vector3.zero;
    private float lastTime;

    //public string actualName;
    //public Color playerColor = Color.white;
    [SerializeField] public TextMesh nameUI;
    [SerializeField] public GameObject floatingInfo;
    public Transform weaponMuzzle;

    [Header("Audio & Visual")]
    public AudioSource audioSource;
    public AudioClip footstepSFX;
    //public AudioClip jumpSFX;
    public AudioClip launcherSFX;
    public GameObject muzzleFlashPrefab;
    public AudioClip shootSFX;
    public GameObject bulletHolePrefab;
    public GameObject bulletBloodFxPrefab;
    public GameObject bulletDirtFxPrefab;
    public GameObject bulletSparkPrefab;



    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
        if (_id == Client.instance.myId)
        {
            UIManager.instance.ChangePlayerState(isAlive, deaths);
            UIManager.instance.nameText.text = username;
        }
        nameUI.text = username;
        if (_id != Client.instance.myId)
        {
            Color color2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            nameUI.color = color2;
        }
        if (audioSource)
        {
            audioSource.volume = Random.Range(0.8f, 1);
            audioSource.pitch = Random.Range(0.8f, 1.1f);
        }
    }

    public void SetPosition(Vector3 position)
    {
        fromPos = toPos;
        toPos = position;
        lastTime = Time.time;
    }

    public void FixedUpdate()
    {
        pingCountdown += Time.fixedDeltaTime;
        if (pingCountdown >= pingCountdownLimit)
        {
            pingCountdown = 0;
            // this is where you would update the ping display/variable
            ClientSend.Ping();
        }
        floatingInfo.transform.LookAt(Camera.main.transform);
    }

    private void Update()
    {
        this.transform.position = Vector3.Lerp(fromPos, toPos, (Time.time - lastTime) / (1.0f / Time.fixedDeltaTime));
        if (this.transform.position != toPos && isWalking == false)
        {
            isWalking = true;
            animator.SetBool("isWalking", true);
        }
        else if (this.transform.position == toPos && isWalking == true)
        {
            isWalking = false;
            animator.SetBool("isWalking", false);
        }
        /*
        while (isWalking){

            if (audioSource)
            {
                audioSource.PlayOneShot(footstepSFX);
            }
        }
        */
    }

    public void SetHealth(int _id, float _health)
    {
        health = _health;
        if (_id == Client.instance.myId)
        {
            UIManager.instance.UpdateHP(health, maxHealth);
        }

        if (health <= 0f)
        {
            Die(_id);
        }
    }

    public void Die(int _id)
    {
        //model.enabled = false;
        animModel.enabled = false;
        weapon.SetActive(false);
        deaths += 1;
        isAlive = false;
        if (_id == Client.instance.myId)
        {
            UIManager.instance.ChangePlayerState(isAlive, deaths);
        }
        floatingInfo.SetActive(false);
    }

    public void Respawn(int _id)
    {
        //model.enabled = true;
        animModel.enabled = true;
        weapon.SetActive(true);
        isAlive = true;
        UIManager.instance.ChangePlayerState(isAlive, deaths);
        SetHealth(_id, maxHealth);
        if (_id != Client.instance.myId)
        {
            floatingInfo.SetActive(true);
        }
    }

    public void pickedUpItem(int _id)
    {
        itemCount++;
        if (_id == Client.instance.myId)
        {
            UIManager.instance.UpdateGrenades(itemCount);
        }
    }

    public void droppedItem(int _id)
    {
        if (audioSource)
        {
            audioSource.PlayOneShot(launcherSFX);
        }
        itemCount--;
        if (_id == Client.instance.myId)
        {
            UIManager.instance.UpdateGrenades(itemCount);
        }
    }

    
    public void ShootFX(int _id, Vector3 _shoot_orig, Vector3 _shoot_dir, string _shot_obj, int _shot_id)
    {

            Debug.Log("in player manager if");
            if (audioSource)
            {
                audioSource.PlayOneShot(shootSFX);
            }
            GameObject muzzleFlashInstance = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle.transform) as GameObject;
            Destroy(muzzleFlashInstance, 2f);

            if (Physics.Raycast(_shoot_orig, _shoot_dir, out RaycastHit _hit, 25f))
            {
                Vector3 impactPos = _hit.point;
                Vector3 impactRot = _hit.normal;
                GameObject impactVFXInstance = Instantiate(bulletSparkPrefab, impactPos + (impactRot * 0.1f), Quaternion.LookRotation(impactRot));
                Destroy(impactVFXInstance.gameObject, 5f);
                
                if (_shot_obj == "None")
                {
                    //Instantiate(bulletHolePrefab, impactPos + impactRot * 0.1f, Quaternion.LookRotation(impactRot));
                    Instantiate(bulletDirtFxPrefab, impactPos, Quaternion.LookRotation(impactRot));
                }
                else if(_shot_obj == "Player")
                {
                    //Instantiate(bulletHolePrefab, impactPos + impactRot * 0.1f, Quaternion.LookRotation(impactRot), GameManager.players[_shot_id].transform);
                    Instantiate(bulletBloodFxPrefab, impactPos, Quaternion.LookRotation(impactRot));
                }
                else if (_shot_obj == "Enemy")
                {
                    //Instantiate(bulletHolePrefab, impactPos + impactRot * 0.1f, Quaternion.LookRotation(impactRot), GameManager.enemies[_shot_id].transform);
                    Instantiate(bulletDirtFxPrefab, impactPos, Quaternion.LookRotation(impactRot));
                }
            }


        string shooter = GameManager.players[_id].username;
        string message = "";
        if (_shot_obj == "Player")
        {
            string shotPlayer = GameManager.players[_shot_id].username;
            message = shooter + " shot player " + shotPlayer;
        }
        else if (_shot_obj == "Enemy")
        {
            message = shooter + " shot EnemyBot";
        }
        if (message != "")
        {
            //Debug.Log(message);
            UIManager.instance.AddNotification(message);
        }

    }
}
