using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private int gridWidth = 50;
    [SerializeField] private int gridHeight = 50;
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private GameObject fencePrefab;
    [SerializeField] private Color evenSquareColor = Color.white;
    [SerializeField] private Color oddSquareColor = Color.gray;
    [SerializeField] private string squareSortingLayer = "Default"; // Adjust as needed[]
    [SerializeField] private string fenceSortingLayer = "Default";

    [Header("Grid Settings")]
    [SerializeField] public static float gridCellSize = 1f;  // Set your default value here

    private void Start()
    {
        GenerateSquares();
    }

    private void GenerateSquares()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        // Pass the gridCellSize to TrailWriter
        TrailWriter trailWriter = FindObjectOfType<TrailWriter>();  // Assuming there's only one TrailWriter in the scene
        if (trailWriter != null)
        {
            trailWriter.SetGridCellSize(gridCellSize);
        }

        Vector3 bottomLeft = new Vector3(0, 0, 0);

        // Create borders using the fencePrefab
        CreateBorder(bottomLeft, gridWidth + 2, 1);
        CreateBorder(bottomLeft + new Vector3(0, (gridHeight + 1) * gridCellSize, 0), gridWidth + 2, 1);
        CreateBorder(bottomLeft, 1, gridHeight + 1);
        CreateBorder(bottomLeft + new Vector3((gridWidth + 1) * gridCellSize, 0, 0), 1, gridHeight + 1);


        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 spawnPosition = new Vector3(bottomLeft.x + x, bottomLeft.y + y, 0);
                GameObject square = Instantiate(squarePrefab, spawnPosition, Quaternion.identity);

                // Set alternating colors based on x and y
                Color squareColor = (x + y) % 2 == 0 ? evenSquareColor : oddSquareColor;

                // Apply color to the square
                square.GetComponent<Renderer>().material.color = squareColor;

                // Set sorting layer for the square
                square.GetComponent<Renderer>().sortingLayerName = squareSortingLayer;

                // Add Box Collider 2D with Is Trigger set to true
                BoxCollider2D boxCollider = square.AddComponent<BoxCollider2D>();
                boxCollider.isTrigger = true;

                // Set the name of the square based on its coordinates
                square.name = $"Square ({x}, {y + 1})";
            }
        }
    }
    private void CreateBorder(Vector3 position, int gridWidth, int gridHeight)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Calculate spawn position for the fence
                Vector3 spawnPosition = new Vector3(position.x + (x - 1) * gridCellSize, position.y + (y - 1) * gridCellSize, 0);

                // Instantiate the fence prefab
                GameObject fence = Instantiate(fencePrefab, spawnPosition, Quaternion.identity);

                // Set the name of the fence based on its coordinates
                fence.name = $"Fence ({x + 1}, {y + 1})";

                //
                fence.GetComponent<Renderer>().sortingLayerName = fenceSortingLayer;
                fence.GetComponent<Renderer>().sortingOrder = 1;


                // Add Box Collider 2D to the fence
                BoxCollider2D boxCollider = fence.AddComponent<BoxCollider2D>();
                boxCollider.isTrigger = true;
            }
        }
    }

}