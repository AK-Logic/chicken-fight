using UnityEngine;

public class InitialAreaCapture : MonoBehaviour
{
    [Header("Initial Area Settings")]
    [SerializeField] private float initialAreaSize = 3f; // Adjust based on the initial area size
    [SerializeField] private Color initialSquareColor = Color.green; // Adjust based on the color for captured squares
    [SerializeField] private LayerMask squareLayer;

    // Reference to the grid line material
    [SerializeField] private Material gridLineMaterial;

    private void Start()
    {
        CaptureInitialArea();
    }

    private void CaptureInitialArea()
    {
        // Calculate the position of the character in grid coordinates
        Vector3 characterPosition = transform.position;
        Vector3Int characterGridPosition = new Vector3Int(
            Mathf.FloorToInt(characterPosition.x / GridGenerator.gridCellSize),
            Mathf.FloorToInt(characterPosition.y / GridGenerator.gridCellSize),
            Mathf.FloorToInt(characterPosition.z / GridGenerator.gridCellSize)
        );

        // Get colliders in the initial area around the character
        Collider2D[] colliders = Physics2D.OverlapBoxAll(characterPosition, new Vector2(initialAreaSize, initialAreaSize), 0, squareLayer);

        foreach (Collider2D collider in colliders)
        {
            // Get the position of the square in grid coordinates
            Vector3 squarePosition = collider.transform.position;
            Vector3Int squareGridPosition = new Vector3Int(
                Mathf.FloorToInt(squarePosition.x / GridGenerator.gridCellSize),
                Mathf.FloorToInt(squarePosition.y / GridGenerator.gridCellSize),
                Mathf.FloorToInt(squarePosition.z / GridGenerator.gridCellSize)
            );

            // Check if the square is within the initial area around the character
            if (Mathf.Abs(squareGridPosition.x - characterGridPosition.x) <= 1 &&
                Mathf.Abs(squareGridPosition.y - characterGridPosition.y) <= 1)
            {
                // Change the color of the square to indicate it's captured
                Renderer squareRenderer = collider.GetComponent<Renderer>();
                squareRenderer.material.color = initialSquareColor;

                // Add grid line effect to the captured square
                AddGridLineEffect(squareRenderer);
            }
        }
    }

    private void AddGridLineEffect(Renderer renderer)
    {
        // Check if the renderer already has a material
        if (renderer.material == null)
        {
            Debug.LogWarning("Renderer has no material.");
            return;
        }

        // Create a new material instance for the grid line effect
        Material gridLineMaterialInstance = new Material(gridLineMaterial);

        // Assign the material to the renderer
        renderer.material = gridLineMaterialInstance;

        // Adjust the grid line material properties as needed
        // For example, you can modify the shader or color properties

        // Example: Set the grid line color to black
        gridLineMaterialInstance.SetColor("_GridLineColor", Color.white);
    }
}
