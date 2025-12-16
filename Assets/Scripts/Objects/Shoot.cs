using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    private PlayerInputs inputAction;

    [Header("Shoot Settings")]
    public float raycastRange = 400f;
    public int maxBullets = 6;
    public int currentBullets;

    private bool ammoChanged = false;

    [Header("Shoot Effects")]
    public Transform muzzlePoint;      // Punto de salida de la bala / partícula
    public ParticleSystem muzzleFlash; // Partícula en el arma
    public ParticleSystem hitEffect;   // Partícula en el impacto
    public AudioSource shootSound;     // Sonido de disparo

    private void Awake()
    {
        inputAction = new PlayerInputs();
        inputAction.Enable();

        currentBullets = maxBullets;
    }

    private void Start()
    {
        inputAction.Player.shoot.performed += Shooting;
    }

    private void Update()
    {
        if (ammoChanged)
        {
            ammoChanged = false;
            if (HUDController.instance != null)
                HUDController.instance.UpdateAmmo(currentBullets);
        }
    }

    private void Shooting(InputAction.CallbackContext obj)
    {
        if (currentBullets <= 0) return;

        // Reducir balas y actualizar HUD
        currentBullets--;
        if (HUDController.instance != null)
            HUDController.instance.UpdateAmmo(currentBullets);

        // Reproducir sonido de disparo
        if (shootSound != null)
            shootSound.Play();

        // Reproducir partícula de disparo en el cañón
        if (muzzleFlash != null && muzzlePoint != null)
        {
            ParticleSystem flash = Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
            flash.Play();
            Destroy(flash.gameObject, 1f);
        }

        Camera camera = Camera.main;
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = camera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, raycastRange))
        {
            // Partícula de impacto
            if (hitEffect != null)
            {
                ParticleSystem impact = Instantiate(hitEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                impact.Play();
                Destroy(impact.gameObject, 2f);
            }

            // Interacción con objetos
            if (hitInfo.collider.CompareTag("CAN"))
            {
                Can canscript = hitInfo.collider.GetComponent<Can>();
                if (canscript != null)
                    canscript.OnHit(hitInfo.point);
            }
            else if (hitInfo.collider.CompareTag("EAGLE"))
            {
                Eagle eagleScript = hitInfo.collider.GetComponent<Eagle>();
                if (eagleScript != null)
                    eagleScript.OnHit(hitInfo.point);
            }
        }
    }

    public void AddBullets(int amount)
    {
        currentBullets = Mathf.Clamp(currentBullets + amount, 0, maxBullets);
        if (HUDController.instance != null)
            HUDController.instance.UpdateAmmo(currentBullets);
    }

    public bool IsFull() => currentBullets >= maxBullets;
}