using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractRange : MonoBehaviour
{
    List<GameObject> interactableObjectList;

    void Start()
    {
        interactableObjectList = new List<GameObject>();
    }
   
    //Interactable 오브젝트가 범위에 들어오면 리스트에 넣음
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Interactable") && !interactableObjectList.Contains(col.gameObject))
        {
            interactableObjectList.Add(col.gameObject);

            Debug.Log(col.gameObject + "in");
        }
    }

    //Interactable 오브젝트가 범위에서 나가면면 리스트에서 뺌
    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Interactable") && interactableObjectList.Contains(col.gameObject))
        {
            interactableObjectList.Remove(col.gameObject);

            Debug.Log(col.gameObject + "out");
        }
    }

    //제일 가까운 Interactable 오브젝트를 찾음
    public GameObject findNearest(Transform player)
    {
        if (interactableObjectList.Count == 0)
        {
            return null;
        }

        GameObject _out = interactableObjectList[0];

        for(int i = 0; i < interactableObjectList.Count ;i++)
        {
            if (Vector3.Distance(interactableObjectList[i].transform.position, player.position) < Vector3.Distance(_out.transform.position, player.position))
            {
                _out = interactableObjectList[i];
            }
        }

        return _out;
    }
}
