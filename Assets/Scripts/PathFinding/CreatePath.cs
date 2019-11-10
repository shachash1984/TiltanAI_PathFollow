using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreatePath : MonoBehaviour
{
    public bool bDebug = true;
    public float Radius = 2.0f;
    //public Vector3[] points;

    //The object we want to add
    public GameObject wayPoint;

    //Whats the radiusBrush of the circle we will add objects inside of?
    public float radiusBrush = 1f;

    //How many GOs will we add each time we press a button?
    public int howManyObjects = 1;

    //Should we add or remove objects within the circle
    public enum Actions { AddObjects, RemoveObjects }

    public Actions action;

    [SerializeField]
    private List<Vector3> _waypoints = new List<Vector3>();


    public int Length { get { return _waypoints.Count; } }

    public Vector3 GetPoint(int index)
    {
        return _waypoints[index];
    }

    void OnDrawGizmos()
    {
        if (!bDebug)
        {
            return;
        }

        for (int i = 0; i < _waypoints.Count; i++)
        {
            if (i + 1 < _waypoints.Count)
            {
                Debug.DrawLine(_waypoints[i], _waypoints[i + 1], Color.red);
            }
        }
    }

    //Add a prefab that we instantiated in the editor script
    public void AddPrefab(GameObject newPrefabObj, Vector3 center)
    {
        //Get a random position within a circle in 2d space
        Vector2 randomPos2D = Random.insideUnitCircle * radiusBrush;

        //But we are in 3d, so make it 3d and move it to where the center is
        Vector3 randomPos = new Vector3(randomPos2D.x, 0f, randomPos2D.y) + center;

        newPrefabObj.transform.position = randomPos;

        newPrefabObj.transform.parent = transform;

        _waypoints.Add(newPrefabObj.transform.position);
    }

    //Remove objects within the circle
    public void RemoveObjects(Vector3 center)
    {
        //Get an array with all children to this transform
        GameObject[] allChildren = GetAllChildren();

        foreach (GameObject child in allChildren)
        {
            //If this child is within the circle
            if (Vector3.SqrMagnitude(child.transform.position - center) < radiusBrush * radiusBrush)
            {
                Vector3 forDelete = _waypoints.Where(v => v == child.transform.position).FirstOrDefault();
                _waypoints.Remove(forDelete);
                DestroyImmediate(child);
                
            }
        }
    }

    //Remove all objects
    public void RemoveAllObjects()
    {
        //Get an array with all children to this transform
        GameObject[] allChildren = GetAllChildren();

        //Now destroy them
        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child);
        }

        _waypoints.Clear();
    }

    //Get an array with all children to this GO
    private GameObject[] GetAllChildren()
    {
        //This array will hold all children
        GameObject[] allChildren = new GameObject[transform.childCount];

        //Fill the array
        int childCount = 0;
        foreach (Transform child in transform)
        {
            allChildren[childCount] = child.gameObject;
            childCount += 1;
        }

        return allChildren;
    }
}
