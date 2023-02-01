using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneController : MonoBehaviour
{
    [Header("Plane Parameters")]
    [Tooltip("How much the throttle ramps up or down")]
    [SerializeField] private float _throttleIncrement = 0.1f;
    [Tooltip("Maximum engine thrust")]
    [SerializeField] private float _maxThrust = 200f;
    [Tooltip("How responsive the plane is when rolling, pitching, and yawing.")]
    [SerializeField] private float _responsiveness = 0.1f;
    [Tooltip("Inverts the Controls")]
    [SerializeField] private bool invertControl;
    [Tooltip("Lift to get off ground")]
    [SerializeField] private float _lift = 135f;

    //Private Variables
    public float _throttle;    //Percentage of engine thrust currently being used.
    private float _roll;        //Tilting left to right.
    private float _pitch;       //Tilting up and down.
    private float _yaw;         //Turning plane left to right (Does not rotate)

    public Rigidbody rb;
    [SerializeField] private AudioSource engineSound;
    [SerializeField] private TextMeshProUGUI hud;

    private float _responseModifier //This float is used to tweak the planes responsivness based on the planes mass.
    {
        get
        {
            return (rb.mass / 10f * _responsiveness);
        }
    }

    //Start of methods
    private void Awake()
    {
        rb.GetComponent<Rigidbody>();
    }

    private void HandleInputs()
    {
        //Grabs the input for our rotational values
        if(invertControl)
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

    private void Update()
    {
        HandleInputs();
        UpdateHUD();

        engineSound.volume = _throttle * 0.01f;
        engineSound.pitch = _throttle * 0.03f;
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * _maxThrust * _throttle);
        rb.AddTorque(transform.up * _yaw * _responseModifier);
        rb.AddTorque(transform.right * _pitch * _responseModifier);
        rb.AddTorque(-transform.forward * _roll * _responseModifier);

        rb.AddForce(Vector3.up * rb.velocity.magnitude * _lift);
    }

    private void UpdateHUD()
    {
        hud.text = "Throttle" + _throttle.ToString("F2") + "%\n";
        hud.text += "Airspeed: " + (rb.velocity.magnitude * 3.6f).ToString("F2") + "km/h\n";
        hud.text += "Altitude: " + (transform.position.y.ToString("F2")) + "m";
    }
}
