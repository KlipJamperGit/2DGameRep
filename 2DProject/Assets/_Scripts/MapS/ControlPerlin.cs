using UnityEngine;

public class ControlPerlin : MonoBehaviour
{
    public int width = 256;                       // ������ ��������
    public int height = 256;                      // ������ ��������
    [Range(1, 100)]
    public float scale = 20f;                     // ������� ����
    [Range(0, 1)]
    public float threshold = 0.5f;                // ���� ��� ���������� �������
    [Range(1, 10)]
    public float frequency = 1.0f;                // ������� ����
    [Range(0.1f, 1.0f)]
    public float amplitude = 1.0f;                // �������� ����
    public Material material;                      // �������, �� ����� ����������� ��������

    private Texture2D texture;

    void Start()
    {
        texture = new Texture2D(width, height);
        GetComponent<Renderer>().material = material;

        GenerateCaves(); // ��������� ����� ��� �����
    }

    private void Update()
    {
        GenerateCaves();
    }

    void GenerateCaves()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // ���������� ���������� ��� ���������� ���� � ����������� ������� � ��������
                float xCoord = (float)x / width * scale * frequency;
                float yCoord = (float)y / height * scale * frequency;

                // ��������� �������� ���������� ���� � ���������
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord) * amplitude;

                // ���������� ������� �� ����� ����
                Color color = noiseValue > threshold ? Color.black : Color.white;

                texture.SetPixel(x, y, color); // ������������ ������ � �������
            }
        }
        texture.Apply(); // ����������� ���� �� ��������
        material.mainTexture = texture; // ����������� �������� �� ��������
    }
}
