using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private GameObject cursor;
    [SerializeField] private GameObject blocked;
    [SerializeField] private GameObject notBlocked;
    [SerializeField] private GameObject stab;

    private GraphicRaycaster rayCaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    private List<RaycastResult> results = new List<RaycastResult>();

    void Start()
    {
        Cursor.visible = false;
        rayCaster = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GraphicRaycaster>();
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        pointerEventData = new PointerEventData(eventSystem);
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            cursor.SetActive(false);
        else
            cursor.SetActive(true);

        pointerEventData.position = Input.mousePosition;
        
        rayCaster.Raycast(pointerEventData, results);

        // Mouse is pointing at free area
        if(results.Count < 1)
        {
            PointingAtFreeArea();
        }
        // Mouse is pointing at blocked area
        else if(results.Count < 2)
        {
            PointingAtBlockedArea();
            results.Clear();
        }
        // Mouse is pointing at stab area
        else
        {
            PointingAtStabArea();
            results.Clear();
        }

        cursor.transform.position = Input.mousePosition;
    }

    void PointingAtFreeArea()
    {
        notBlocked.SetActive(true);
        blocked.SetActive(false);
        stab.SetActive(false);
    }

    void PointingAtBlockedArea()
    {
        blocked.SetActive(true);
        notBlocked.SetActive(false);
        stab.SetActive(false);
    }

    void PointingAtStabArea()
    {
        stab.SetActive(true);
        blocked.SetActive(false);
        notBlocked.SetActive(false);
    }
}
