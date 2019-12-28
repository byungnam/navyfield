using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Sailor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler{

   
    private Image img;

    private int listPos;

    public enum SailorCls
    {
        captain,
        engine,
        gunner,
        repair
    }

	void Start () {
        img = GetComponent<Image>();
    }

    public void setImage(Sprite spr)
    {
        GetComponent<Image>().sprite = spr;
    }

    /*
    public void loadImage(SailorCls sailor)
    {
        switch(sailor)
        {
            case SailorCls.captain:
                GetComponent<Image>().sprite = captain;
                break;
            case SailorCls.engine:
                GetComponent<Image>().sprite = engine;
                break;
            case SailorCls.gunner:
                GetComponent<Image>().sprite = gunner;
                break;
            case SailorCls.repair:
                GetComponent<Image>().sprite = repair;
                break;
        }
    }
    */

    public int getListPos()
    {
        return listPos;
    }

    public void setListPos(int listPos)
    {
        this.listPos = listPos;
    }

    void Update () {
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        print("dragging");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("Sailor Leave");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Sailor Enter");
    }
}
