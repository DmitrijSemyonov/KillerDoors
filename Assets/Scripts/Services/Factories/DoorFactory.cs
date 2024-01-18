using KillerDoors.Common;
using KillerDoors.Services.AssetManagement;
using KillerDoors.Services.StaticDataSpace;
using KillerDoors.StaticDataSpace;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace KillerDoors.Services.Factories
{
    public class DoorFactory : IDoorFactory
    {
        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;
        private const string DoorPath = "Door/Door";

        public DoorFactory(IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }
        public List<Door> CreateDoors()
        {
            LevelStaticData level = _staticDataService.ForLevel(SceneManager.GetActiveScene().name);

            List<Door> result = new List<Door>();
            foreach (DoorStaticData doorStaticData in level.doorsDatas)
            {
                Door door = _assetProvider.Instantiate<Door>(DoorPath, doorStaticData.position, doorStaticData.rotation);
                door.Init(doorStaticData);
                result.Add(door);
            }
            return result;
        }
    }
}