using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] private int gridWidth = 50;
    [SerializeField] private int gridHeight = 50;
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private Color evenSquareColor = Color.white;
    [SerializeField] private Color oddSquareColor = Color.gray;
    [SerializeField] private string squareSortingLayer = "Default"; // Adjust as needed

    [Header("Grid Settings")]
    [SerializeField] private float gridCellSize = 1f;  // Set your default value here

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

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));

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
                square.name = $"Square ({x + 1}, {y + 1})";
            }
        }
    }
}