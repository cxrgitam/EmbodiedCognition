using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FigureEnabler : MonoBehaviour
{
    public List<GameObject> itemList;
    public List<GameObject> fiveSecObjList;
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
    private bool hasWaitedFiveSeconds = false;

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

            if (childIdx == 1 && itemList[index].transform.childCount > 2 && !hasWaitedFiveSeconds)
            {
                StartCoroutine(WaitAndActivateNextChild());
            }
            else if (childIdx == 2 && itemList[index].transform.childCount > 3 && IsInFiveSecObjList(itemList[index]) && !hasWaitedFiveSeconds)
            {
                StartCoroutine(FiveSecWaitAndActivateNextChild());
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

    private bool IsInFiveSecObjList(GameObject obj)
    {
        return fiveSecObjList.Exists(item => item.name == obj.name);
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

        if (itemList[index].transform.childCount > 1)
        {
            itemList[index].transform.GetChild(1).gameObject.SetActive(false);
            if (itemList[index].transform.childCount > 2)
            {
                itemList[index].transform.GetChild(2).gameObject.SetActive(true);
                childIdx = 2;
            }
        }

        isWaiting = false;
        hasWaitedFiveSeconds = false;
    }

    IEnumerator FiveSecWaitAndActivateNextChild()
    {
        isWaiting = true;
        yield return new WaitForSeconds(5f);

        if (itemList[index].transform.childCount > 2)
        {
            itemList[index].transform.GetChild(2).gameObject.SetActive(false);
            if (itemList[index].transform.childCount > 3)
            {
                itemList[index].transform.GetChild(3).gameObject.SetActive(true);
                childIdx = 3;
            }
        }

        isWaiting = false;
        hasWaitedFiveSeconds = true;
    }
}
