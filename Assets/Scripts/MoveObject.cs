using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveObject : MonoBehaviour
{
    public Image reticleImage;
    public Color focusColor;
    public Transform holdingPosition;
    public Transform inspectingPosition;

    Color originalReticleColor;

    RaycastHit hit;

    bool holding;
    bool inspecting;
    Transform heldObject;
    Vector3 originalScale;

    void Start()
    {
        originalReticleColor = reticleImage.color;
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            UpdateReticle(true, hit.collider.CompareTag("Moveable") || hit.collider.CompareTag("Container"));

            if (Input.GetButtonDown("Fire1"))
            {
                //Do we have a free hand, and are we looking at a moveable object?
                if (!holding && hit.collider.CompareTag("Moveable"))
                {
                    PickUp();
                }

                //Do we have an object, and are we looking at a container?
                else if (holding && hit.collider.CompareTag("Container"))
                {
                    PutDown();
                }
            }
        }
        else
        {
            UpdateReticle(false, false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Do we have an object, and are we inspecting?
            if (holding && !inspecting)
            {
                inspecting = true;
                heldObject.SetPositionAndRotation(inspectingPosition.position, inspectingPosition.rotation);
            }
            else if (holding && inspecting)
            {
                inspecting = false;
                heldObject.SetPositionAndRotation(holdingPosition.position, holdingPosition.rotation);
            }
        }
    }

    void PickUp()
    {
        holding = true;
        heldObject = hit.collider.transform;
        originalScale = heldObject.localScale;
        heldObject.SetParent(this.transform, true);
        heldObject.SetPositionAndRotation(holdingPosition.position, holdingPosition.rotation);
        heldObject.localScale = originalScale;
    }

    void PutDown()
    {
        StoreObject containerScript = hit.collider.GetComponent<StoreObject>();

        if (containerScript != null)
        {
            if (containerScript.PlaceItemInSlot(heldObject))
            {
                holding = false;
            }
        }
    }


    void UpdateReticle(bool hitDetected, bool isTarget)
    {
        if (hitDetected && isTarget)
        {
            reticleImage.color = Color.Lerp(reticleImage.color, focusColor, Time.deltaTime * 2);
            reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, new Vector3(0.7f, 0.7f, 1), Time.deltaTime * 2);
        }
        else
        {
            reticleImage.color = Color.Lerp(reticleImage.color, originalReticleColor, Time.deltaTime * 2);
            reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, Vector3.one, Time.deltaTime * 2);
        }
    }
}
