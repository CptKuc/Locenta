using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Look : MonoBehaviour
{
    [SerializeField]
    private Transform playerRoot, lookRoot;
    
    [SerializeField]
    private bool invert;

    [SerializeField]
    private bool can_Unlock = true;

    [SerializeField]
    private float sensivity = 5f;

    [SerializeField]
    private int smooth_Steps = 10;

    [SerializeField]
    private float smooth_Weight = 0.4f;

    [SerializeField]
    private float roll_Angle = 10f;

    [SerializeField]
    private float roll_Speed = 3f;


    [SerializeField]
    private Vector2 default_look_Limits = new Vector2(-70f, 80f);

    private Vector2 look_Angles;
    private Vector2 current_Mouse_Look;

    private GameObject menu;
    //private Vector2 smooth_Move;

    //private float current_roll_Angle;
    //private int last_look_Frame;

    // Start is called before the first frame update
    void Awake()
    {
        menu = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        lock_unlock_Curson();

        if(Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }
    }

    void lock_unlock_Curson()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                PauseGame();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ResumeGame();
            }
        }
    }

    void LookAround()
    {
        current_Mouse_Look = new Vector2(
            Input.GetAxis(MouseAxis.MOUSE_Y), Input.GetAxis(MouseAxis.MOUSE_X));

        look_Angles.x += current_Mouse_Look.x * sensivity * (invert ? 1f : -1f);
        look_Angles.y += current_Mouse_Look.y * sensivity;

        look_Angles.x = Mathf.Clamp(look_Angles.x, default_look_Limits.x, default_look_Limits.y);

        //current_roll_Angle =
        //    Mathf.Lerp(current_roll_Angle, Input.GetAxisRaw(MouseAxis.MOUSE_X)
        //                * roll_Angle, Time.deltaTime * roll_Speed);

        lookRoot.localRotation = Quaternion.Euler(look_Angles.x, 0f, 0f);
        playerRoot.localRotation = Quaternion.Euler(0f, look_Angles.y, 0f);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
