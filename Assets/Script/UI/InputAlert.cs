using UnityEngine;
using UnityEngine.UI;

public class InputAlert : MonoBehaviour
{
    public PlayerController playerController;

    public Image upImage;
    public Image leftImage;
    public Image downImage;
    public Image rightImage;

    public GameObject attackImage;
    public GameObject debuffImage;
    public GameObject shieldImage;
    public GameObject judgementImage;

    private void Start()
    {
        SetActiveFalse();
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver)
        {
            SetActiveFalse();
            return;
        }

        upImage.enabled    = Input.GetKey(playerController.upButton);
        leftImage.enabled  = Input.GetKey(playerController.leftButton);
        downImage.enabled  = Input.GetKey(playerController.downButton);
        rightImage.enabled = Input.GetKey(playerController.rightButton);

        attackImage.SetActive(Input.GetKey(playerController.attackButton));
        debuffImage.SetActive(Input.GetKey(playerController.debuffButton));
        shieldImage.SetActive(Input.GetKey(playerController.shieldButton));
        judgementImage.SetActive(Input.GetKey(playerController.judgementButton));
    }

    private void SetActiveFalse()
    {
        upImage.enabled    = false;
        leftImage.enabled  = false;
        downImage.enabled  = false;
        rightImage.enabled = false;

        attackImage.SetActive(false);
        debuffImage.SetActive(false);
        shieldImage.SetActive(false);
        judgementImage.SetActive(false);
    }
}
