using KillerDoors.Common;
using UnityEngine;

namespace KillerDoors.Services.Factories
{
    public interface IPersonFactory : IService
    {
        Person CreatePerson(PersonType personType, Vector3 at);
    }
}