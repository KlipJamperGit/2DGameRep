using UnityEngine;

public class ControlPerlin : MonoBehaviour
{
    public int width = 256;                       // Ширина текстури
    public int height = 256;                      // Висота текстури
    [Range(1, 100)]
    public float scale = 20f;                     // Масштаб шуму
    [Range(0, 1)]
    public float threshold = 0.5f;                // Поріг для визначення кольору
    [Range(1, 10)]
    public float frequency = 1.0f;                // Частота шуму
    [Range(0.1f, 1.0f)]
    public float amplitude = 1.0f;                // Амплітуда шуму
    public Material material;                      // Матеріал, до якого застосовуємо текстуру

    private Texture2D texture;

    void Start()
    {
        texture = new Texture2D(width, height);
        GetComponent<Renderer>().material = material;

        GenerateCaves(); // Генерація печер при старті
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
                // Обчислюємо координати для перлинного шуму з урахуванням частоти і амплітуди
                float xCoord = (float)x / width * scale * frequency;
                float yCoord = (float)y / height * scale * frequency;

                // Генерація значення перлинного шуму з амплітудою
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord) * amplitude;

                // Визначення кольору на основі шуму
                Color color = noiseValue > threshold ? Color.black : Color.white;

                texture.SetPixel(x, y, color); // Встановлюємо піксель у текстурі
            }
        }
        texture.Apply(); // Застосовуємо зміни до текстури
        material.mainTexture = texture; // Застосовуємо текстуру до матеріалу
    }
}
