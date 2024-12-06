using UnityEngine;

public class Block : MonoBehaviour
{
    public TileManager tileManager; // Силка на об'єкт TileManager
    public Camera mainCamera; // Силка на основну камеру для визначення позиції в світі
    public int tileIndexToPlace = 0; // Індекс тайлу для поставлення (можна змінювати в інспекторі)

    void Update()
    {
        // Ламання тайлу при натисканні лівої кнопки миші
        if (Input.GetMouseButtonDown(0)) // 0 - ліва кнопка миші
        {
            Vector3 worldPosition = GetMouseWorldPosition();
            tileManager.BreakTile(worldPosition);
        }

        // Поставлення тайлу при натисканні правої кнопки миші
        if (Input.GetMouseButtonDown(1)) // 1 - права кнопка миші
        {
            Vector3 worldPosition = GetMouseWorldPosition();
            tileManager.PlaceTile(worldPosition, tileIndexToPlace);
        }
    }

    // Метод для отримання позиції миші в координатах світу
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane; // Встановлюємо відстань від камери
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }
}
