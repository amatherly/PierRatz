using UnityEngine;
using UnityEngine.UI;

public class SpeedBoostSlider : MonoBehaviour
{
    [Header("UI Components")]
    public Slider speedBoostSlider;
    public Image boostZoneImage;
    public float hotZoneWidth = 0.2f;

    private float hotZoneMin;
    private float hotZoneMax;
    private float direction = 1f;

    private bool isBoosted = false;
    private float boostEndTime;


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
                FindObjectOfType<PlayerController>().ActivateSpeedBoost();
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
