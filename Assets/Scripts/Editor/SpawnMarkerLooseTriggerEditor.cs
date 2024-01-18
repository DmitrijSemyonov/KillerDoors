using UnityEngine;
using UnityEditor;
using KillerDoors.SceneDependency;
namespace EditorSpace
{

    [CustomEditor(typeof(SpawnMarkerLooseTrigger))]
    public class SpawnMarkerLooseTriggerEditor : Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmo(SpawnMarkerLooseTrigger spawner, GizmoType gizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawner.transform.position, 1f);
        }
    }
}