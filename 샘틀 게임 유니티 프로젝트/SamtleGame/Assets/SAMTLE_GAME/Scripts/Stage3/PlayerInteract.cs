using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private GameObject nearestInteractable = null;

    private InteractRange InteractRange;

    void Start()
    {
        InteractRange = gameObject.transform.Find("Main Camera").Find("InteractRange").GetComponent<InteractRange>();
        Debug.Log(InteractRange);
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
            if (nearestInteractable != null)
            {
                Debug.Log(nearestInteractable.name);
            }
        }
    }

    private void checkNearest()
    {
        nearestInteractable = InteractRange.findNearest(gameObject.transform);
    }
}
