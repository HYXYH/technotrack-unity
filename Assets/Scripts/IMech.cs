using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMech
{

    void MoveForward(float controllerValue);

    void RotateBot(float controllerValue);

    void LookAt(Vector3 targetDir);

    Vector3 GetLookDirection();

    void Fire(int holderId);

    void UseWeapon(int holderId, int weaponId);
    
    void UseWeapon(int weaponId);

    void ReceiveDamage(float damage);

    void SetGui(InGameGui inGameGui);
}
