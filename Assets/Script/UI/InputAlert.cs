using UnityEngine;
using UnityEngine.UI;

public class InputAlert : MonoBehaviour
{
    public PlayerController playerController;

    public Image upImage;
    public Image leftImage;
    public Image downImage;
    public Image rightImage;

    public Image attackImage;
    public Image debuffImage;
    public Image shieldImage;
    public Image strikeImage;

    private void Start()
    {
        upImage.enabled    = false;
        leftImage.enabled  = false;
        downImage.enabled  = false;
        rightImage.enabled = false;
    }

    private void Update()
    {
        upImage.enabled     = Input.GetKey(playerController.upButton);
        leftImage.enabled   = Input.GetKey(playerController.leftButton);
        downImage.enabled   = Input.GetKey(playerController.downButton);
        rightImage.enabled  = Input.GetKey(playerController.rightButton);
    }
}
