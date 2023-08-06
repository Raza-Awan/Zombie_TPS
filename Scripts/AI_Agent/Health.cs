using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [HideInInspector] public float maxHealth;
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float blinkIntensity;
    [HideInInspector] public float blinkDuration;
    [HideInInspector] public float enemiesCount;

    SkinnedMeshRenderer skinnedMeshRenderer;
    UIHealthBar healthBar;

    float blinkTimer;

    void Start()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        healthBar = GetComponentInChildren<UIHealthBar>();

        currentHealth = maxHealth;

        var rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rigidBody in rigidBodies)
        {
            HitBox hitBox = rigidBody.gameObject.AddComponent<HitBox>();
            hitBox.health = this;
            if (hitBox.gameObject != gameObject)
            {
                hitBox.gameObject.layer = LayerMask.NameToLayer("HitBox");
            }
        }

        OnStart();
    }

    private void Update()
    {
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }

    public void TakeDamage(float amount, Vector3 direction)
    {
        currentHealth -= amount;
        healthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        OnDamge(direction);

        if (currentHealth <= 0.0f)
        {
            Die(direction);
        }

        blinkTimer = blinkDuration;
    }

    //public bool IsDead()
    //{
    //    return currentHealth <= 0;
    //}

    void Die(Vector3 direction)
    {
        OnDeath(direction);
    }

    protected virtual void OnStart()
    {

    }
    protected virtual void OnDeath(Vector3 direction)
    {

    }
    protected virtual void OnDamge(Vector3 direction)
    {

    }
}
