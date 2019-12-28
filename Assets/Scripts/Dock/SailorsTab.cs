using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SailorsTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {


    public Sprite captain;
    public Sprite engine;
    public Sprite gunner;
    public Sprite repair;

    private Scrollbar scrollbar;

    GameObject[] slot;
    int n = 6;
    int nSailors = 8;
    List<GameObject> sailorObjList;
    int startPos = 0;
    bool focus = false;
    

	void Start () {
        int pos = 0;
        slot = new GameObject[n];
        for(int i=0; i<n; i++)
        {
            slot[i] = new GameObject("Slot" + i);
            slot[i].transform.SetParent(transform);
            slot[i].transform.localPosition = new Vector3(-300-75 + i * 150, 0, 0);
        }

        sailorObjList = new List<GameObject>();
        for (int i=0; i<nSailors; i++)
        {
            GameObject sailorObj = new GameObject("SailorObj" + i);
            sailorObj.AddComponent<Image>();

            Sailor sailor = sailorObj.AddComponent<Sailor>();
            /*
            if (i == 0)
            {
                sailor.loadImage(Sailor.SailorCls.captain);
            }
            else if (i < 3)
            {
                sailor.loadImage(Sailor.SailorCls.gunner);
            }
            else if (i < 6)
            {
                sailor.loadImage(Sailor.SailorCls.engine);
            }
            else if (i < 8)
            {
                sailor.loadImage(Sailor.SailorCls.repair);
            }
            */

            if (i == 0)
            {
                sailor.setImage(captain);
            }
            else if (i < 3)
            {
                sailor.setImage(gunner);
            }
            else if (i < 6)
            {
                sailor.setImage(engine);
            }
            else if (i < 8)
            {
                sailor.setImage(repair);
            }

            sailor.setListPos(pos);
            pos += 1;
            sailorObjList.Add(sailorObj);
        }
        scrollbar = GetComponentInChildren<Scrollbar>();

        updateList(startPos);
    }

    

    // Update is called once per frame
    void Update () {
        if (focus)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Debug.Log("up");
                if (startPos > 0)
                {
                    startPos -= 1;
                    updateList(startPos);
                }
                Debug.Log(startPos);

            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Debug.Log("down");
                if (startPos < nSailors - n)
                {
                    startPos += 1;
                    updateList(startPos);
                }
                Debug.Log(startPos);
            }
        }
        scrollbar.size = (float) n / nSailors;
        scrollbar.value = (float) startPos / (nSailors - n);
    }

    void updateList(int start)
    {
        int pos = start;
        for (int i=0; i<n; i++)
        {
            slot[i].transform.DetachChildren();
            sailorObjList[pos].transform.SetParent(slot[i].transform);
            sailorObjList[pos].transform.localPosition = new Vector3(0, 0, 0);
            pos += 1;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        focus = true;
        Debug.Log("enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        focus = false;
        Debug.Log("leave");
    }
}
