using UnityEngine;

public class ConveyorLineManager : MonoBehaviour
{
    [Header("Initial Direction")]
    [SerializeField] private bool startLookingRight = false;

    [Header("Conveyor Segments")]
    [SerializeField] private GameObject smallBlueConveyor;
    [SerializeField] private GameObject regularBlueConveyor;
    [SerializeField] private GameObject smallRedConveyor;
    [SerializeField] private GameObject regularRedConveyor;

    [Header("Direction Buttons")]
    [SerializeField] private GlowingButton redButton;
    [SerializeField] private GlowingButton blueButton;

    // Current direction: true = right (90°), false = left (-90°)
    private bool isLookingRightNow;

    void Start()
    {
        // Assign button click listeners
        redButton.onMouseDown += OnRedButtonClicked;
        blueButton.onMouseDown += OnBlueButtonClicked;

        //OnGameStart();
    }

    public void OnGameStart()
    {
        // Apply initial state
        SetConveyorLineDirection(startLookingRight);
    }

    public void OnRedButtonClicked()
    {
        if (!isLookingRightNow)
        {
            SetConveyorLineDirection(true); // change to right
        }
    }

    public void OnBlueButtonClicked()
    {
        if (isLookingRightNow)
        {
            SetConveyorLineDirection(false); // change to left
        }
    }

    private void SetConveyorLineDirection(bool lookRight)
    {
        isLookingRightNow = lookRight;

        float yRotation = lookRight ? 90f : -90f;
        Quaternion targetRotation = Quaternion.Euler(0f, yRotation, 0f);

        // Rotate all conveyor segments
        smallBlueConveyor.transform.rotation = targetRotation;
        regularBlueConveyor.transform.rotation = targetRotation;
        smallRedConveyor.transform.rotation = targetRotation;
        regularRedConveyor.transform.rotation = targetRotation;

        // Update button visuals
        redButton.SetButtonActive(lookRight);
        blueButton.SetButtonActive(!lookRight);
    }

    public bool getCurrentDirection()
    {
        return isLookingRightNow;
    }
}