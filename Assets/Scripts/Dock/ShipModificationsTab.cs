using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipModificationsTab : MonoBehaviour {

    /*
    public Sprite spr_turret;
    public Sprite spr_control_tower;
    public Sprite spr_anti_air;
    public Sprite spr_upper_armor;
    public Sprite spr_side_armor;

    private GameObject turret;
    private GameObject control_tower;
    private GameObject anti_air;
    private GameObject upper_armor;
    private GameObject side_armor;
    */

    public Text hp_text;
    public Slider hp_slider;
    public float hp;

    public Text caliber_text;
    public Slider caliber_slider;
    public float caliber;

    public Text deck_armor_text;
    public Slider deck_armor_slider;
    public float deck_armor;

    public Text side_armor_text;
    public Slider side_armor_slider;
    public float side_armor;

    public Text max_speed_text;
    public Slider max_speed_slider;
    public float max_speed;

    public Text acceleration_text;
    public Slider acceleration_slider;
    public float acceleration;

    public Text max_rudder_angle_text;
    public Slider max_rudder_angle_slider;
    public float max_rudder_angle;

    public Text rudder_turn_speed_text;
    public Slider rudder_turn_speed_slider;
    public float rudder_turn_speed;

    
    void Start () {
        /*
        turret = new GameObject("turret");
        turret.transform.SetParent(transform);
        turret.AddComponent<Image>().sprite = spr_turret;

        control_tower = new GameObject("control_tower");
        control_tower.transform.SetParent(transform);
        control_tower.AddComponent<Image>().sprite = spr_control_tower;

        anti_air = new GameObject("anti_air");
        anti_air.transform.SetParent(transform);
        anti_air.AddComponent<Image>().sprite = spr_anti_air;

        upper_armor = new GameObject("upper_armor");
        upper_armor.transform.SetParent(transform);
        upper_armor.AddComponent<Image>().sprite = spr_upper_armor;

        side_armor = new GameObject("side_armor");
        side_armor.transform.SetParent(transform);
        side_armor.AddComponent<Image>().sprite = spr_side_armor;

        turret.transform.localPosition = new Vector3(-100, 300, 0);
        control_tower.transform.localPosition = new Vector3(-100, 150, 0);
        anti_air.transform.localPosition = new Vector3(-100, 0, 0);
        upper_armor.transform.localPosition = new Vector3(-100, -150, 0);
        side_armor.transform.localPosition = new Vector3(-100, -300, 0);
        */

        hp_text_update(0);
        caliber_text_update(0);
        deck_armor_text_update(0);
        side_armor_text_update(0);
        max_speed_text_update(0);
        acceleration_text_update(0);
        max_rudder_angle_text_update(0);
        rudder_turn_speed_text_update(0);
    }

    void Update () {
		
	}

    public void hp_text_update(float value)
    {
        hp_text.text = "HP " + Mathf.RoundToInt(5000 + value * 35000);
        hp = (float) Mathf.RoundToInt(5000 + value * 35000);
    }

    public void caliber_text_update(float value)
    {
        caliber_text.text = "Caliber " + System.Math.Round((3 + value * 15) * 25.4)  + " mm";
        caliber = (float) System.Math.Round((3 + value * 15) * 25.4);
    }

    public void deck_armor_text_update(float value)
    {
        deck_armor_text.text = "Deck armor " + (0 + System.Math.Round(value * 200, 2)) + " mm";
        deck_armor = (float)(0 + System.Math.Round(value * 200, 2));
    }

    public void side_armor_text_update(float value)
    {
        side_armor_text.text = "Side armor " + (0 + System.Math.Round(value * 200, 2)) + " mm";
        side_armor = (float)(0 + System.Math.Round(value * 200, 2));
    }

    public void max_speed_text_update(float value)
    {
        max_speed_text.text = "Max speed " + (10 + System.Math.Round(value * 30, 1)) + " knot";
        max_speed = (float)(10 + System.Math.Round(value * 30, 1));
    }

    public void acceleration_text_update(float value)
    {
        acceleration_text.text = "Acceleration " + (1 + System.Math.Round(value * 4, 2)) + " knot/s";
        acceleration = (float)(1 + System.Math.Round(value * 4, 2));
    }

    public void max_rudder_angle_text_update(float value)
    {
        max_rudder_angle_text.text = "Max rudder angle " + (1 + System.Math.Round(value * 39, 1)) + " deg";
        max_rudder_angle = (float)(1 + System.Math.Round(value * 39, 1));
    }

    public void rudder_turn_speed_text_update(float value)
    {
        rudder_turn_speed_text.text = "Rudder turn speed " + (0.1 + System.Math.Round(value * 9.9, 1)) + " deg/s";
        rudder_turn_speed = (float)(0.1 + System.Math.Round(value * 9.9, 1));
    }
}
