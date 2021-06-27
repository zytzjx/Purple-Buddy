using iMobileDevice;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using Prizrak;
using System;
using System.Collections.Generic;


namespace PurpleBuddy
{
    /// <summary>
    /// Represents primitve "static" information of iOS devices.
    /// </summary>
    class PrimativeInformation
    {
        
        AppleDevices apple_device_mods = new AppleDevices();

        
        private struct NORMAL_MODE_IDEVICE_INFO
        {
            public string ecid;          
        }



        /// <summary>
        /// Returns device ECID as string
        /// </summary>
        /// <param name="device_handle">handle of the iOS device</param>
        /// <returns></returns>
        public string return_device_ecid(iDeviceHandle device_handle)
        {
            var lockdown = LibiMobileDevice.Instance.Lockdown;
            LockdownClientHandle lockdowndevice;
            lockdown.lockdownd_client_new_with_handshake(device_handle, out lockdowndevice, "Ghost");
            NORMAL_MODE_IDEVICE_INFO normal_info = new NORMAL_MODE_IDEVICE_INFO();

            string return_ecid = null;

            try
            {
                lockdown.lockdownd_get_device_udid(lockdowndevice, out normal_info.ecid);                 
            }
            catch (Exception)
            {               
                return return_ecid;
            }

            if (string.IsNullOrWhiteSpace(normal_info.ecid))
            {
                return_ecid = normal_info.ecid.Split('-')[1];
            }

            return return_ecid;
        }


        /// <summary>
        /// Returns device model type as string
        /// </summary>
        /// <param name="device_handle">handle of the iOS device</param>
        /// <returns></returns>
        public string return_device_model(iDeviceHandle device_handle)
        {
            var lockdown = LibiMobileDevice.Instance.Lockdown;
            LockdownClientHandle lockdowndevice;
            lockdown.lockdownd_client_new_with_handshake(device_handle, out lockdowndevice, "Ghost");           

            string return_model = null;

            try
            {
                lockdown.lockdownd_get_value(lockdowndevice, null, "ProductType", out var model);
                LibiMobileDevice.Instance.Plist.plist_get_string_val(model, out return_model);
            }
            catch (Exception)
            {               
                return return_model;
            }

            if (string.IsNullOrWhiteSpace(return_model))
            {               
                return return_model;
            }
            
            else
            {
                foreach (KeyValuePair<string, string> iphone_make in apple_device_mods.devices_iphones)
                {
                    if (return_model == iphone_make.Key)
                    {

                        return_model = iphone_make.Value;
                        break;

                    }
                }
            }

            return return_model;
        }
    }
}
