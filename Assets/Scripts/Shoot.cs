using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    private PlayerInputs inputAction;
    public float raycastRange = 100f;

    public int maxBullets = 6;
    private int currentBullets;

    private bool isReloading = false;
    private float reloadTime = 0f;
    private float reloadDuration = 0f;

    private void Awake()
    {
        inputAction = new PlayerInputs();
        inputAction.Enable();

        currentBullets = maxBullets;
    }
    private void Start()
    {
        inputAction.Player.shoot.performed += Shooting;
        inputAction.Player.recharge.performed += Recharge;
    }

    private void Update()
    {
        if (isReloading)
        {
            reloadTime -= Time.deltaTime;

            if (reloadTime <= 0f)
            {
                FinishReload();
            }
        }
    }

    private void Shooting(InputAction.CallbackContext obj)
    {

        if (isReloading){ return; }
        if (currentBullets <= 0) 
        {
            StartReload(2f);
            return;
        }

        currentBullets--;

        Camera camera = Camera.main;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = camera.ScreenPointToRay(mousePos);

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 0.1f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastRange)) 
        {
            if (hitInfo.collider.CompareTag("CAN"))
            {
                Can canscript = hitInfo.collider.GetComponent<Can>();

                if (canscript != null)
                {
                    canscript.OnHit(hitInfo.point);
                }
            }
        }
    }

    private void Recharge(InputAction.CallbackContext obj)
    {
        if (isReloading) { return; }
        if(currentBullets == maxBullets) { return; }

        StartReload(1.5f);
    }
    private void StartReload(float time)
    {
        isReloading = true;
        reloadDuration = time;
        reloadTime = time;
    }
    private void FinishReload()
    {
        isReloading = false;
        currentBullets = maxBullets;
    }
}