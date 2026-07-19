using System.Collections.Generic;
using UnityEngine;
        using UnityEngine.UI;

public class SquareGrid : MonoBehaviour
    {
        [Header("四角を配置する白枠")]
        public RectTransform squareArea;

        [Header("配置する四角のPrefab")]
        public GameObject squarePrefab;

        [Header("マスの設定")]
        public int columns = 2;
        public int rows = 4;

        [Header("黒線の太さに合わせる")]
        public float spacing = 5f;

        [Header("外側の余白")]
        public int paddingLeft = 0;
        public int paddingRight = 0;
        public int paddingTop = 0;
        public int paddingBottom = 0;

        private GridLayoutGroup gridLayout;

        public Transform parentTransform;

    [SerializeField] private List<GameObject> objectList = new List<GameObject>();

    void Start()
        {
            CreateSquares();
            //           AddObject(parentTransform);

            for (int i = 0; i < columns * rows; i++)
            {
               // AddObject(objectList[i].gameObject.transform);

            }
        FuncPutImage(5);
        }
    public void FuncPutImage(int index)
    {
        AddObject(objectList[index].gameObject.transform);
    }
        void CreateSquares()
        {
            if (squareArea == null)
            {
                Debug.LogError("Square Areaが設定されていません");
                return;
            }

            if (squarePrefab == null)
            {
                Debug.LogError("Square Prefabが設定されていません");
                return;
            }

            // GridLayoutGroupがなければ追加
            gridLayout = squareArea.GetComponent<GridLayoutGroup>();

            if (gridLayout == null)
            {
                gridLayout = squareArea.gameObject.AddComponent<GridLayoutGroup>();
            }

            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = columns;

            gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft;
            gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal;

            gridLayout.childAlignment = TextAnchor.UpperLeft;
            gridLayout.spacing = new Vector2(spacing, spacing);

            gridLayout.padding = new RectOffset(
                paddingLeft,
                paddingRight,
                paddingTop,
                paddingBottom
            );

            // 白枠の大きさ
            float areaWidth = squareArea.rect.width;
            float areaHeight = squareArea.rect.height;

            // 1マスの横幅
            float cellWidth =
                (areaWidth
                - paddingLeft
                - paddingRight
                - spacing * (columns - 1))
                / columns;

            // 1マスの縦幅
            float cellHeight =
                (areaHeight
                - paddingTop
                - paddingBottom
                - spacing * (rows - 1))
                / rows;

            gridLayout.cellSize = new Vector2(cellWidth, cellHeight);

            // すでに入っている四角を削除
            for (int i = squareArea.childCount - 1; i >= 0; i--)
            {
                Destroy(squareArea.GetChild(i).gameObject);
            }

            // 左上から順番に配置
            int squareCount = columns * rows;

            for (int i = 0; i < squareCount; i++)
            {
                GameObject newSquare =
                    Instantiate(squarePrefab, squareArea);

                newSquare.name = "Square_" + (i + 1);

                Debug.Log(newSquare.name);

                objectList.Add(newSquare);
            }
        }
        
    void AddObject(Transform parentTransform)
    {
        GameObject newImageObject = new GameObject("NewImage");

        // インスペクターから親にしたいオブジェクトを指定します
        // public Transform parentTransform;
        //指定したゲームオブジェクトの子供として新しいゲームオブジェクト(image)を追加する
        // 1. 新しいGameObjectを作成し、名前を「NewImage」にする
        // GameObject newImageObject = new GameObject("NewImage");
        // 2. 作成したオブジェクトを親要素の子にする（世界座標を維持しない設定）
        newImageObject.transform.SetParent(parentTransform, false);
        // 3. UIのImageコンポーネントを追加する
        Image imageComponent = newImageObject.AddComponent<Image>();
        imageComponent.color = Color.red;
    }
}