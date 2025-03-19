using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UniqueID))]
public class UniqueIDEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        UniqueID uniqueID = (UniqueID)target;

        if (GUILayout.Button("Generate Incremental ID"))
        {
            GenerateIncrementalID(uniqueID);
        }
    }

    private void GenerateIncrementalID(UniqueID uniqueID)
    {
        string baseName = uniqueID.gameObject.name;
        int count = 1;

        var allObjects = GameObject.FindObjectsOfType<UniqueID>();
        foreach (var obj in allObjects)
        {
            if (obj != uniqueID && obj.ID != null && obj.ID.StartsWith(baseName))
            {
                string numberPart = obj.ID.Replace(baseName + "_", "");
                if (int.TryParse(numberPart, out int num))
                {
                    if (num >= count)
                        count = num + 1;
                }
            }
        }

        uniqueID.ID = $"{baseName}_{count}";
        EditorUtility.SetDirty(uniqueID);
        Debug.Log($"Generated ID: {uniqueID.ID}");
    }
}
