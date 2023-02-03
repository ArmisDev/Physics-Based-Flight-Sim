using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [SerializeField] private PickupType pickupType;
    [SerializeField] float _currentFuel;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip _getFuelSound;

    public Collider pickUpCollider;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pickUpCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider pickUpCollider)
    {
        if (pickUpCollider.gameObject.tag == "Player")
        {
            FindObjectOfType<PickupHandler>().HandleFuelPickup(pickupType, _currentFuel);
            audioSource.PlayOneShot(_getFuelSound);
            Destroy(gameObject, 0.1f);
        }
    }
}
