using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// カスタム属性を定義
public class EnumIndexAttribute : PropertyAttribute
{
    public string[] names;
    public EnumIndexAttribute(System.Type enumType)
    {
        names = System.Enum.GetNames(enumType);
    }
}

#if UNITY_EDITOR
// Inspectorの描画を書き換える処理
[CustomPropertyDrawer(typeof(EnumIndexAttribute))]
public class EnumIndexDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumIndexAttribute enumIndex = (EnumIndexAttribute)attribute;

        // プロパティのパス（例: "stocks.Array.data[0]"）からインデックス番号を抽出
        string path = property.propertyPath;
        int startIndex = path.LastIndexOf('[') + 1;
        int endIndex = path.LastIndexOf(']');

        if (startIndex > 0 && endIndex > startIndex)
        {
            if (int.TryParse(path.Substring(startIndex, endIndex - startIndex), out int index) && index < enumIndex.names.Length)
            {
                // "Element 0" を Enum の名前に書き換える
                label.text = enumIndex.names[index];
            }
        }

        EditorGUI.PropertyField(position, property, label, true);
    }
}
#endif