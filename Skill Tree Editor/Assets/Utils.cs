using UnityEngine;
public static class Utils
{
    public static Vector2[] GetNeighbours(Vector2 position)
    {
        Vector2[] positions = new Vector2[8];

        positions[0] = new Vector2(position.x + MainManager.SPACING, position.y);
        positions[1] = new Vector2(position.x + MainManager.SPACING, position.y + MainManager.SPACING);
        positions[2] = new Vector2(position.x, position.y + MainManager.SPACING);
        positions[3] = new Vector2(position.x - MainManager.SPACING, position.y + MainManager.SPACING);
        positions[4] = new Vector2(position.x - MainManager.SPACING, position.y);
        positions[5] = new Vector2(position.x - MainManager.SPACING, position.y - MainManager.SPACING);
        positions[6] = new Vector2(position.x, position.y - MainManager.SPACING);
        positions[7] = new Vector2(position.x + MainManager.SPACING, position.y - MainManager.SPACING);

        return positions;
    }
}
