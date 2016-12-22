using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    private static int PICKUP_LAYER = -1;

    private SteamVR_TrackedController _controller;
    private SteamVR_TrackedObject _object;
    private int _deviceIndex;

    private Transform _transform;
    private Transform _barrelTransform;
    private Transform _sightTransform;
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
    private GameObject _moveUI;
    private GameController _gameController;
    
    private Button _button;
    private Button _lastButton;

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
        _transform = transform;

        // Assign
        _gameController = FindObjectOfType<GameController>();
        _controller = _transform.GetComponentInParent<SteamVR_TrackedController>();
        if(!Options.LeftHanded)
            _deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        else
            _deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        _controller.SetDeviceIndex(_deviceIndex);
        _controller.TriggerClicked += TriggerPressed;
        StartCoroutine(GetController());

        _barrelTransform = _transform.Find("Model/Barrel");
        _sightTransform = _transform.Find("Model/Sight");
        _lineRenderer = _sightTransform.GetComponent<LineRenderer>();
        _audioSource = _transform.GetComponent<AudioSource>();
        _muzzleFlash = _transform.Find("Model/MuzzleFlash").gameObject;

        _bulletImages = new GameObject[clipSize];

        _bulletUI = _transform.Find("BulletUI").gameObject;
        Transform _bulletTransform = _bulletUI.transform;

        for (int i = 0; i < clipSize; i++)
        {
            _bulletImages[i] = _bulletTransform.GetChild(i).gameObject;
        }

        _reloadUI = _transform.Find("ReloadUI").gameObject;
        _reloadUI.SetActive(false);

        _dryUI = _transform.Find("DryUI").gameObject;
        _dryUI.SetActive(false);

        _waitUI = _transform.Find("WaitUI").gameObject;
        _waitUI.SetActive(false);

        // Initialize
        _muzzleFlash.SetActive(false);
        SetAmmo(0);

        Debug.Log("Gun: Device Index = " + _deviceIndex);
    }

    private IEnumerator GetController()
    {
        yield return new WaitWhile(new Func<bool>(() => _transform.GetComponentInParent<SteamVR_TrackedObject>() == null));
        
        _object = _transform.GetComponentInParent<SteamVR_TrackedObject>();
        _object.SetDeviceIndex(_deviceIndex);
    }

    void Update()
    {
        _ray.origin = _sightTransform.position;
        _ray.direction = _sightTransform.forward;

        if (Physics.Raycast(_ray, out _hit, 200f))
        {
            if (_hit.transform.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                _lastButton = _button;
                _button = _hit.transform.GetComponent<Button>();
                
                if (_button != null)
                {
                    if (_button != _lastButton)
                    {

                    }
                }
                else
                {

                }
            }

            _lineRenderer.SetPosition(1, new Vector3(0, 0, _hit.distance));
        }
        else
            _lineRenderer.SetPosition(1, _sightTransform.InverseTransformPoint(_ray.GetPoint(200)));
    }

    public void SwapHandedness()
    {
        if (_deviceIndex == SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost))
            _deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        else
            _deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);

        _controller.SetDeviceIndex(_deviceIndex);
        _object.SetDeviceIndex(_deviceIndex);
    }

    private void SetAmmo(int ammo)
    {
        _ammo = ammo;
        UpdateUI();
    }
    
    private void TriggerPressed(object sender, ClickedEventArgs e)
    {
        if (_button != null)
        {
            _button.OnPointerClick(new PointerEventData(EventSystem.current));
            return;
        }

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
        StartCoroutine(ShowMuzzleFlash());

        _ray.origin = _barrelTransform.position;
        _ray.direction = _barrelTransform.forward;

        if (Physics.Raycast(_ray, out _hit, 100))
        {
            //Debug.Log("Hit: " + _hit.transform.name);
            if (_hit.transform.GetComponent<Shootable>() != null)
            {
                _hit.transform.GetComponent<Shootable>().OnClick();
            }

            if (_hit.transform.GetComponent<Rigidbody>() != null)
            {
                _hit.transform.GetComponent<Rigidbody>().AddForce(_ray.direction * 20000000);
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

    private IEnumerator ShowMuzzleFlash()
    {
        _muzzleFlash.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        
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
