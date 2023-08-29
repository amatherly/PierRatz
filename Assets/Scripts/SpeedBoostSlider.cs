using UnityEngine;
using UnityEngine.UI;

public class SpeedBoostSlider : MonoBehaviour
{
    [Header("UI Components")]
    public Slider speedBoostSlider;  // The UI Slider
    public Image boostZoneImage;  // The Image component representing the boost zone
    public float hotZoneWidth = 0.2f;  // The width of the hot zone on the slider

    [Header("Speed")]
    public float normalSpeed = 5f;
    public float boostedSpeed = 10f;
    public float speedBoostDuration = 2f;  // How long the speed boost lasts

    private float hotZoneMin;  // The start of the hot zone on the slider
    private float hotZoneMax;  // The end of the hot zone on the slider

    private bool isBoosted = false;
    private float boostEndTime;

    private float direction = 1f;

    private void Start()
    {
        RandomizeHotZone();
    }

    void Update()
    {
        speedBoostSlider.value += Time.deltaTime * direction;
        if (speedBoostSlider.value >= 1f || speedBoostSlider.value <= 0f)
        {
            direction *= -1f;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (speedBoostSlider.value >= hotZoneMin && speedBoostSlider.value <= hotZoneMax)
            {
                Debug.Log("You hit it!");
                GameManager.GAME.Player.ActivateSpeedBoost();
                this.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("You missed it!");
                this.gameObject.SetActive(false);
                RandomizeHotZone();
            }
        }
    }


    void RandomizeHotZone()
    {
        hotZoneMin = Random.Range(0f, 1f - hotZoneWidth);
        hotZoneMax = hotZoneMin + hotZoneWidth;
        RectTransform rt = boostZoneImage.rectTransform;
        rt.anchorMin = new Vector2(hotZoneMin, 0);
        rt.anchorMax = new Vector2(hotZoneMax, 1);
    }
}
