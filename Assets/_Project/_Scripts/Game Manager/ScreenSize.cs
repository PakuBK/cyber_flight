using UnityEngine;

namespace CF {

public static class ScreenSize
{
    public static float GetScreenToWorldHeight
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var height = edgeVector.y * 2;
            return height;
        }
    }
    public static float GetScreenToWorldWidth
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            var width = edgeVector.x * 2;
            return width;
        }
    }

    public static float GetScreenToWorldBottom
    {
        get
        {
            Vector2 bottomLeftCorner = new Vector2(0, 0);
            Vector2 edgeVector = Camera.main.ViewportToWorldPoint(bottomLeftCorner);
            var height = edgeVector.y * 2;
            return height;
        }
    }

    public static float GetPixelScreenToWorldHeight
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ScreenToWorldPoint(topRightCorner);
            var height = edgeVector.y * 2;
            return height;
        }
    }

    public static float GetPixelScreenToWorldWidth
    {
        get
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 edgeVector = Camera.main.ScreenToWorldPoint(topRightCorner);
            var width = edgeVector.x * 2;
            return width;
        }
    }
}
}

