using KillerDoors.SceneDependency;
using KillerDoors.StaticDataSpace;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace EditorSpace
{

    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelStaticData = (LevelStaticData)target;

            if (GUILayout.Button("Collect Data"))
            {
                levelStaticData.doorsDatas = FindObjectsOfType<SpawnMarkerDoor>().Select(marker => new DoorStaticData(
                    marker.GetComponent<UniqueId>().id,
                    marker.transform.position,
                    marker.transform.rotation.eulerAngles,
                    marker.doorStaticData.baseOpenTime,
                    marker.doorStaticData.baseCloseTime,
                    marker.doorStaticData.doorUpgradeViewOffset))
                  .ToList();

                levelStaticData.looseTriggerPosition = FindObjectOfType<SpawnMarkerLooseTrigger>().transform.position;

                levelStaticData.spawnPointsDatas = FindObjectsOfType<SpawnMarkerSpawnPoint>().Select(marker => new SpawnPointStaticData(
                    marker.GetComponent<UniqueId>().id,
                    marker.transform.position,
                    marker.spawnPointStaticData.floorNumber))
                    .OrderBy(data => data.floorNumber)
                    .ToList();

                levelStaticData.levelKey = SceneManager.GetActiveScene().name;
            }

            EditorUtility.SetDirty(target);
        }
    }
}