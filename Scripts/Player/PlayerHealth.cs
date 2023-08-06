using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public PlayerHealthBar playerHealthBar;
    public CameraManager cameraManager;
    public GameObject weaponItem;

    Ragdoll ragdoll;
    CharacterAiming characterAiming;

    Vector3 direction;

    private void Start()
    {
        characterAiming = GetComponent<CharacterAiming>();
        ragdoll = GetComponent<Ragdoll>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        playerHealthBar.SetMaxHealth(maxHealth);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        playerHealthBar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            ragdoll.ActivateRagdoll();
            direction.y = 1f;
            ragdoll.ApplyForce(direction);
            characterAiming.enabled = false;
            weaponItem.GetComponent<Rigidbody>().isKinematic = false;
            weaponItem.GetComponent<BoxCollider>().enabled = true;
            cameraManager.EnableKillCam();
        }
    }
}
