using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreObject : MonoBehaviour
{
    List<Transform> slots = new List<Transform>();

    void Start()
    {
        AssignSlots();
    }

    void AssignSlots()
    {
        //Find any child slots and put them in a list
        var childObjects = GetComponentInChildren<Transform>();

        foreach (Transform item in childObjects)
        {
            if (item.CompareTag("Slot") && item != transform)
            {
                slots.Add(item);
            }
        }
    }

    public bool PlaceItemInSlot(Transform obj)
    {
        foreach (var slot in slots)
        {
            if (slot.childCount == 0)
            {
                Vector3 ogSize = obj.localScale;
                obj.SetParent(slot, true);
                obj.SetPositionAndRotation(slot.position, slot.rotation);
                obj.localScale = ogSize;
                return true;
            }
         }

        return false;
    }

}
