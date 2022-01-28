using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunLogic : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
     [SerializeField] Text ammoText;
    [SerializeField] AudioClip pistolShot;
    [SerializeField] AudioClip pistolEmpty;
    [SerializeField] AudioClip pistolReload;



    bool isEquipped = false;

    AudioSource audioSource;
    Rigidbody rigidbody;
    Collider collider;

    const float MAX_COOLDOWN = 0.5f;
    const int MAX_AMMO = 10;
    float currentCoolDown = 0.0f;
    int ammoCount = MAX_AMMO;
    // Start is called before the first frame update
    void Start()
    {
        SetAmmoText();
         audioSource = GetComponent<AudioSource>();
          rigidbody = GetComponent<Rigidbody>();
          collider = GetComponent<Collider>();
    }
    void SetAmmoText()
    {
        if (ammoText)
        {
            ammoText.text = "Ammo : " + ammoCount;
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (!isEquipped)
            return;

        if (currentCoolDown > 0.0f)
        {
            currentCoolDown -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Fire1") && currentCoolDown <= 0.0f)
        {
            if (ammoCount > 0)
            {
                if (bulletPrefab && bulletSpawnPoint)
                    Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation * bulletPrefab.transform.rotation);
                currentCoolDown = MAX_COOLDOWN;
                --ammoCount;
                SetAmmoText();
                PlayerSound(pistolShot);
            }
            else
            {
                PlayerSound(pistolEmpty);
            }
        }
    }
    public void RefillAmmo()
    {
        PlayerSound(pistolReload);
        ammoCount = MAX_AMMO;
        SetAmmoText();
    }
    void PlayerSound(AudioClip sound)
    {
        if (audioSource && sound)
        {
            audioSource.PlayOneShot(sound);
        }
    }
    public void EquippGun()
    {
        isEquipped = true;
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().useGravity = false;
        }
        if (GetComponent<Collider>())
        {
            GetComponent<Collider>().enabled = false;
        }
    }
    public void UnEquippGun()
    {
        isEquipped = false;
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().useGravity = true;
        }
        if (GetComponent<Collider>())
        {
            GetComponent<Collider>().enabled = true;
        }
    }
}
