using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PowerSelectorUI : MonoBehaviour
{
    [SerializeField]
    Image topImage, rightImage, bottomImage, leftImage;
    [SerializeField]
    Color defaultColor, topColor, rightColor, bottomColor, leftColor;
    [SerializeField]
    Canvas canvas;

    bool changedPower;


    Image lastImage;
    Vector2 lastInput;

    private void Start()
    {
        lastImage = topImage;
        topImage.color = defaultColor;
        rightImage.color = defaultColor;
        bottomImage.color = defaultColor;
        leftImage.color = defaultColor;
    }

    public void OpenSelector(InputAction.CallbackContext context)
    {
        if (GloopMain.Instance.MyMovement.MyBase.GroundedAmount == 0)
            return;
        //return;
        if (context.started)
        {
            //WwisePlay SlowMainSoundtrack and lower volume
            EnableCanvas(true);
            changedPower = false;
        }
        else if (context.canceled && canvas.enabled == true)
        {
            //WwisePlay ContinueSoundtrack at normal speed and volume
            EnableCanvas(false);
            if (!changedPower)
                ChangeState(lastInput);
        }
    }

    public void ChangeState(Vector2 dir)
    {
        int stateIndex = 0;
        if (dir.x > 0.5f)
        {
            stateIndex = 2;
        }
        else if (dir.x < -0.5f)
        {
            stateIndex = 4;
        }
        else if (dir.y > 0.5f)
        {
            stateIndex = 1;
        }
        else if (dir.y < -0.5f)
        {
            stateIndex = 3;
        }
        EMode mode = EMode.NONE;
        switch (stateIndex)
        {
            case 0:
                break;
            case 1:
                mode = EMode.DASH;
                break;
            case 2:
                mode = EMode.GLIDE;
                break;
            case 3:
                mode = EMode.GRAVITY;
                break;
            case 4:
                mode = EMode.GRAPPLE;
                break;
            default:
                break;
        }
        if (mode == EMode.NONE)
            return;
        GloopMain.Instance.CurrentMode = mode;
    }

    public void EnableCanvas(bool enable)
    {
        canvas.enabled = enable;

        if (canvas.enabled)
        {
            GameManager.Instance.CursorUnlockers++;
            GloopMain.Instance.LockInput(true);
            GameManager.Instance.TimeScalers++;
        }
        else
        {
            GameManager.Instance.CursorUnlockers--;
            GloopMain.Instance.LockInput(false);
            GameManager.Instance.TimeScalers--;
        }
    }

    public void SelectPower(InputAction.CallbackContext context)
    {
        if (context.canceled)
            return;
        Vector2 dir = context.ReadValue<Vector2>().normalized;
        lastInput = dir;
        if (dir.x > 0.5f)
        {
            Highlight(rightImage, rightColor);
        }
        else if (dir.x < -0.5f)
        {
            Highlight(leftImage, leftColor);
        }
        else if (dir.y > 0.5f)
        { 
            Highlight(topImage, topColor);
        }
        else if (dir.y < -0.5f)
        {
            Highlight(bottomImage, bottomColor);
        }
    }

    public void ButtonSelect(int index)
    {
        switch (index)
        {
            case 0:
                break;
            case 1:
                GloopMain.Instance.CurrentMode = EMode.DASH;
                break;
            case 2:
                GloopMain.Instance.CurrentMode = EMode.GLIDE;
                break;
            case 3:
                GloopMain.Instance.CurrentMode = EMode.GRAVITY;
                break;
            case 4:
                GloopMain.Instance.CurrentMode = EMode.GRAPPLE;
                break;
            default:
                break;
        }
        changedPower = true;
    }

    private void Highlight(Image image, Color hColor)
    {
        lastImage.color = defaultColor;
        lastImage = image;
        lastImage.color = hColor;
    }
}
