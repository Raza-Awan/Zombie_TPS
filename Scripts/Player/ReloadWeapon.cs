using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReloadWeapon : MonoBehaviour
{
    public Animator rigController;
    public WeaponAnimationEvents animationEvents;
    public Transform leftHand;
    public GameObject magzine;
    public RaycastWeapon raycastWeapon;
    public AmmoWidget ammoWidget;
    public CharacterAiming characterAiming;
    public PlayerInput playerInput;

    InputAction reloadGunAction;

    //GameObject magzineHand;

    void Start()
    {
        animationEvents.WeaponAnimationEvent.AddListener(OnAnimationEvent);
        reloadGunAction = playerInput.actions["ReloadGun"];
    }

    void Update()
    {
        if (/*Input.GetKeyDown(KeyCode.R) ||*/ raycastWeapon.ammaCount <= 0 || reloadGunAction.triggered) // reloading
        {
            rigController.SetTrigger("reload_weapon");
            characterAiming.Sound = false;
            characterAiming.gunSound.SetActive(false);
        }

        if (raycastWeapon.isFiring) // done reloading
        {
            ammoWidget.Refresh(raycastWeapon.ammaCount);
            characterAiming.Sound = true;
            characterAiming.gunSound.SetActive(true);
        }
    }

    void OnAnimationEvent(string eventName)
    {
        switch (eventName)
        {
            case "detach_magzine":
                DetachMagzine();
                break;
         //-------------------------------
            case "drop_magzine":
                DropMagzine();
                break;
         //--------------------------------
            case "refill_magzine":
                RefillMagzine();
                break;
         //---------------------------------
            case "attach_magzine":
                AttachMagzine();
                break;
        }
    }

    void DetachMagzine()
    {
        //magzineHand = Instantiate(magzine, leftHand.position,Quaternion.identity);
        //magzine.transform.parent = leftHand.parent;
        //magzine.transform.localRotation = Quaternion.identity;
    }
    void DropMagzine()
    {

    }
    void RefillMagzine()
    {

    }
    void AttachMagzine()
    {
        raycastWeapon.ammaCount = raycastWeapon.clipSize;
        rigController.ResetTrigger("reload_weapon");
        ammoWidget.Refresh(raycastWeapon.ammaCount);
    }
}
