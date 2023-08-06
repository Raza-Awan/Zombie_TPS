using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public CharacterAiming characterAiming;
    public Animator rigController;

    Cinemachine.CinemachineImpulseSource cameraShake;

    public float duration;

    float verticalRecoil;
    float horizontalRecoil;
    float time;

    private void Awake()
    {
        cameraShake = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }

    public void GenerateRecoil()
    {
        time = duration;

        cameraShake.GenerateImpulse(Camera.main.transform.forward);

        horizontalRecoil = Random.Range(10, 20);
        verticalRecoil = Random.Range(40,80);

        rigController.Play("weapon_recoil", 0, 0.0f);
    }

    void Update()
    {
        if (time > 0)
        {
            //characterAiming.xAxis.Value -= ((horizontalRecoil / 100) * Time.deltaTime) / duration;
            //characterAiming.yAxis.Value -= ((verticalRecoil / 100) * Time.deltaTime) / duration;
            time -= Time.deltaTime;
        }
    }
}
