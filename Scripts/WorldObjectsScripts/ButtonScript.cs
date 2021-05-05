using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private GameObject pres;
    public bool isPressed, isOpen;

    private float down_Time = 0.05f, up_Time = 0.05f;
    private float current_Up_Time, current_Down_Time;

    private GameObject player;
    private Camera mainCam;

    private float distance;
    public float interaction_Distance = 1.5f;

    public GameObject text_Window;

    void Awake()
    {
        pres = gameObject.transform.GetChild(0).gameObject;
        player = GameObject.FindWithTag(Tags.PLAYER_TAG);
        mainCam = Camera.main;
        //text_Window = player.transform.GetChild(2).GetChild(2).gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray();
        if (isPressed)
        {
            PressButton();
        }
        if (isOpen)
        {
            GetComponentInParent<GateScript>().Open();
        }
    }

    void Ray()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit)
            && Vector3.Distance(transform.position, player.transform.position) < interaction_Distance)
        {
            print(hit);
            text_Window.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                isPressed = true;
                gameObject.GetComponent<AudioSource>().Stop();
            }
        }
        else
        {
            text_Window.SetActive(false);
        }
    }

    void PressButton()
    {
        isOpen = true;
        if (current_Down_Time < down_Time)
        {
            gameObject.transform.Translate(Vector3.down * Time.deltaTime * 2);
            pres.transform.Translate(Vector3.up * Time.deltaTime * 2);
            current_Down_Time += Time.deltaTime;
            pres.GetComponent<AudioSource>().Play();
        }
        else if (current_Up_Time < up_Time)
        {
            gameObject.transform.Translate(Vector3.up * Time.deltaTime * 2);
            pres.transform.Translate(Vector3.down * Time.deltaTime * 2);
            current_Up_Time += Time.deltaTime;
        }

        if (current_Down_Time >= down_Time && current_Up_Time >= up_Time)
        {
            current_Down_Time = 0;
            current_Up_Time = 0;
            isPressed = false;
        }
    }
}
