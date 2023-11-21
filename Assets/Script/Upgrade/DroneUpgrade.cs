using Drone;
using UnityEngine;

namespace Upgrade
{
    [CreateAssetMenu(menuName = "Upgrade/DroneUpgrade")]
    public class DroneUpgrade : BaseUpgrade
    {
        public enum DroneUpgradeType
        {
            MoveSpeed,
            RotateSpeed,
            Damage
        }

        public DroneUpgradeType droneUpgradeType;
        public UpgradeGroup[] upgrades;
        internal int i;
        
    }
}
