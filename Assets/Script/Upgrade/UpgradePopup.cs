using UnityEngine;

namespace Upgrade
{
    public class UpgradePopup : Popup
    {
        [SerializeField] private BaseUpgrade baseUpgrade;
        
        protected override void Init()
        {
            base.Init();
            LoadData();
        }

        private void LoadData()
        {
            
        }
    }
}
