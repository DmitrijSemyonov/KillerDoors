using UnityEngine;
using UnityEditor;
using KillerDoors.SceneDependency;
namespace EditorSpace
{

    [CustomEditor(typeof(SpawnMarkerDoor))]
    public class SpawnMarkerDoorEditor : Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SpawnMarkerDoor spawner, GizmoType gizmo)
        {
            Gizmos.color = Color.red;

            Vector3 cube = new Vector3(1f, 15f, 1f);
            Gizmos.DrawCube(spawner.transform.position + spawner.transform.up * cube.y / 2f, cube);
            Gizmos.DrawCube(spawner.transform.position + spawner.transform.up * cube.y / 2f - spawner.transform.right * 5f, cube);
        }
    }
}