using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private GameObject _nearestInteractable = null;

    private InteractRange _InteractRange;

    void Start()
    {
        _InteractRange = gameObject.transform.Find("Main Camera").Find("InteractRange").GetComponent<InteractRange>();
        Debug.Log(_InteractRange);
    }

    void Update()
    {
        checkNearest();
        interact();
    }

    private void interact()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (_nearestInteractable != null)
            {
                Debug.Log(_nearestInteractable.name);
            }
        }
    }

    private void checkNearest()
    {
        _nearestInteractable = _InteractRange.findNearest(gameObject.transform);
    }
}
