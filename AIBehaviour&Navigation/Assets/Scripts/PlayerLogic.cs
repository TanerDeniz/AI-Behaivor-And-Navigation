using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{
     CharacterController characterController;
     float movementSpeed = 5.0f;
    
     float horizontalInput;
     float verticalInput;
    int playerHealth = 100;
     float jumpHeight = 0.6f;
     float gravity = 0.05f;

     bool jump = false;
     Vector3 movement;
     Vector3 movementInput;

    GameObject interactiveObject = null;
    GameObject equippedObject=null;
    GunLogic gunLogic;

    [SerializeField] Transform weaponEquippmentPosition;
    [SerializeField] Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        SetHealthText();
    }

    void SetHealthText()
    {
        if (healthText)
        {
            healthText.text = "Health : " + playerHealth; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        movementInput = new Vector3(horizontalInput, 0, verticalInput);
        if (!jump && Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        if (interactiveObject && Input.GetButtonDown("Fire2"))
        {
            if (!equippedObject)
            {
                gunLogic = interactiveObject.GetComponent<GunLogic>();
                if (gunLogic)
                {
                    interactiveObject.transform.position = weaponEquippmentPosition.position;
                    interactiveObject.transform.rotation = weaponEquippmentPosition.rotation;
                    interactiveObject.transform.parent = gameObject.transform;
                   gunLogic.EquippGun();
                    equippedObject = interactiveObject;
                }
            }
            else if (equippedObject)
            {
                gunLogic = equippedObject.GetComponent<GunLogic>();
                if (gunLogic)
                {
                    equippedObject.transform.parent = null;
                    gunLogic.UnEquippGun();
                   equippedObject = null;
                }
            }
        }
    }
    private void FixedUpdate()
    {

        movement = movementInput * movementSpeed * Time.deltaTime;
        RotateCharacterTowardsMouseCursor();
      /*  if (movementInput != Vector3.zero) 
             transform.forward = Quaternion.Euler(0, -90, 0) * movementInput.normalized;*/
        if (!characterController.isGrounded)
        { 
            if (movement.y > 0)
            {
                movement.y -= gravity;
            }
            else
            {
                movement.y -= gravity * 1.5f;
            }
        }
        else
        {
            movement.y = 0;
        }
        if (jump)
        {
            movement.y = jumpHeight;
            jump = false;
        }
        if (characterController)
        {
            characterController.Move(movement);
        }
    }
    void RotateCharacterTowardsMouseCursor()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 direction = mousePos - playerPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            interactiveObject = other.gameObject;
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon" && interactiveObject==other.gameObject)
        {
            interactiveObject = null;
        }
    }
    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene(0);
        }
        SetHealthText();
    }
}
