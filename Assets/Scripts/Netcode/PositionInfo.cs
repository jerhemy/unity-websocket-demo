using System;
using UnityEngine;

namespace Netcode
{
    [Serializable]
    public class PositionInfo
    {
        public string owner;
        public Vector3 position;
        public Vector3 rotation;
    }
}