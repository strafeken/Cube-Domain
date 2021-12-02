using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartFormation : MonoBehaviour
{
    private GameObject h1;
    private GameObject h2;
    private GameObject h3;
    private GameObject h4;
    private GameObject h5;
    private GameObject h6;

    private Fortune fortune;
    private FortuneBodyMovement bodyMovement;

    private GameObject activeHeart;

    void Awake()
    {
        h1 = transform.Find("1H").gameObject;
        h2 = transform.Find("2H").gameObject;
        h3 = transform.Find("3H").gameObject;
        h4 = transform.Find("4H").gameObject;
        h5 = transform.Find("5H").gameObject;
        h6 = transform.Find("6H").gameObject;

        fortune = GetComponentInParent<Fortune>();
        bodyMovement = GetComponentInParent<FortuneBodyMovement>();
    }

    void Start()
    {
        fortune.OnUsedSkill += OnUsedSkillEvent;
        bodyMovement.OnSideChanged += OnSideChangedEvent;
    }

    void OnDisable()
    {
        fortune.OnUsedSkill -= OnUsedSkillEvent;
        bodyMovement.OnSideChanged -= OnSideChangedEvent;
    }

    private void OnSideChangedEvent(string name)
    {
        h1.SetActive(false);
        h2.SetActive(false);
        h3.SetActive(false);
        h4.SetActive(false);
        h5.SetActive(false);
        h6.SetActive(false);

        switch (name)
        {
            case "Num1":
                h1.SetActive(true);
                break;
            case "Num2":
                h2.SetActive(true);
                break;
            case "Num3":
                h3.SetActive(true);
                break;
            case "Num4":
                h4.SetActive(true);
                break;
            case "Num5":
                h5.SetActive(true);
                break;
            case "Num6":
                h6.SetActive(true);
                break;
        }

        if (h1.activeInHierarchy)
        {
            activeHeart = h1;
            //StartCoroutine("Impale");
        }
        else if (h2.activeInHierarchy)
        {
            activeHeart = h2;

        }
        else if (h3.activeInHierarchy)
        {

            activeHeart = h3;
        }
        else if (h4.activeInHierarchy)
        {
            activeHeart = h4;

        }
        else if (h5.activeInHierarchy)
        {
            activeHeart = h5;
            //SetState(State.MISSILE);
        }
        else if (h6.activeInHierarchy)
        {
            activeHeart = h6;
            //SetState(State.CAGE);
        }

        OnUsedSkillEvent();
    }

    private void OnUsedSkillEvent()
    {
        for (int i = 0; i < activeHeart.transform.childCount; ++i)
        {
            float angle = i * Mathf.PI * 2f / activeHeart.transform.childCount;
            Vector3 newPosition = new Vector3(Mathf.Cos(angle) * 3f, -3f, Mathf.Sin(angle) * 3f);
            activeHeart.transform.GetChild(i).localPosition = newPosition;
        }
    }
}
