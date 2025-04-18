using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject cratePrefab;

    public int gridMin = -7;
    public int gridMax = 7;

    void Start()
    {
        for (int x = gridMin; x <= gridMax; x++)
        {
            for (int z = gridMin; z <= gridMax; z++)
            {
                if (IsInCorner(x, z)) continue;

                // Border or even-even = crate block
                if (x == gridMin || x == gridMax || z == gridMin || z == gridMax || (x % 1 == 0 && z % 1 == 0))
                {
                    Vector3 position = new Vector3(x, 0.9f, z); // Adjust Y if needed
                    Instantiate(cratePrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
    bool IsInCorner(int x, int z)
    {
        // 2x2 corner clearance (bottom-left, bottom-right, top-left, top-right)
        bool bottomLeft = (x == gridMin && z == gridMin) || (x == gridMin + 1 && z == gridMin) || (x == gridMin && z == gridMin + 1) || (x == gridMin + 1 && z == gridMin + 1);
        bool bottomRight = (x == gridMax && z == gridMin) || (x == gridMax - 1 && z == gridMin) || (x == gridMax && z == gridMin + 1) || (x == gridMax - 1 && z == gridMin + 1);
        bool topLeft = (x == gridMin && z == gridMax) || (x == gridMin + 1 && z == gridMax) || (x == gridMin && z == gridMax - 1) || (x == gridMin + 1 && z == gridMax - 1);
        bool topRight = (x == gridMax && z == gridMax) || (x == gridMax - 1 && z == gridMax) || (x == gridMax && z == gridMax - 1) || (x == gridMax - 1 && z == gridMax - 1);

        return bottomLeft || bottomRight || topLeft || topRight;
    }
}
