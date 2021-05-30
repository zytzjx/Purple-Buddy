using System.Diagnostics;
using System.IO;
using iMobileDevice;
using iMobileDevice.DiagnosticsRelay;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using iMobileDevice.Recovery;

namespace Prizrak
{
    class iosbasicessentials
    {

       

        private string imobiledevice_tools_path = Directory.GetCurrentDirectory() + "\\" + "win-x64";



        public void mass_boot_iosdevice(string udid, int bootoption)
        {
            


            if(bootoption == 1)
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName        = imobiledevice_tools_path + "\\" + "idevicediagnostics.exe";
                    process.StartInfo.Arguments       = "-u " + udid + " shutdown";
                    process.StartInfo.CreateNoWindow  = true;
                    process.StartInfo.UseShellExecute = false;
                    process.Start();                   
                }
            }

            if (bootoption == 2)
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName        = imobiledevice_tools_path + "\\" + "idevicediagnostics.exe";
                    process.StartInfo.Arguments       = "-u " + udid + " restart";
                    process.StartInfo.CreateNoWindow  = true;
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                }
            }
        }

        public void mass_activation_set_device_activation_state(string udid, int activation_option)
        {
            

            if (activation_option == 1)
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName        = imobiledevice_tools_path + "\\" + "ideviceactivation.exe";
                    process.StartInfo.Arguments       = "-u " + udid + " activate";
                    process.StartInfo.CreateNoWindow  = true;
                    process.StartInfo.UseShellExecute = false;                   
                    process.Start();
                }
            }

            if (activation_option == 2)
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName        = imobiledevice_tools_path + "\\" + "ideviceactivation.exe";
                    process.StartInfo.Arguments       = "-u " + udid + " deactivate";
                    process.StartInfo.CreateNoWindow  = true;
                    process.StartInfo.UseShellExecute = false;
                    process.Start();
                   
                }
            }
        }


        public void recovery_enterrecoverymode(iDeviceHandle idevice)
        {
            var lockdown = LibiMobileDevice.Instance.Lockdown;
            LockdownClientHandle lockdowndevice;
            
            lockdown.lockdownd_client_new_with_handshake(idevice, out lockdowndevice, "Ghost").ThrowOnError();            
            lockdown.lockdownd_enter_recovery(lockdowndevice);           
        }


        public void recovery_exitrecoverymode()
        {
            var recoverydevice = LibiMobileDevice.Instance.Recovery;

            RecoveryClientHandle recoveryClientHandle = null;
            RecoveryDeviceHandle recoveryDeviceHandle = null;
            
            ulong ecid = 0;

            recoverydevice.irecv_devices_get_all();
            recoverydevice.irecv_open_with_ecid(out recoveryClientHandle, ecid);
            recoverydevice.irecv_devices_get_device_by_client(recoveryClientHandle, out recoveryDeviceHandle);
            recoverydevice.irecv_setenv(recoveryClientHandle, "auto-boot", "true");
            recoverydevice.irecv_saveenv(recoveryClientHandle);
            recoverydevice.irecv_reboot(recoveryClientHandle);
        }
    }
}
