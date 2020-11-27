using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    SpriteRenderer sr;
    Texture2D baseTexture;
    Texture2D cloneTexture;
    float pixelsPerUnit = 100.0f;
    public int r = 25;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseTexture = sr.sprite.texture;
        cloneTexture = Instantiate(baseTexture);

        sr.sprite = Sprite.Create(cloneTexture,
            new Rect(0,0,cloneTexture.width, cloneTexture.height), Vector2.zero);

        gameObject.AddComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MakeAHole();
            Debug.Log("clicked");
        }
    }
    void MakeAHole()
    {
        Vector2Int center = World2Pixel(Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            transform.position.z - Camera.main.transform.position.z)));
        //int r = 25;

        int px, nx, py, ny, d;
        for (int i = 0; i <= r; i++)
        {
            d = Mathf.RoundToInt(Mathf.Sqrt(r * r - i * i));
            for (int j = 0; j <= d; j++)
            {
                px = center.x + i;
                nx = center.x - i;
                py = center.y + j;
                ny = center.y - j;

                cloneTexture.SetPixel(px, py, Color.clear);
                cloneTexture.SetPixel(nx, py, Color.clear);
                cloneTexture.SetPixel(px, ny, Color.clear);
                cloneTexture.SetPixel(nx, ny, Color.clear);
            }
        }
        cloneTexture.Apply();

        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }
    Vector2Int World2Pixel(Vector2 pos)
    {
        Vector2Int v = Vector2Int.zero;

        var dx = (pos.x - transform.position.x);
        var dy = pos.y - transform.position.y;

        v.x = Mathf.RoundToInt(dx * pixelsPerUnit);
        v.y = Mathf.RoundToInt(dy * pixelsPerUnit);

        return v;
    }
}
