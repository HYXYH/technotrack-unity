using UnityEngine;

[System.Serializable]
public class WeaponHolder
{
    [SerializeField] 
    private GameObject _barrel;

    [SerializeField]
    public GameObject[] WeaponPrefabs;
    private Weapon[] _weapons;
    [SerializeField] 
    private int _weaponId = 0;

    public GameObject GetBarrel()
    {
        return _barrel;
    }

    public void InitWeapons(string ownerName)
    {
        foreach (var weapon in _weapons)
        {
            weapon.SetOwnerName(ownerName);
        }
    }

    public void SetWeapons(Weapon[] weapons)
    {
        _weapons = weapons;
    }

    public void SetWeaponId(int weaponId)
    {
        if (weaponId < _weapons.Length)
        {
            _weaponId = weaponId;
        }
        else
        {
            Debug.Log("Weapon not found!");
        }
    }

    public Weapon GetWeapon()
    {
        return _weapons[_weaponId];
    }
	

    public void Fire()
    {
        if (_weaponId == -1 || _weaponId >= _weapons.Length)
        {
            Debug.Log("Weapon not installed!");
            return;
        }
        _weapons[_weaponId].Fire();
    }
}