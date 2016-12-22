using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuGun : MonoBehaviour
{
    private static Vector3 FARPOINT = new Vector3(0, 0, 200);

    private SteamVR_TrackedController _controller;
    private SteamVR_TrackedObject _object;
    private int _deviceIndex;

    private Transform _transform;
    private Transform _barrel;
    private AudioSource _audioSource;

    private LineRenderer _lineRenderer;
    private MainMenuTable _menuTable;

    private Ray _ray = new Ray();
    private RaycastHit _hit;

    private AsyncOperation _asyncOperation;

    private GameObject _optionsMenu;
    private Button _button;
    private Button _lastButton;

    void Awake()
    {
        _optionsMenu = GameObject.Find("OptionsMenu");
    }

	void Start()
	{
        _transform = transform;
        _controller = _transform.GetComponentInParent<SteamVR_TrackedController>();
        if (!Options.LeftHanded)
            _deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
        else
            _deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        _controller.SetDeviceIndex(_deviceIndex);
        StartCoroutine(GetController());
        _controller.TriggerClicked += TriggerPressed;
        _audioSource = _transform.GetComponent<AudioSource>();
        _menuTable = FindObjectOfType<MainMenuTable>();

        _barrel = _transform.Find("Model/Barrel");
        _lineRenderer = _transform.Find("Model/Sight").GetComponent<LineRenderer>();

        Debug.Log("MenuGun: Device Index = " + _deviceIndex);
    }

    private IEnumerator GetController()
    {
        yield return new WaitWhile(new Func<bool>(() => _transform.GetComponentInParent<SteamVR_TrackedObject>() == null));

        _object = _transform.GetComponentInParent<SteamVR_TrackedObject>();

        _object.SetDeviceIndex(_deviceIndex);
    }

    void Update()
    {
        _ray.origin = _barrel.position;
        _ray.direction = _barrel.forward;

        if (Physics.Raycast(_ray, out _hit, 100))
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

            _lineRenderer.SetPosition(1, _barrel.InverseTransformPoint(_hit.point));
        }
        else
            _lineRenderer.SetPosition(1, FARPOINT);

        if (_asyncOperation == null) return;

        if (Mathf.Approximately(_asyncOperation.progress, 0.9f))
            _asyncOperation.allowSceneActivation = true;
    }

    private void TriggerPressed(object sender, ClickedEventArgs e)
    {
        Shoot();
    }

    public void SwapHandedness()
    {
        if (_deviceIndex == SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost))
            _deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
        else
            _deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);

        _controller.SetDeviceIndex(_deviceIndex);
        _object.SetDeviceIndex(_deviceIndex);

        Debug.Log("Swapped Handedness");
    }

    private void Shoot()
    {
        _audioSource.Play();

        _ray.origin = _barrel.position;
        _ray.direction = _barrel.forward;

        if (_button != null)
        {
            _button.OnPointerClick(new PointerEventData(EventSystem.current));
        }
        else if (Physics.Raycast(_ray, out _hit, 100))
        {
            Debug.Log("hit: " + _hit.transform.name);

            switch (_hit.transform.name)
            {
                case "Play":
                    _asyncOperation = SceneManager.LoadSceneAsync("Tutorial");
                    _asyncOperation.allowSceneActivation = false;
                    break;
                case "Options":
                    _optionsMenu.SetActive(!_optionsMenu.activeSelf);
                    break;
                case "Exit":
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }
}
