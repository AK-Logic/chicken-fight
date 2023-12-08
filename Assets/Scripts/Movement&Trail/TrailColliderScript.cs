using UnityEngine;

public class TrailColliderScript : MonoBehaviour
{
    public TrailWriter trailWriter;

    private float timestamp = 0f;

    public void SetTimestamp(float time)
    {
        timestamp = time;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has a TrailWriter component
        TrailWriter otherTrailWriter = other.GetComponent<TrailWriter>();

        // If the colliding object has a TrailWriter and it's the same as the current object's TrailWriter, handle the collision
        if (otherTrailWriter != null && otherTrailWriter.gameObject == trailWriter.gameObject)
        {
            // Get the timestamp of the collided trail object
            float collidedTimestamp = timestamp;

            // Set the maximum allowed time difference in milliseconds
            float maxTimeDifference = 1000f; // Change this value to your desired maximum time difference

            // Get the current time
            float currentTime = Time.time;

            // Calculate the time difference
            float timeDifference = Mathf.Abs(currentTime - collidedTimestamp) * 1000f; // Convert to milliseconds

            // Check if the time difference is greater than the maximum allowed time difference
            if (timeDifference > maxTimeDifference)
            {
                // Log the collision for debugging
                Debug.Log("Character touched its own trail with timestamp difference: " + timeDifference + " milliseconds");

                // Fill the area surrounded by the trail
                trailWriter.FillArea();
                trailWriter.ClearTrail();


                // Add your collision handling logic here
            }
        }
    }
}
