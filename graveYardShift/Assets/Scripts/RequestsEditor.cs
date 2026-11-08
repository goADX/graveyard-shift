using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Requests))]
public class RequestsEditor: Editor
{



    private SerializedProperty requester;
    private SerializedProperty charecterRequester;
    private SerializedProperty monsterTypeRequester;

    private SerializedProperty requestType;
    private SerializedProperty lineNumber;
    private SerializedProperty exactLocation;
    private SerializedProperty charecterRelatedToRequest;

    private SerializedProperty requestText;

    private void OnEnable()
    {
        requester = serializedObject.FindProperty("requester");
        charecterRequester = serializedObject.FindProperty("charecterRequester");
        monsterTypeRequester = serializedObject.FindProperty("monsterTypeRequester");

        requestType = serializedObject.FindProperty("requestType");
        lineNumber = serializedObject.FindProperty("LineNumber");
        exactLocation = serializedObject.FindProperty("exactLocation");
        charecterRelatedToRequest =
            serializedObject.FindProperty("charecterRelatedToRequest");

        requestText = serializedObject.FindProperty("requestText");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Message Requester", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(
            requester,
            new GUIContent("Requester", "Who can make this request?")
        );

        switch ((Requests.Requester)requester.enumValueIndex)
        {
            case Requests.Requester.TypeOfCharecter:
                EditorGUILayout.PropertyField(
                    monsterTypeRequester,
                    new GUIContent("Monster Type", "The required character type.")
                );
                break;

            case Requests.Requester.Charecter:
                EditorGUILayout.PropertyField(
                    charecterRequester,
                    new GUIContent("Character", "The specific character making the request.")
                );
                break;
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Request Type", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(requestType);

        switch ((Requests.RequestType)requestType.enumValueIndex)
        {
            case Requests.RequestType.MoveToExactLocation:
                EditorGUILayout.PropertyField(
                    exactLocation,
                    new GUIContent("Exact Location", "The destination grid coordinates.")
                );
                break;

            case Requests.RequestType.MoveToHorizontalLine:
            case Requests.RequestType.MoveToVerticalLine:
                EditorGUILayout.PropertyField(
                    lineNumber,
                    new GUIContent("Line Number", "The target horizontal or vertical line.")
                );
                break;

            case Requests.RequestType.MoveNextToCharecter:
            case Requests.RequestType.MoveAwayFromCharecter:
                EditorGUILayout.PropertyField(
                    charecterRelatedToRequest,
                    new GUIContent("Related Character", "The character used as the destination reference.")
                );
                break;
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Message", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Request Text");

        requestText.stringValue = EditorGUILayout.TextArea(
            requestText.stringValue,
            GUILayout.MinHeight(80)
        );

        serializedObject.ApplyModifiedProperties();
    }
}

