using UnityEngine;

/**
 * This component spawns the given laser-prefab whenever the player clicks a given key.
 * It also updates the "scoreText" field of the new laser.
 */
public class LaserShooter : ClickSpawner
{
    [SerializeField]
    [Tooltip("How many points to add to the shooter, if the laser hits its target")]
    int pointsToAdd = 1;

    // A reference to the field that holds the score that has to be updated when the laser hits its target.
    NumberField scoreField;


    private void Start()
    {
        //scoreField = GetComponentInChildren<NumberField>();
        //if (!scoreField);
        //    Debug.LogError($"No child of {gameObject.name} has a NumberField component!");
        GameObject otherObject = GameObject.Find("PlayerSpaceship1");  // Replace with the target object's name
        scoreField = otherObject.GetComponentInChildren<NumberField>();
    }

    protected override GameObject spawnObject()
    {

        if (CompareTag("Player"))
        {
            if (IsBombOutsideCamera())
            {
                return null; // Do not spawn if a bomb is already near the camera borders
            }
        }

        GameObject newObject = base.spawnObject();  // base = super
        if (CompareTag("Bomb"))
        {
            Destroy(gameObject, 0.01f);  // Give time for the other lasers to be destroyd
            //gameObject.GetComponent<Renderer>().enabled = false;
        }

        // Modify the text field of the new object.
        ScoreAdder newObjectScoreAdder = newObject.GetComponent<ScoreAdder>();
        if (newObjectScoreAdder)
            newObjectScoreAdder.SetScoreField(scoreField).SetPointsToAdd(pointsToAdd);

        return newObject;
    }

    private bool IsBombOutsideCamera()
    {
        // Get all bombs in the scene
        GameObject[] bombs = GameObject.FindGameObjectsWithTag("Bomb"); // Make sure bombs have the "Bomb" tag
        Camera mainCamera = Camera.main;

        foreach (GameObject bomb in bombs)
        {
            Vector3 screenPosition = mainCamera.WorldToViewportPoint(bomb.transform.position);

            // Check if the bomb is outside the camera's viewport (between 0 and 1 in both x and y)
            if (screenPosition.y < 1)
            {
                return true; // A bomb is outside the camera's borders
            }
        }

        return false; // No bomb outside the camera's borders
    }
}
