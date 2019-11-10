using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreatePath))]
public class WaypointManagerEditor : Editor
{
    private CreatePath _path;

    //The _center of the circle
    private Vector3 _center;

    private void OnEnable()
    {
        _path = target as CreatePath;

        //Hide the handles of the GO so we dont accidentally move it instead of moving the circle
        Tools.hidden = true;
    }

    private void OnDisable()
    {
        //Unhide the handles of the GO
        Tools.hidden = false;
    }

    private void OnSceneGUI()
    {
        //Move the circle when moving the mouse
        //A ray from the mouse position
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //Where did we hit the ground?
            _center = hit.point;

            //Need to tell Unity that we have moved the circle or the circle may be displayed at the old position
            SceneView.RepaintAll();
        }


        //Display the circle
        Handles.color = Color.white;

        Handles.DrawWireDisc(_center, Vector3.up, _path.radiusBrush);


        //Add or remove objects with left mouse click

        //First make sure we cant select another gameobject in the scene when we click
        HandleUtility.AddDefaultControl(0);

        //Have we clicked with the left mouse button?
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            //Should we add or remove objects?
            if (_path.action == CreatePath.Actions.AddObjects)
            {
                AddNewPrefabs();

                MarkSceneAsDirty();
            }
            else if (_path.action == CreatePath.Actions.RemoveObjects)
            {
                _path.RemoveObjects(_center);

                MarkSceneAsDirty();
            }
        }
    }

    //Add buttons this scripts inspector
    public override void OnInspectorGUI()
    {
        //Add the default stuff
        DrawDefaultInspector();

        //Remove all objects when pressing a button
        if (GUILayout.Button("Remove all objects"))
        {
            //Pop-up so you don't accidentally remove all objects
            if (EditorUtility.DisplayDialog("Safety check!", "Do you want to remove all objects?", "Yes", "No"))
            {
                _path.RemoveAllObjects();

                MarkSceneAsDirty();
            }
        }
    }

    //Force unity to save changes or Unity may not save when we have instantiated/removed prefabs despite pressing save button
    private void MarkSceneAsDirty()
    {
        UnityEngine.SceneManagement.Scene activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(activeScene);
    }

    //Instantiate prefabs at random positions within the circle
    private void AddNewPrefabs()
    {
        //How many prefabs do we want to add
        int howManyObjects = _path.howManyObjects;

        //Which prefab to we want to add
        GameObject prefabGO = _path.wayPoint;

        for (int i = 0; i < howManyObjects; i++)
        {
            GameObject newGO = PrefabUtility.InstantiatePrefab(prefabGO) as GameObject;

            //Send it to the main script to add it at a random position within the circle
            _path.AddPrefab(newGO, _center);
        }
    }
}
