using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//[System.Serializable]
public class LaserGun : Weapon
{
    [SerializeField] private float _maxDistance = 30;
    [SerializeField] private float _damage = 10f;

    [SerializeField] private float _fadeOutTime = 0.15f;

    [SerializeField] private GameObject _laserPrefab;

    [SerializeField] private GameObject _explosionPrefab;

    private GameObject _laser;
    private LineRenderer _lineRenderer;
    private GameObject _explosion = null;
    private float _startFadeOutTime;



    private void Start()
    {
        _laser = Instantiate(_laserPrefab, transform.position, transform.rotation);
        _laser.transform.parent = transform;
        _lineRenderer = _laser.GetComponent<LineRenderer>();
        
        Color c = _lineRenderer.material.GetColor("_TintColor");
        c.a = 0f;
        _lineRenderer.material.SetColor("_TintColor", c);
    }

    public override void Fire()
    {
        if (CheckWeaponReady())
        {
            var ray = new Ray(transform.position, transform.forward * _maxDistance);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    Vector3[] positions = {Vector3.zero, new Vector3(0, 0, hit.distance)};
                    _lineRenderer.SetPositions(positions);
                    var hitName = hit.collider.gameObject.GetInstanceID().ToString();
                    var canDamage = hit.collider.gameObject.CompareTag("Damagable");
                    var notMe = !_ownerName.Equals(hitName);
                    if (notMe && canDamage)
                    {
                        if (_explosionPrefab != null)
                        {
                            _explosion = PoolManager.GetObject(_explosionPrefab.name, hit.point,
                                Quaternion.LookRotation(hit.normal));

                            if (_explosion != null)
                            {
                                _explosion.GetComponent<ParticleSystem>().Play();
                            }
                            else
                            {
                                Debug.Log("_explosionPrefabName not found!");
                            }
                        }

                        hit.collider.gameObject.SendMessage("ReceiveDamage", this._damage);
                    }
                }
            }
            else
            {
                Vector3[] positions = {Vector3.zero, new Vector3(0, 0, _maxDistance)};
                _lineRenderer.SetPositions(positions);
            }


            Color c = _lineRenderer.material.GetColor("_TintColor");
            c.a = 1f;
            _lineRenderer.material.SetColor("_TintColor", c);
            _startFadeOutTime = Time.time;
            StartCoroutine(FadeoutLaser());

            base.Fire();
        }
    }


    private IEnumerator FadeoutLaser()
    {
        Color c = _lineRenderer.material.GetColor("_TintColor");
        while (c.a > 0)
        {
            c.a =  1f - (Time.time - _startFadeOutTime)/_fadeOutTime;
            if (c.a < 0.01)
            {
                c.a = 0;
            }
            _lineRenderer.material.SetColor("_TintColor", c);
            yield return null;
        }
    }
}