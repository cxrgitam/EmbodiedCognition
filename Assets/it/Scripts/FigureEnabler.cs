using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FigureEnabler : MonoBehaviour
{
    public List<GameObject> itemList;
    public GameObject reticle;
    private GameObject spawnedReticle;

    public XRRayInteractor rayInteractor;

    public InputActionProperty rightTrigger;
    public InputActionProperty leftTrigger;

    public Vector3ToJson vecToJson;

    private RaycastHit hit;

    private int index = 0;
    private int childIdx = 0;
    private bool isWaiting = false;

    void Start()
    {
        if (itemList.Count > 0)
        {
            ActivateCurrentItem();
        }
        spawnedReticle = Instantiate(reticle);
        spawnedReticle.SetActive(false);
    }

    void Update()
    {
        if (leftTrigger.action.WasPressedThisFrame() && !isWaiting)
        {
            HandleLeftTriggerPress();
        }

        UpdateReticle();

        if (rightTrigger.action.WasPressedThisFrame())
        {
            HandleRightTriggerPress();
        }
    }

    private void ActivateCurrentItem()
    {
        itemList[index].SetActive(true);
        if (itemList[index].transform.childCount > 0)
        {
            itemList[index].transform.GetChild(childIdx).gameObject.SetActive(true);
        }
    }

    private void HandleLeftTriggerPress()
    {
        spawnedReticle.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        if (itemList.Count == 0) return;

        if (itemList[index].transform.childCount > 0 && !isWaiting)
        {
            itemList[index].transform.GetChild(childIdx).gameObject.SetActive(false);
        }

        MoveToNextChildOrItem();
    }

    private void MoveToNextChildOrItem()
    {
        if (itemList.Count == 0) return;

        if (childIdx < itemList[index].transform.childCount - 1)
        {
            childIdx++;
            itemList[index].transform.GetChild(childIdx).gameObject.SetActive(true);

            if (childIdx == 1)
            {
                StartCoroutine(WaitAndActivateNextChild());
            }
        }
        else
        {
            itemList[index].SetActive(false);
            index++;
            childIdx = 0;

            if (index < itemList.Count)
            {
                ActivateCurrentItem();
            }
        }
    }

    private void UpdateReticle()
    {
        Ray ray = new Ray(rayInteractor.transform.position, rayInteractor.transform.forward);
        if (Physics.Raycast(ray, out hit))
        {
            spawnedReticle.transform.position = hit.point;
            spawnedReticle.SetActive(true);
        }
        else
        {
            spawnedReticle.SetActive(false);
        }
    }

    private void HandleRightTriggerPress()
    {
        if (spawnedReticle.activeSelf)
        {
            Debug.Log(hit.point);
            vecToJson.AddVector(hit.point);
            spawnedReticle.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            Debug.Log("No hit detected.");
        }
    }

    IEnumerator WaitAndActivateNextChild()
    {
        isWaiting = true;
        yield return new WaitForSeconds(3f);

        itemList[index].transform.GetChild(1).gameObject.SetActive(false);
        itemList[index].transform.GetChild(2).gameObject.SetActive(true);
        childIdx = 2;
        
        if (childIdx == 2 && itemList[index].transform.childCount >= 4)
        {
            StartCoroutine(WaitAndActivateNextChildFiveSeconds());
        }

        isWaiting = false;
    }
    IEnumerator WaitAndActivateNextChildFiveSeconds()
    {
        isWaiting = true;
        yield return new WaitForSeconds(5f);

        itemList[index].transform.GetChild(2).gameObject.SetActive(false);
        itemList[index].transform.GetChild(3).gameObject.SetActive(true);
        childIdx = 3;

        isWaiting = false;
    }
}
