using UnityEngine;
using UnityEditor;
using KillerDoors.SceneDependency;
namespace EditorSpace
{

    [CustomEditor(typeof(SpawnMarkerSpawnPoint))]
    public class SpawnMarkerSpawnPointEditor : Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SpawnMarkerSpawnPoint spawner, GizmoType gizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawner.transform.position, 1f);
        }
    }
}