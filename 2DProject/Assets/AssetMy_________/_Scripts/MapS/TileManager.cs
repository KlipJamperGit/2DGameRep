using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap tilemap; // Силка на Tilemap, яку ми використовуємо
    public TileBase[] buildableTiles; // Масив тайлів, які можна встановлювати

    // Ламання об'єкта тайлу
    public void BreakTile(Vector3 worldPosition)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(worldPosition); // Отримання позиції тайлу в координатах Tilemap
        TileBase tile = tilemap.GetTile(tilePosition); // Отримання тайлу на цій позиції

        if (tile != null)
        {
            tilemap.SetTile(tilePosition, null); // Заміна тайлу на "зламаний" варіант
        }
    }

    // Встановлення нового об'єкта тайлу
    public void PlaceTile(Vector3 worldPosition, int tileIndex)
    {
        if (tileIndex < 0 || tileIndex >= buildableTiles.Length) return; // Перевірка допустимості індексу

        Vector3Int tilePosition = tilemap.WorldToCell(worldPosition); // Отримання позиції тайлу в координатах Tilemap
        tilemap.SetTile(tilePosition, buildableTiles[tileIndex]); // Встановлення нового тайлу
    }
}