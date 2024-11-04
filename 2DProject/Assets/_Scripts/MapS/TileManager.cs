using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public Tilemap tilemap; // ����� �� Tilemap, ��� �� �������������
    public TileBase[] buildableTiles; // ����� �����, �� ����� �������������

    // ������� ��'���� �����
    public void BreakTile(Vector3 worldPosition)
    {
        Vector3Int tilePosition = tilemap.WorldToCell(worldPosition); // ��������� ������� ����� � ����������� Tilemap
        TileBase tile = tilemap.GetTile(tilePosition); // ��������� ����� �� ��� �������

        if (tile != null)
        {
            tilemap.SetTile(tilePosition, null); // ����� ����� �� "��������" ������
        }
    }

    // ������������ ������ ��'���� �����
    public void PlaceTile(Vector3 worldPosition, int tileIndex)
    {
        if (tileIndex < 0 || tileIndex >= buildableTiles.Length) return; // �������� ����������� �������

        Vector3Int tilePosition = tilemap.WorldToCell(worldPosition); // ��������� ������� ����� � ����������� Tilemap
        tilemap.SetTile(tilePosition, buildableTiles[tileIndex]); // ������������ ������ �����
    }
}