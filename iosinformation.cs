using iMobileDevice;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using Prizrak;
using System;
using System.Collections.Generic;


namespace PurpleBuddy
{
    class iosinformation
    {

        AppleDevices apple_device_mods = new AppleDevices();

        private struct NORMAL_MODE_IDEVICE_INFO
        {
            public string ecid;             
        }


        public string returnstr_device_ecid(iDeviceHandle device_handle)
        {
            var lockdown = LibiMobileDevice.Instance.Lockdown;
            LockdownClientHandle lockdowndevice;
            lockdown.lockdownd_client_new_with_handshake(device_handle, out lockdowndevice, "Ghost");
            NORMAL_MODE_IDEVICE_INFO normal_info = new NORMAL_MODE_IDEVICE_INFO();

            string return_ecid = null;

            try
            {
                lockdown.lockdownd_get_device_udid(lockdowndevice, out normal_info.ecid);
                return_ecid = normal_info.ecid.Split('-')[1];
            }
            catch (Exception)
            {
                if (string.IsNullOrWhiteSpace(normal_info.ecid))

                    return "";
            }
           
            return return_ecid;
        }
    }
}
