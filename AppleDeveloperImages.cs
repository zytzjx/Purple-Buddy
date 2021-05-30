using iMobileDevice;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using System.IO;


namespace PurpleBuddy
{
    class AppleDeveloperImages
    {
        private static string apple_developer_dmg_location = Directory.GetCurrentDirectory() + "\\" + "Assets\\DeveloperImages";

        public void mount_developer_image(iDeviceHandle device_handle, string ios_version)
        {
            var ImageMounter = LibiMobileDevice.Instance.MobileImageMounter;
            var lockdown     = LibiMobileDevice.Instance.Lockdown;           
        }
    }
}
