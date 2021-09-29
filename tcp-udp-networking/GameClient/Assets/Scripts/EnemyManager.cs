using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public int id;
    public float health;
    public float maxHealth = 100f;

    public Image healthBarImage;
    public Transform healthBarPivot;

    public Transform weaponMuzzle;

    [Header("Audio & Visual")]
    public GameObject muzzleFlashPrefab;
    public AudioClip shootSFX;
    public GameObject bulletSparkPrefab;

    public void Initialize(int _id)
    {
        id = _id;
        health = maxHealth;
        GetComponent<AudioSource>().volume = Random.Range(0.8f, 1);
        GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.1f);
        GetComponent<AudioSource>().Play();
    }

    public void SetHealth(float _health)
    {
        health = _health;
        float amt = (float)health / (float)maxHealth;
        //Debug.Log("bot health = " + health);
        //Debug.Log("bot amt = " + amt);
        healthBarImage.fillAmount = amt;
        healthBarPivot.LookAt(Camera.main.transform.position);

        if (health <= 0f)
        {
            //enemy die VFX
            GameManager.enemies.Remove(id);
            Destroy(gameObject);
        }
    }

    public void EnemyShootFX(int _id, Vector3 _shoot_orig, Vector3 _shoot_dir, string _shot_obj, int _shot_id)
    {
        GameObject muzzleFlashInstance = Instantiate(muzzleFlashPrefab, weaponMuzzle.position, weaponMuzzle.rotation, weaponMuzzle.transform) as GameObject;
        Destroy(muzzleFlashInstance, 2f);
        if (Physics.Raycast(_shoot_orig, _shoot_dir, out RaycastHit _hit, 5f))
        {
            Vector3 impactPos = _hit.point;
            Vector3 impactRot = _hit.normal;
            GameObject impactVFXInstance = Instantiate(bulletSparkPrefab, impactPos + (impactRot * 0.1f), Quaternion.LookRotation(impactRot));
            Destroy(impactVFXInstance.gameObject, 5f);

            string shooter = "EnemyBot";
            string shotPlayer = GameManager.players[_shot_id].username;
            string message = shooter + " shot player " + shotPlayer;
            UIManager.instance.AddNotification(message);
        }

    }
}
