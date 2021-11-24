using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackAreaManager : MonoBehaviour
{
    private RealisticSwordAttack sword;

    [SerializeField] private Image untouchableArea;
    [SerializeField] private Image stabArea;

    void Awake()
    {
        sword = GameObject.FindGameObjectWithTag("Sword").GetComponent<RealisticSwordAttack>();
    }

    void Update()
    {
        FollowPivot();

        if (sword.isFirstSwing)
            FirstSwingCover();
        else
            OtherSwingsCover();
    }

    private void FirstSwingCover()
    {
        Vector3 pivotScreenPointPosition = Camera.main.WorldToViewportPoint(sword.transform.position);
        untouchableArea.rectTransform.anchorMax = new Vector2(pivotScreenPointPosition.x + 0.1f, 1);
        untouchableArea.rectTransform.anchorMin = new Vector2(0, 0);
    }

    private void OtherSwingsCover()
    {
        Vector3 pivotScreenPointPosition = Camera.main.WorldToViewportPoint(sword.transform.position);
        untouchableArea.rectTransform.anchorMax = new Vector2(pivotScreenPointPosition.x + 0.1f, 1);
        untouchableArea.rectTransform.anchorMin = new Vector2(pivotScreenPointPosition.x - 0.1f, 0);
    }

    private void FollowPivot()
    {
        Vector3 pivotScreenPointPosition = Camera.main.WorldToScreenPoint(sword.transform.position);
        stabArea.rectTransform.position = pivotScreenPointPosition;
    }
}
