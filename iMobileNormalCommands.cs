using System.Diagnostics;
using System.Windows.Forms;
using iMobileDevice;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using iMobileDevice.Recovery;
using PurpleBuddy;
using PurpleBuddy.Primitive.Commands;

namespace Prizrak
{
    /// <summary>
    /// Overall general commands accross iOS devices.
    /// </summary>
    public class iMobileNormalCommands
    {      
        
        Essentials Helper = new Essentials();
             

        /// <summary>
        /// Boots all iOS devices based on user option.
        /// </summary>
        /// <param name="udid">the device UDID as string.</param>
        /// <param name="bootoption">the boot option; 1 = shutdown devices, 2 = reboot / restart devices.</param>
        public void bootDeviceOption(string udid, int bootoption)
        {
            
            switch (bootoption)
            {
                case 1:
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName        = $"{Helper.BinaryPath}" + "\\" + $"{Helper.DeviceDiagnostics}";
                        process.StartInfo.Arguments       = "-u " + udid + " shutdown";
                        process.StartInfo.CreateNoWindow  = true;
                        process.StartInfo.UseShellExecute = false;
                        process.Start();                  
                    }
                    break;

                case 2:
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName        = $"{Helper.BinaryPath}" + "\\" + $"{Helper.DeviceDiagnostics}";
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
        /// <param name="activationoption">Activation options; 1 = activate devices, 2 = deactivate devices.</param>
        public void setDeviceActivationState(in string udid, int activationoption)
        {

            int processExitCode = 0;
            bool activiationMethodIsSuccess;


            switch (activationoption)
            {
                case 1:
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName        = $"{Helper.BinaryPath}" + "\\" + $"{Helper.DeviceActivation}";
                        process.StartInfo.Arguments       = "-u " + udid + " activate";
                        process.StartInfo.CreateNoWindow  = true;
                        process.StartInfo.UseShellExecute = false;                   
                        process.Start();
                        process.WaitForExit();

                        processExitCode = process.ExitCode;
                        activiationMethodIsSuccess = (processExitCode == (int)BinaryReturnCodes.DeviceActivation.Success) ? true : false;

                        if (activiationMethodIsSuccess)
                        {
                            MessageBox.Show("Success");
                        }
                        else
                        {
                            MessageBox.Show(processExitCode.ToString());
                        }
                    }
                    break;

                case 2:
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName        = $"{Helper.BinaryPath}" + "\\" + $"{Helper.DeviceActivation}";
                        process.StartInfo.Arguments       = "-u " + udid + " deactivate";
                        process.StartInfo.CreateNoWindow  = true;
                        process.StartInfo.UseShellExecute = false;
                        process.Start();
                        process.WaitForExit();

                        processExitCode = process.ExitCode;
                        activiationMethodIsSuccess = (processExitCode == (int)BinaryReturnCodes.DeviceActivation.Success) ? true : false;

                        if (activiationMethodIsSuccess)
                        {
                            MessageBox.Show("Success");
                        }
                        else
                        {
                            MessageBox.Show(processExitCode.ToString());
                        }
                    }
                    break;

                default:
                    return;
            }

            if (activiationMethodIsSuccess)
            {
                MessageBox.Show("Success");
            }
        }


        /// <summary>
        /// Put all iOS devices in recovery mode.
        /// </summary>
        /// <param name="idevice">The device handle.</param>
        public void enterRecoveryMode(iDeviceHandle idevice)
        {
            var lockdown = LibiMobileDevice.Instance.Lockdown;
            LockdownClientHandle lockdowndevice;
            
            lockdown.lockdownd_client_new_with_handshake(idevice, out lockdowndevice, "Ghost");            
            lockdown.lockdownd_enter_recovery(lockdowndevice);           
        }


        /// <summary>
        /// Exit recovery loop.
        /// </summary>       
        public void exitRecoveryMode()
        {
            var recoverydevice = LibiMobileDevice.Instance.Recovery;
            ulong ecid = 0;

            RecoveryClientHandle recoveryClientHandle = null;
            RecoveryDeviceHandle recoveryDeviceHandle = null;
                       
            recoverydevice.irecv_devices_get_all();
            recoverydevice.irecv_open_with_ecid(out recoveryClientHandle, ecid);
            recoverydevice.irecv_devices_get_device_by_client(recoveryClientHandle, out recoveryDeviceHandle);
            recoverydevice.irecv_setenv(recoveryClientHandle, "auto-boot", "true");
            recoverydevice.irecv_saveenv(recoveryClientHandle);
            recoverydevice.irecv_reboot(recoveryClientHandle);
        }
    }
}
