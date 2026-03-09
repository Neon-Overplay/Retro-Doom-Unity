using UnityEngine;

public class DebugTimeControl : MonoBehaviour
{
    // The multiplier for fast forwarding the game
    public float timeMultiplier1x = 2.0f;
    public float timeMultiplier2x = 5.0f;

    // The multiplier for slowing down the game
    public float timeMultiplierSlow = 0.5f;

    // The keys to toggle fast forward and slow motion
    public KeyCode toggleKey1x = KeyCode.Z;
    public KeyCode toggleKey2x = KeyCode.LeftAlt;
    public KeyCode toggleKeySlow = KeyCode.V;

    // The current time multiplier
    private float currentMultiplier = 1.0f;

    // The normal game time scale
    private float normalTimeScale = 1.0f;


    private void Awake()
    {
        Time.timeScale = normalTimeScale;
    }

    void Update()
    {
        if (true) //ChecklistAccess.IsGlobalTestingEnabled())
        {
            // Check if the first toggle key is pressed
            if (Input.GetKeyDown(toggleKey1x))
            {
                // Toggle between normal and 2x fast forward
                if (currentMultiplier == normalTimeScale)
                {
                    currentMultiplier = timeMultiplier1x;
                }
                else if (currentMultiplier == timeMultiplier1x)
                {
                    currentMultiplier = normalTimeScale;
                }

                Time.timeScale = currentMultiplier;
            }

            // Check if the second toggle key is pressed
            if (Input.GetKeyDown(toggleKey2x))
            {
                // Toggle between normal and 5x fast forward
                if (currentMultiplier == normalTimeScale)
                {
                    currentMultiplier = timeMultiplier2x;
                }
                else if (currentMultiplier == timeMultiplier2x)
                {
                    currentMultiplier = normalTimeScale;
                }

                Time.timeScale = currentMultiplier;
            }

            // Check if the slow motion toggle key is pressed
            if (Input.GetKeyDown(toggleKeySlow))
            {
                // Toggle between normal and 0.5x slow motion
                if (currentMultiplier == normalTimeScale)
                {
                    currentMultiplier = timeMultiplierSlow;
                }
                else if (currentMultiplier == timeMultiplierSlow)
                {
                    currentMultiplier = normalTimeScale;
                }

                Time.timeScale = currentMultiplier;
            }
        }
    }
}
