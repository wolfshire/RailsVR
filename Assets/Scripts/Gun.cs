using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private static int PICKUP_LAYER = -1;

    private SteamVR_TrackedController _controller;

    private Transform _barrelTransform;
    private LineRenderer _lineRenderer;
    private AudioSource _audioSource;
    private GameObject _muzzleFlash;
    public GameObject _particleSpark;

    private Ray _ray = new Ray();
    private RaycastHit _hit;

    public int Ammo { get { return _ammo; } }
    private int _ammo;
    public int clipSize = 6;
    private GameObject[] _bulletImages;
    private GameObject _bulletUI;
    private GameObject _reloadUI;
    private GameObject _dryUI;
    private GameObject _waitUI;

    private bool _safety = false;

    public float reloadTime = 1;
    private bool _reloading;

    private bool _flashing;

    public AudioClip[] audioFiles;

    void Awake()
    {
        if (PICKUP_LAYER == -1)
            PICKUP_LAYER = LayerMask.NameToLayer("Pickup");
    }

    void Start()
    {
        // Assign
        _controller = transform.GetComponentInParent<SteamVR_TrackedController>();
        _controller.TriggerClicked += TriggerPressed;

        _barrelTransform = transform.Find("Model/Barrel");
        _lineRenderer = _barrelTransform.GetComponent<LineRenderer>();
        _audioSource = transform.GetComponent<AudioSource>();
        _muzzleFlash = transform.Find("Model/MuzzleFlash").gameObject;

        _bulletImages = new GameObject[clipSize];

        _bulletUI = transform.Find("BulletUI").gameObject;
        Transform _bulletTransform = _bulletUI.transform;

        for (int i = 0; i < clipSize; i++)
        {
            _bulletImages[i] = _bulletTransform.GetChild(i).gameObject;
        }

        _reloadUI = transform.Find("ReloadUI").gameObject;
        _reloadUI.SetActive(false);

        _dryUI = transform.Find("DryUI").gameObject;
        _dryUI.SetActive(false);

        _waitUI = transform.Find("WaitUI").gameObject;
        _waitUI.SetActive(false);

        // Initialize
        _lineRenderer.enabled = false;
        _muzzleFlash.SetActive(false);
        SetAmmo(0);
    }

    private void SetAmmo(int ammo)
    {
        _ammo = ammo;
        UpdateUI();
    }
    
    private void TriggerPressed(object sender, ClickedEventArgs e)
    {
        if (_safety) return;

        if (_ammo > 0)
        {
            Shoot();
        }
        else
        {
            if (!_flashing && !_reloading)
                StartCoroutine("DisplayNoAmmo");
        }
    }

    private IEnumerator DisplayNoAmmo()
    {
        _flashing = true;
        _dryUI.SetActive(true);

        _audioSource.PlayOneShot(audioFiles[2]);

        yield return new WaitForSeconds(0.1f);

        _dryUI.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        _dryUI.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        _flashing = false;
        _dryUI.SetActive(false);
    }

    private void UpdateUI()
    {
        for(int i = 0; i < clipSize; i++)
        {
            _bulletImages[i].SetActive(i < _ammo);
        }
    }

    private void Shoot()
    {
        _audioSource.PlayOneShot(audioFiles[0], 1);

        _ammo--;
        StartCoroutine(ShowLaser());

        _ray.origin = _barrelTransform.position;
        _ray.direction = _barrelTransform.forward;

        if (Physics.Raycast(_ray, out _hit, 100))
        {
            if (_hit.transform.GetComponent<Shootable>() != null)
            {
                _hit.transform.GetComponent<Shootable>().OnClick();
            }
            Instantiate(_particleSpark, _hit.point, Quaternion.identity);
        }

        UpdateUI();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Equals("ReloadTrigger"))
        {
            if (!_reloading)
                StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        if (_ammo >= clipSize) yield break;

        _ammo = 0;

        _reloading = true;

        _audioSource.PlayOneShot(audioFiles[1], 1);

        if (_flashing)
        {
            StopCoroutine("DisplayNoAmmo");
            _dryUI.SetActive(false);
            _flashing = false;
        }

        _reloadUI.SetActive(true);
        _bulletUI.SetActive(false);

        yield return new WaitForSeconds(reloadTime);

        _reloadUI.SetActive(false);
        _bulletUI.SetActive(true);

        _ammo = clipSize;
        _reloading = false;

        UpdateUI();
    }

    private IEnumerator ShowLaser()
    {
        _lineRenderer.enabled = true;
        _muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        _lineRenderer.enabled = false;
        _muzzleFlash.SetActive(false);
    }

    public void EnableSafety()
    {
        _safety = true;
        _waitUI.SetActive(true);
    }

    public void DisableSafety()
    {
        _safety = false;
        _waitUI.SetActive(false);
    }
}
