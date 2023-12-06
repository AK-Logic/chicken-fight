using UnityEngine;

public class TrailWriter : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1.0f;
    private Vector2 movement;
    private Animator animator;

    // Trail variables
    public int trailPoints = 50; // Adjust the number of points in the trail
    public float trailWidth = 0.1f; // Adjust the width of the trail
    public float distanceThreshold = 0.1f; // Adjust the distance threshold for adding points
    public float checkInterval = 0.1f; // Adjust the interval for checking the specified distance
    public GameObject trailPrefab; // Assign the sprite prefab in the inspector
    private GameObject[] trailObjects;
    private int trailIndex = 0;
    private Vector3 lastCharacterPosition;
    private float distanceCheckTimer = 0f;

    // Added variable for character transform
    public Transform characterTransform;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Trail initialization
        trailObjects = new GameObject[trailPoints];
        lastCharacterPosition = characterTransform.position;

        for (int i = 0; i < trailPoints; i++)
        {
            trailObjects[i] = Instantiate(trailPrefab, transform.position, Quaternion.identity);
            trailObjects[i].SetActive(false);

            // Attach a collider and script to each trail object
            var collider = trailObjects[i].AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            var trailColliderScript = trailObjects[i].AddComponent<TrailColliderScript>();
            trailColliderScript.trailWriter = this;
        }
    }

    void Update()
    {
        // Check for the specified distance every x milliseconds
        distanceCheckTimer += Time.deltaTime;

        if (distanceCheckTimer >= checkInterval)
        {
            if (Vector3.Distance(characterTransform.position, lastCharacterPosition) > distanceThreshold)
            {
                // Set the position of the trail object
                trailObjects[trailIndex].transform.position = characterTransform.position;
                trailObjects[trailIndex].SetActive(true);
                trailObjects[trailIndex].GetComponent<TrailColliderScript>().SetTimestamp(Time.time);

                // Increment the trail index
                trailIndex = (trailIndex + 1) % trailPoints;
            }

            lastCharacterPosition = characterTransform.position;
            distanceCheckTimer = 0f; // Reset the timer
        }

        // Your existing movement logic
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        animator.SetFloat("Speed", Mathf.Abs(movement.magnitude * movementSpeed));

        bool flipped = movement.x > 0;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180 : 0f, 0f));
    }

    private void FixedUpdate()
    {
        var xMovement = movement.x * movementSpeed * Time.deltaTime;
        var yMovement = movement.y * movementSpeed * Time.deltaTime;

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
        {
            this.transform.Translate(new Vector3(xMovement, 0), Space.World);
        }
        else if (Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            this.transform.Translate(new Vector3(0, yMovement), Space.World);
        }
    }

    // Added method to get the index of a specific trail object
    public int GetTrailIndex(GameObject trailObject)
    {
        for (int i = 0; i < trailPoints; i++)
        {
            // Compare the instanceIDs of the GameObjects
            if (trailObjects[i].GetInstanceID() == trailObject.GetInstanceID())
            {
                return i;
            }
        }

        // If the trail object is not found, return an invalid index (e.g., -1)
        return -1;
    }

    public void ClearTrail()
    {
        for (int i = 0; i < trailPoints; i++)
        {
            trailObjects[i].SetActive(false);
        }

        trailIndex = 0;
    }


    public void FillArea()
    {
        // Only fill the area if there are enough trail points
        if (trailIndex >= 3)
        {
            // Create a new game object for the filled area
            GameObject filledArea = new GameObject("FilledArea");
            filledArea.transform.position = Vector3.zero; // Adjust as needed

            // Add a MeshFilter and MeshRenderer to the filled area game object
            MeshFilter meshFilter = filledArea.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = filledArea.AddComponent<MeshRenderer>();

            // Create a mesh for the filled area
            Mesh filledMesh = new Mesh();

            // Set the vertices of the mesh to be the trail points
            Vector3[] vertices = new Vector3[trailIndex];
            for (int i = 0; i < trailIndex; i++)
            {
                vertices[i] = trailObjects[i].transform.position;
            }

            // Set the triangles of the mesh to create a polygon
            int[] triangles = new int[(trailIndex - 2) * 3];
            for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
            {
                triangles[i] = 0;
                triangles[i + 1] = j + 1;
                triangles[i + 2] = j + 2;
            }

            // Set the mesh properties
            filledMesh.vertices = vertices;
            filledMesh.triangles = triangles;

            // Assign the filled mesh to the MeshFilter
            meshFilter.mesh = filledMesh;

            // Set the material of the filled area (you can adjust the material as needed)
            meshRenderer.material = new Material(Shader.Find("Standard"));

            // Optionally, adjust the color of the filled area
            meshRenderer.material.color = Color.green; // Change the color as needed

            // Optionally, set the sorting layer and order to ensure it's rendered correctly
            meshRenderer.sortingLayerName = "Foreground"; // Change the sorting layer as needed
            meshRenderer.sortingOrder = 1; // Change the sorting order as needed
        }
    }
}
