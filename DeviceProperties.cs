using iMobileDevice.iDevice;
using PurpleBuddy.DevicePropertiesForm;
using PurpleBuddy.Mobile;
using PurpleBuddy.Primitive.Information;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurpleBuddy
{
    public partial class DeviceProperties : Form
    {
        public DeviceProperties()
        {
            InitializeComponent();
        }


        PrimativeInformation primativeInformation = new PrimativeInformation();
        MobileGestalt gestaltKeyValues = new MobileGestalt();
        ConsoleOutput consoleOutput = new ConsoleOutput();
        Essentials essentials = new Essentials();
        DefaultDeviceInformationKeys defaultDeviceInformationKeys = new DefaultDeviceInformationKeys();

        public iDeviceHandle DeviceHandle { get; set; } = null;
        public string DeviceUDID { get; set; } = null;



        private void updateBatteryPercentage()
        {
            while (true)
            {
                change_via_thread.ControlInvoke(lbl_batterypercent, () => lbl_batterypercent.Text = primativeInformation.getBatteryPercentage(DeviceHandle));
               //Thread.CurrentThread.Sleep(essentials.SleepBetweenBatteryValueCapture);              
            }
        }


        private void updateDeviceCPUType()
        {           
            string cpu = primativeInformation.getDefaultInformationAsString(DeviceHandle, defaultDeviceInformationKeys.CPUArchitecture);
            change_via_thread.ControlInvoke(lbl_devicecpu, () => lbl_devicecpu.Text = cpu);
        }

        private void updateDeviceActivationState()
        {
            string state = primativeInformation.getDefaultInformationAsString(DeviceHandle, defaultDeviceInformationKeys.ActivationState);
            change_via_thread.ControlInvoke(lbl_deviceactivationstate, () => lbl_deviceactivationstate.Text = state);
        }

        private void updateDeviceFirmwareVersion()
        {
            string firmware = primativeInformation.getDefaultInformationAsString(DeviceHandle, defaultDeviceInformationKeys.FirmwareVersion);
            change_via_thread.ControlInvoke(lbl_devicefirmwareversion, () => lbl_devicefirmwareversion.Text = firmware);
        }

        private void updateDeviceImei()
        {
            string imei = primativeInformation.getDefaultInformationAsString(DeviceHandle, defaultDeviceInformationKeys.InternationalMobileEquipmentIdentity);
            change_via_thread.ControlInvoke(lbl_deviceimei, () => lbl_deviceimei.Text = imei);
        }

        private void updateDeviceImei2()
        {
            string imei2 = primativeInformation.getDefaultInformationAsString(DeviceHandle, defaultDeviceInformationKeys.InternationalMobileEquipmentIdentity2);
            change_via_thread.ControlInvoke(lbl_deviceimei2, () => lbl_deviceimei2.Text = imei2);
        }

        private void updateDeviceImsi()
        {
            string imsi = primativeInformation.getDefaultInformationAsString(DeviceHandle, defaultDeviceInformationKeys.InternationalMobileSubscriberIdentity);
            change_via_thread.ControlInvoke(lbl_deviceimsi, () => lbl_deviceimsi.Text = imsi);
        }

        private void updateDeviceMeid()
        {
            string meid = primativeInformation.getDefaultInformationAsString(DeviceHandle, defaultDeviceInformationKeys.MobileEquipmentIdentifier);
            change_via_thread.ControlInvoke(lbl_devicemeid, () => lbl_devicemeid.Text = meid);
        }

        private void updateDeviceType()
        {
            string type = primativeInformation.getDefaultInformationAsString(DeviceHandle, defaultDeviceInformationKeys.DeviceClass);
            change_via_thread.ControlInvoke(lbl_devicetype, () => lbl_devicetype.Text = type);
        }

        private void updateDeviceModelNumber()
        {
            string modelNumber = primativeInformation.getDefaultInformationAsString(DeviceHandle, defaultDeviceInformationKeys.ModelNumber);
            change_via_thread.ControlInvoke(lbl_devicemodelnumber, () => lbl_devicemodelnumber.Text = modelNumber);
        }

        private void updateDeviceSoftwareVersion()
        {
            string softwareVersion = primativeInformation.getDefaultInformationAsString(DeviceHandle, defaultDeviceInformationKeys.ProductVersion);
            change_via_thread.ControlInvoke(lbl_devicesoftwareversion, () => lbl_devicesoftwareversion.Text = softwareVersion);
        }

        private void Properties_Load(object sender, EventArgs e)
        {
            //Task liveCaputreBatteryPercent = new Task(updateBatteryPercentage);
            //liveCaputreBatteryPercent.Start();


            Task[] fillGeneralInformationIndividuals = new Task[10];

            fillGeneralInformationIndividuals[0] = Task.Run(() => { updateDeviceCPUType(); });
            fillGeneralInformationIndividuals[1] = Task.Run(() => { updateDeviceActivationState(); });
            fillGeneralInformationIndividuals[2] = Task.Run(() => { updateDeviceFirmwareVersion(); });
            fillGeneralInformationIndividuals[3] = Task.Run(() => { updateDeviceImei(); });
            fillGeneralInformationIndividuals[4] = Task.Run(() => { updateDeviceImei2(); });
            fillGeneralInformationIndividuals[5] = Task.Run(() => { updateDeviceImsi(); });
            fillGeneralInformationIndividuals[6] = Task.Run(() => { updateDeviceMeid(); });
            fillGeneralInformationIndividuals[7] = Task.Run(() => { updateDeviceType(); });
            fillGeneralInformationIndividuals[8] = Task.Run(() => { updateDeviceModelNumber(); });
            fillGeneralInformationIndividuals[9] = Task.Run(() => { updateDeviceSoftwareVersion(); });


        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {
            


            
        }
    }

    class change_via_thread
    {
        delegate void UniversalVoidDelegate();

        public static void ControlInvoke(Control control, Action function)
        {
            if (control.IsDisposed || control.Disposing)
                return;

            if (control.InvokeRequired)
            {

                control.Invoke(new UniversalVoidDelegate(() => ControlInvoke(control, function)));
                return;

            }
            function();
        }
    }
}
