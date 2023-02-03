using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlaneController : MonoBehaviour
{
    #region - Variables -

    [Header("Plane Parameters")]
    [SerializeField] private float _fuelTotal;
    [SerializeField] private float _currentFuel;
    [Tooltip("How much the throttle ramps up or down")]
    [SerializeField] private float _throttleIncrement = 0.1f;
    [Tooltip("Maximum engine thrust")]
    [SerializeField] private float _maxThrust = 200f;
    [Tooltip("How responsive the plane is when rolling, pitching, and yawing.")]
    [SerializeField] private float _responsiveness = 0.1f;
    [Tooltip("Inverts the Controls")]
    [SerializeField] private bool invertControl;
    [SerializeField] private bool isPropPlane;
    public Collider pickUpCollider;

    //Incase Plane Has Prop
    [SerializeField] GameObject propeller;

    [Header("Physics")]
    [Tooltip("Lift to get off ground")]
    [SerializeField] private float _lift = 135f;
    public Rigidbody rb;

    [Header("Audio/UI")]
    [SerializeField] private AudioSource engineSound;
    [SerializeField] private TextMeshProUGUI hud;
    [SerializeField] private TextMeshProUGUI fuelHUD;

    #endregion

    #region - Private Variables

    //Private Variables
    public float _throttle;    //Percentage of engine thrust currently being used.
    public bool canFly;
    private float _roll;        //Tilting left to right.
    private float _pitch;       //Tilting up and down.
    private float _yaw;         //Turning plane left to right (Does not rotate)

    private float _responseModifier //This float is used to tweak the planes responsivness based on the planes mass.
    {
        get
        {
            return (rb.mass / 10f * _responsiveness);
        }
    }

    private PickupType pickupType;
    private PickupHandler pickupHandler;

    #endregion

    #region - Fuel Variables

    #endregion

    //Start of methods
    private void Start()
    {
        rb.GetComponent<Rigidbody>();
        _currentFuel = _fuelTotal;
        canFly = true;
        pickUpCollider = GetComponent<Collider>();
    }

    private void HandleInputs()
    {
            //Grabs the input for our rotational values
            if (invertControl)
            {
                _roll = -Input.GetAxis("Roll");
                _pitch = -Input.GetAxis("Pitch");
                _yaw = Input.GetAxis("Yaw");
            }

            else
            {
                _roll = Input.GetAxis("Roll");
                _pitch = Input.GetAxis("Pitch");
                _yaw = Input.GetAxis("Yaw");
            }

            //Handles throttle value and clamping it between values of 0 and 100.
            if (Input.GetKey(KeyCode.Space)) _throttle += _throttleIncrement;
            else if (Input.GetKey(KeyCode.LeftCommand)) _throttle -= _throttleIncrement;
            _throttle = Mathf.Clamp(_throttle, 0f, 100f);
    }

    #region - Fuel Logic -

    void FuelHandler()
    {
        _currentFuel -= _throttle * 0.0001f;

        if (_currentFuel <= 0f) StartCoroutine(OutOfFuel());
    }

    IEnumerator OutOfFuel()
    {
        _throttle--;

        yield return new WaitForSeconds(1f);
        canFly = false;
    }

    #endregion

    private void Update()
    {
        HandleInputs();
        UpdateHUD();
        FuelHandler();

        engineSound.volume = _throttle * 0.01f;
        engineSound.pitch = _throttle * 0.01f;

        if(isPropPlane)
        {
            propeller.transform.Rotate(Vector3.right * _throttle);
        }

        else
        {
            return;
        }
    }

    private void FixedUpdate()
    {
        if (canFly)
        {
            rb.AddForce(transform.forward * _maxThrust * _throttle);
            rb.AddTorque(transform.up * _yaw * _responseModifier);
            rb.AddTorque(transform.right * _pitch * _responseModifier);
            rb.AddTorque(-transform.forward * _roll * _responseModifier);

            rb.AddForce(Vector3.up * rb.velocity.magnitude * _lift);
        }

        else return;
    }

    private void UpdateHUD()
    {
        hud.text = "Throttle " + _throttle.ToString("F2") + "%\n";
        hud.text += "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F2") + "km/h\n";
        hud.text += "Altitude: " + (transform.position.y.ToString("F2")) + "m\n";
        fuelHUD.text = "Fuel: " + _currentFuel.ToString("F0") + "%";
    }
}
