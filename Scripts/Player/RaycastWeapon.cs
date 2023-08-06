using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialVelocity;
        public Vector3 initialPosition;
        public TrailRenderer tracer;
    }

    public bool isFiring;
    public int fireRate = 25; // 25 bullets per second or frame
    public float bulletSpeed = 1000f;
    public float bulletDrop = 0.0f;
    public float damage = 10f;
    public int ammaCount;
    public int clipSize;
    public ParticleSystem muzzelFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;
    public Transform raycastOrigin;
    public Transform crossHairTarget;
    public LayerMask layerMask;

    public WeaponRecoil weaponRecoil;
    public GameObject weaponItem;

    float accumulatedTime;
    float maxLifeTime = 3.0f;
    Ray ray;
    RaycastHit hitInfo;

    List<Bullet> bullets = new List<Bullet>();

    private void Awake()
    {
        weaponRecoil = GetComponent<WeaponRecoil>();
        weaponItem.GetComponent<Rigidbody>().isKinematic = true;
        weaponItem.GetComponent<BoxCollider>().enabled = false;
    }

    Vector3 GetPosition(Bullet bullet)
    {
        // producing bulletdrop using projectile motion equation
        // p + v*t + 0.5*g*t*t
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);

        return bullet;
    }

    public void StartFiring()
    {
        isFiring = true;
        FireBullet();
    }

    public void UpdateFiring(float deltaTime)
    {
        accumulatedTime += deltaTime;
        float fireInterval = 1.0f / fireRate;

        while (accumulatedTime>=0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltaTime)
    {
        SimulateBullets(Time.deltaTime);
        DestroyBullets();
    }
    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time > maxLifeTime);
    }

    void SimulateBullets(float deltaTime)
    {
        bullets.ForEach(bullet =>
        {
            Vector3 p0 = GetPosition(bullet); // current position or start point of bullet
            bullet.time += deltaTime;
            Vector3 p1 = GetPosition(bullet); // updated or end position of bullet
            RaycastSegment(p0, p1, bullet);
        });
    }
    void RaycastSegment(Vector3 start, Vector3 end,Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        ray.origin = start;
        ray.direction = direction;

        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.green, 1.0f);
            hitEffect.transform.position = crossHairTarget.position;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = crossHairTarget.position;
            bullet.tracer.transform.forward = hitInfo.normal;
            bullet.time = maxLifeTime;

            // Adding force impact to bullet when hit with objects if they have rigidbody component attached to them
            var rb = hitInfo.collider.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddForceAtPosition(ray.direction * 20, crossHairTarget.position, ForceMode.Impulse);
            }

            var hitBox = hitInfo.collider.GetComponent<HitBox>();
            if (hitBox)
            {
                hitBox.OnRaycastHit(this, ray.direction);
            }
        }
        else
        {
            bullet.tracer.transform.position = end;
        }
    }

    private void FireBullet()
    {
        if (ammaCount <= 0)
        {
            return;
        }
        else
        {
            ammaCount--;
        }
        muzzelFlash.Emit(1);

        Vector3 velocity = (raycastOrigin.forward).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);

        weaponRecoil.GenerateRecoil();

        //ray.origin = raycastOrigin.position;
        //ray.direction = /*crossHairTarget.position -*/ raycastOrigin.forward;

        //var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
        //tracer.AddPosition(ray.origin);

        //if (Physics.Raycast(ray, out hitInfo))
        //{
        //    //Debug.DrawLine(ray.origin, hitInfo.point, Color.green, 1.0f);
        //    hitEffect.transform.position = crossHairTarget.position;
        //    hitEffect.transform.forward = hitInfo.normal;
        //    hitEffect.Emit(1);

        //    tracer.transform.position = crossHairTarget.position;
        //    tracer.transform.forward = hitInfo.normal;
        //}
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
