using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Sakemottekoi.MainGame.Tetris
{
    [RequireComponent(typeof(Tilemap))]
    public class TetrisRenderer: MonoBehaviour
    {
        private Tilemap tilemap;
        [SerializeField] TileBase black;
        [SerializeField] TileBase orangeBlock;
        [SerializeField] TileBase greenBlock;

        private void Awake()
        {
            tilemap = GetComponent<Tilemap>();
        }

        public void Render(IReadOnlyList<IReadOnlyList<TetrisBlock>> fieldData, Vector2Int offset)
        {
            for (int y = 0; y < fieldData.Count; y++)
            {
                for (int x = 0; x < fieldData[y].Count; x++)
                {
                    // 塗りつぶす座標
                    Vector3Int pos = new(x + offset.x, y + offset.y);

                    // 座標にあるデータをもとに色を塗る。
                    switch (fieldData[y][x])
                    {
                        case TetrisBlock.None:
                            tilemap.SetTile(pos, black);
                            break;

                        case TetrisBlock.Orange:
                            tilemap.SetTile(pos, orangeBlock);
                            break;

                        case TetrisBlock.Green:
                            tilemap.SetTile(pos, greenBlock);
                            break;
                    }
                }
            }
        }
    }
}
