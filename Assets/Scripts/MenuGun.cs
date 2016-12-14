using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGun : MonoBehaviour
{
    private static Vector3 FARPOINT = new Vector3(0, 0, 200);

    private SteamVR_TrackedController _controller;

    private Transform _transform;
    private Transform _barrel;
    private AudioSource _audioSource;

    private LineRenderer _lineRenderer;
    private MainMenuTable _menuTable;

    private Ray _ray = new Ray();
    private RaycastHit _hit;

    private AsyncOperation _asyncOperation;

	void Start()
	{
        _transform = transform;
        _controller = _transform.GetComponentInParent<SteamVR_TrackedController>();
        _controller.TriggerClicked += TriggerPressed;
        _audioSource = _transform.GetComponent<AudioSource>();
        _menuTable = FindObjectOfType<MainMenuTable>();

        _barrel = _transform.Find("Model/Barrel");
        _lineRenderer = _barrel.GetComponent<LineRenderer>();
    }

    void Update()
    {
        _ray.origin = _barrel.position;
        _ray.direction = _barrel.forward;

        if (Physics.Raycast(_ray, out _hit, 100))
            _lineRenderer.SetPosition(1, _barrel.InverseTransformPoint(_hit.point));
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

    private void Shoot()
    {
        _audioSource.Play();

        _ray.origin = _barrel.position;
        _ray.direction = _barrel.forward;

        if (Physics.Raycast(_ray, out _hit, 100, LayerMask.GetMask("MenuItem")))
        {
            switch (_hit.transform.name)
            {
                case "Play":
                    _asyncOperation = SceneManager.LoadSceneAsync("Tutorial");
                    _asyncOperation.allowSceneActivation = false;
                    break;
                case "Options":
                case "MainMenu":
                    _menuTable.Flip();
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
