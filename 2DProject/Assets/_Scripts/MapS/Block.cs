using UnityEngine;

public class Block : MonoBehaviour
{
    public TileManager tileManager; // ����� �� ��'��� TileManager
    public Camera mainCamera; // ����� �� ������� ������ ��� ���������� ������� � ���
    public int tileIndexToPlace = 0; // ������ ����� ��� ����������� (����� �������� � ���������)

    void Update()
    {
        // ������� ����� ��� ��������� ��� ������ ����
        if (Input.GetMouseButtonDown(0)) // 0 - ��� ������ ����
        {
            Vector3 worldPosition = GetMouseWorldPosition();
            tileManager.BreakTile(worldPosition);
        }

        // ����������� ����� ��� ��������� ����� ������ ����
        if (Input.GetMouseButtonDown(1)) // 1 - ����� ������ ����
        {
            Vector3 worldPosition = GetMouseWorldPosition();
            tileManager.PlaceTile(worldPosition, tileIndexToPlace);
        }
    }

    // ����� ��� ��������� ������� ���� � ����������� ����
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane; // ������������ ������� �� ������
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }
}
