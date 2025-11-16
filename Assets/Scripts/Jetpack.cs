using UnityEngine;
using UnityEngine.UI;

public class Jetpack : MonoBehaviour
{
    public float jetForce = 10f;
    public float fuel = 100f;
    public float fuelDrainTime = 1f;
    public float fuelRechargeTime = 0.5f;
    public float emptyRechargeDelay = 0.5f;

    public Image fuelUI; // radial fill

    Rigidbody rb;
    PlayerMovement pm;

    bool grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        if (grounded)
            Recharge();
        else
            Fly();

        fuelUI.fillAmount = fuel / 100f;
    }

    void Fly()
    {
        if (!Input.GetKey(KeyCode.Space)) return;
        if (fuel <= 0) return;

        fuel -= (100 / fuelDrainTime) * Time.deltaTime;
        rb.AddForce(Vector3.up * jetForce, ForceMode.Acceleration);
    }

    float emptyTimer = 0f;

    void Recharge()
    {
        if (fuel <= 0)
        {
            emptyTimer += Time.deltaTime;
            if (emptyTimer < emptyRechargeDelay) return;
        }

        emptyTimer = 0f;
        fuel += (100 / fuelRechargeTime) * Time.deltaTime;
        fuel = Mathf.Clamp(fuel, 0, 100);
    }
}
