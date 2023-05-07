/// ksPark
/// 
/// InitPoolingManager 스크립트
/// Inspector 창에서 보이는 형태 디자인해 주는 스크립트

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof(InitPoolingManager))]
public class InitPoolingManagerEditor : Editor
{
    SerializedProperty poolList;

    ReorderableList list;

    private void OnEnable()
    {
        poolList = serializedObject.FindProperty("poolList"); // poolList 배열 호출

        // list 그려주는 디자인 설정
        list = new ReorderableList(serializedObject, poolList, true, true, true, true);

        list.drawElementCallback = DrawListItems; // draw 콜백
        list.drawHeaderCallback = DrawHeader; // header 콜백
        list.onAddCallback = AddItem; // add 버튼 콜백
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(); // 맨 위에 한 줄 여백

        list.DoLayoutList(); // list 그리기

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// 리스트 안 디자인
    /// </summary>
    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

        // obj 값 그리기
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y + 2, 140, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("obj"),
            GUIContent.none
        );

        // Count 그리기
        EditorGUI.LabelField(new Rect(rect.x + 155, rect.y + 2, 100, EditorGUIUtility.singleLineHeight), "Count");

        // count 값 그리기
        EditorGUI.PropertyField(
            new Rect(rect.x + 200, rect.y + 2, 50, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("count"),
            GUIContent.none
        ); 

    }

    /// <summary>
    /// 리스트 헤더 디자인
    /// </summary>
    void DrawHeader(Rect rect)
    {
        string name = "Pool Objects"; // 헤더 이름
        EditorGUI.LabelField(rect, name);
    }

    /// <summary>
    /// Add 이벤트
    /// </summary>
    void AddItem(ReorderableList list)
    {
        int index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;

        list.index = index;

        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

        element.FindPropertyRelative("obj").objectReferenceValue = null; // obj 초기 값 : null
        element.FindPropertyRelative("count").intValue = 5; // count 초기 값 : 5
    }
}
