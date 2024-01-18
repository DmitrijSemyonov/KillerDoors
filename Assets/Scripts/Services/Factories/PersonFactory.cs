using KillerDoors.Common;
using KillerDoors.Services.AssetManagement;
using KillerDoors.Services.StaticDataSpace;
using UnityEngine;

namespace KillerDoors.Services.Factories
{
    public class PersonFactory : IPersonFactory
    {
        private const string LowSpeedPersonPath = "Persons/LowSpeedPerson";
        private const string MiddleSpeedPersonPath = "Persons/MiddleSpeedPerson";
        private const string HighSpeedPersonPath = "Persons/HighSpeedPerson";

        private readonly IAssetProvider _assetProvider;
        private readonly IStaticDataService _staticDataService;

        public PersonFactory(IAssetProvider assetProvider, IStaticDataService staticDataService)
        {
            _assetProvider = assetProvider;
            _staticDataService = staticDataService;
        }

        public Person CreatePerson(PersonType personType, Vector3 at)
        {
            Person person = null;
            switch (personType)
            {
                case PersonType.LowSpeed:
                    person = _assetProvider.Instantiate<Person>(LowSpeedPersonPath, at, Quaternion.identity.eulerAngles);
                    break;
                case PersonType.MiddleSpeed:
                    person = _assetProvider.Instantiate<Person>(MiddleSpeedPersonPath, at, Quaternion.identity.eulerAngles);
                    break;
                case PersonType.HighSpeed:
                    person = _assetProvider.Instantiate<Person>(HighSpeedPersonPath, at, Quaternion.identity.eulerAngles);
                    break;
            }
            person.Construct(_staticDataService);
            return person;
        }
    }
}