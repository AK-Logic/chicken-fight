using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class GridManager : MonoBehaviour {
    [SerializeField] private int _width, _height;
 
    [SerializeField] private Tile _tilePrefab;
 
    [SerializeField] private Transform _cam;
 
    private Dictionary<Vector3, Tile> _tiles;
 
    void Start() {
        GenerateGrid();
    }
 
    void GenerateGrid() {
        _tiles = new Dictionary<Vector3, Tile>();
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y,0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
 
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);
 
 
                _tiles[new Vector3(x, y, 0)] = spawnedTile;
            }
        }
 

 // Lets move the camera onto the grid.

    Camera mainCamera = Camera.main;

    if (mainCamera != null)
    {
        // Adjust this multiplier to control the visual coverage of the grid
        float orthoSizeMultiplier = 2.8f;

        // Set the orthographic size based on the grid dimensions
        mainCamera.orthographicSize = Mathf.Max(_width, _height) * 0.5f * orthoSizeMultiplier;

        // Center the camera on the grid
        mainCamera.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }
    else
    {
        Debug.LogError("Main camera not found!");
    }
    }

    
 
    public Tile GetTileAtPosition(Vector3 pos) {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }
}