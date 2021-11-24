using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    /*public GameObject linePrefab;
    public GameObject currentLine;

    public LineRenderer lineRenderer;

    public List<Vector2> pointPositions;

    Sword sword;

    bool canDraw = true;

    bool hasDrawnPath;

    public int currentIndex;

    Transform swordHolder;

    void Start()
    {
        sword = GameObject.Find("SwordHolder").GetComponent<Sword>();
        swordHolder = sword.transform;
    }

    void Update()
    {
        if (hasDrawnPath)
        {
            float step = 10f * Time.deltaTime; // calculate distance to move
            swordHolder.transform.position = Vector3.MoveTowards(swordHolder.transform.position, pointPositions[currentIndex + 1], step);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(swordHolder.transform.position, pointPositions[currentIndex + 1]) < 0.001f)
            {
                if (currentIndex < pointPositions.Count - 1)
                    ++currentIndex;

                if(currentIndex == pointPositions.Count - 1)
                    hasDrawnPath = false;
            }
        }

        if (!sword.swordHasFormed)
            return;

        if (!canDraw)
            return;

        if(!hasDrawnPath)
        { 
            if(Input.GetMouseButtonDown(0))
            {
                CreateLine();
            }
            if(Input.GetMouseButton(0))
            {
                Vector2 tempPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z));
                if(Vector2.Distance(tempPos, pointPositions[pointPositions.Count - 1]) >  1f)
                {
                    UpdateLine(tempPos);
                }
            }
        }    

        if(Input.GetMouseButtonUp(0))
        {
            hasDrawnPath = true;
            sword.transform.position = new Vector3(pointPositions[0].x, pointPositions[0].y, sword.transform.position.z);
            swordHolder = sword.transform;
            Transform swordObject = swordHolder.transform.GetChild(0).transform;
            swordObject.localPosition = Vector3.zero;
            swordObject.localRotation = Quaternion.identity;
            swordHolder.RotateAround(swordObject.position, Vector3.right, 90);

            Transform line = GameObject.Find("DrawingLine(Clone)").transform;
            swordHolder.parent = line;
            swordHolder.transform.localPosition = new Vector3(pointPositions[0].x, pointPositions[0].y, 0);
            currentIndex = 0;
        }
    }

    void CreateLine()
    {
        canDraw = true;
        currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        pointPositions.Clear();
        pointPositions.Add(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)));
        pointPositions.Add(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)));
        lineRenderer.SetPosition(0, pointPositions[0]);
        lineRenderer.SetPosition(1, pointPositions[1]);
        StartCoroutine(SwordStrikeTimer());
    }

    void UpdateLine(Vector2 newPos)
    {
        pointPositions.Add(newPos);
        ++lineRenderer.positionCount;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPos);
    }

    IEnumerator SwordStrikeTimer()
    {
        yield return new WaitForSeconds(1.5f);
        canDraw = false;
    }
    void MoveAlongPath()
    {
        // Move our position a step closer to the target.

    }*/
}
