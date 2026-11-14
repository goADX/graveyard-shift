using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Requests))]
public class RequestsEditor : Editor
{
    private SerializedProperty requester;
    private SerializedProperty charecterRequester;
    private SerializedProperty monsterTypeRequester;

    private SerializedProperty requestsDoneRule;
    private SerializedProperty requestsFailedRule;

    private SerializedProperty requestType;
    private SerializedProperty lineNumber;
    private SerializedProperty exactLocation;
    private SerializedProperty charecterRelatedToRequest;

    private SerializedProperty weight;

    private SerializedProperty requestDuration;
    private SerializedProperty stayWhenDone;
    private SerializedProperty stayWhenFailed;

    private SerializedProperty scoreToGainOnDone;
    private SerializedProperty scoreToLoseOnFail;

    private SerializedProperty requestText;

    private void OnEnable()
    {
        requester = serializedObject.FindProperty("requester");
        charecterRequester = serializedObject.FindProperty("charecterRequester");
        monsterTypeRequester = serializedObject.FindProperty("monsterTypeRequester");

        requestsDoneRule = serializedObject.FindProperty("RequestsDoneRule");
        requestsFailedRule = serializedObject.FindProperty("RequestsFailedRule");

        requestType = serializedObject.FindProperty("requestType");
        lineNumber = serializedObject.FindProperty("LineNumber");
        exactLocation = serializedObject.FindProperty("exactLocation");
        charecterRelatedToRequest =
            serializedObject.FindProperty("charecterRelatedToRequest");

        weight = serializedObject.FindProperty("weight");

        requestDuration = serializedObject.FindProperty("requestDuration");
        stayWhenDone = serializedObject.FindProperty("StayWhenDone");
        stayWhenFailed = serializedObject.FindProperty("StayWhenFailed");

        scoreToGainOnDone = serializedObject.FindProperty("scoreToGainOnDone");
        scoreToLoseOnFail = serializedObject.FindProperty("scoreToLoseOnFail");

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
        EditorGUILayout.LabelField("Follow-up Requests", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(
            requestsDoneRule,
            new GUIContent("Done Rule", "Request can only come if this request is done.")
        );
        EditorGUILayout.PropertyField(
            requestsFailedRule,
            new GUIContent("Failed Rule", "Request can only come if this request fails.")
        );

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
        EditorGUILayout.PropertyField(
            weight,
            new GUIContent("Weight", "Relative chance of this request being picked.")
        );

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Duration", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(
            requestDuration,
            new GUIContent("Request Duration", "In whole days.")
        );
        EditorGUILayout.PropertyField(stayWhenDone);
        EditorGUILayout.PropertyField(stayWhenFailed);
        if (requestDuration.intValue > 0 && !stayWhenDone.boolValue && !stayWhenFailed.boolValue)
        {
            EditorGUILayout.HelpBox(
                "You need to enable at least one stayer for the duration to work.",
                MessageType.Warning
            );
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Scores updates", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(scoreToGainOnDone);
        EditorGUILayout.PropertyField(scoreToLoseOnFail);

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