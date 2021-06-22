using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using iMobileDevice;
using iMobileDevice.DiagnosticsRelay;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using iMobileDevice.Recovery;
using PurpleBuddy;

namespace Prizrak
{
    /// <summary>
    /// Overall general commands accross iOS devices.
    /// </summary>
    class iMobileNormalCommands
    {
      
        /// <summary>
        /// Directory where iMobile binaries should be located.
        /// </summary>
        private string imobiledevice_tools_path = Directory.GetCurrentDirectory() + "\\" + "win-x64";

        /// <summary>
        /// idevicediagnostics true file name.
        /// </summary>
        private const string idevicediagnostics = "idevicediagnostics.exe";

        /// <summary>
        /// ideviceactivation true file name.
        /// </summary>
        private const string ideviceactivation  = "ideviceactivation.exe";

        
        iMobileDeviceReturnCodes operation_return_codes = new iMobileDeviceReturnCodes();
        

        /// <summary>
        /// Gets if the activiation opperation (activate or deactivate) was successful or not.
        /// </summary>
        private bool ActiviationMethodIsSuccess { get; set; }

        /// <summary>
        /// Boots all iOS devices based on user option.
        /// </summary>
        /// <param name="udid">the device UDID as string.</param>
        /// <param name="bootoption">the boot option; 1 = shutdown devices, 2 = reboot / restart devices.</param>
        public void mass_boot_iosdevice(string udid, int bootoption)
        {
            
            switch(bootoption)
            {
                case 1:
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName        = imobiledevice_tools_path + "\\" + $"[{idevicediagnostics}]";
                        process.StartInfo.Arguments       = "-u " + udid + " shutdown";
                        process.StartInfo.CreateNoWindow  = true;
                        process.StartInfo.UseShellExecute = false;
                        process.Start();                  
                    }
                    break;

                case 2:
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName        = imobiledevice_tools_path + "\\" + $"[{idevicediagnostics}]";
                        process.StartInfo.Arguments       = "-u " + udid + " restart";
                        process.StartInfo.CreateNoWindow  = true;
                        process.StartInfo.UseShellExecute = false;
                        process.Start();
                    }
                    break;

                default:
                    return;
            }            
        }


        /// <summary>
        /// Activates / Deactivates all iOS devices based on user option.
        /// </summary>
        /// <param name="udid">the device UDID as string.</param>
        /// <param name="activation_option">Activation options; 1 = activate devices, 2 = deactivate devices.</param>
        public void mass_activation_set_device_activation_state(string udid, int activation_option)
        {

            int r_code = 0;

            switch (activation_option)
            {
                case 1:
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName        = imobiledevice_tools_path + "\\" + $"[{ideviceactivation}]";
                        process.StartInfo.Arguments       = "-u " + udid + " activate";
                        process.StartInfo.CreateNoWindow  = true;
                        process.StartInfo.UseShellExecute = false;                   
                        process.Start();
                        process.WaitForExit();

                        r_code = process.ExitCode;
                        ActiviationMethodIsSuccess = (r_code == (int)operation_return_codes.ACTIVATION_IDEVICE_ACTIVATION_E_SUCCESS) ? true : false;
                    }
                    break;

                case 2:
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = imobiledevice_tools_path + "\\" + $"[{ideviceactivation}]";
                        process.StartInfo.Arguments = "-u " + udid + " deactivate";
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.UseShellExecute = false;
                        process.Start();

                    }
                    break;

                default:
                    return;
            }                      
        }


        /// <summary>
        /// Put all iOS devices in recovery mode.
        /// </summary>
        /// <param name="idevice">The device handle.</param>
        public void mass_recovery_enterrecoverymode(iDeviceHandle idevice)
        {
            var lockdown = LibiMobileDevice.Instance.Lockdown;
            LockdownClientHandle lockdowndevice;
            
            lockdown.lockdownd_client_new_with_handshake(idevice, out lockdowndevice, "Ghost");            
            lockdown.lockdownd_enter_recovery(lockdowndevice);           
        }


        /// <summary>
        /// Exit recovery loop.
        /// </summary>
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
