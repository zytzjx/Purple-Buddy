using iMobileDevice;
using iMobileDevice.DiagnosticsRelay;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using iMobileDevice.Recovery;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prizrak
{
    public partial class main_form : Form
    {
        public main_form()
        {
            InitializeComponent();
            NativeLibraries.Load();
        }


        iosbasicessentials basic_ios_essentials = new iosbasicessentials();
        AppleDevices apple_device_mods = new AppleDevices();


        private struct NORMAL_MODE_IDEVICE_INFO     //      Only usage: update_listview_normal_mode_idevices(arg)
        {
            public string model;
            public ulong  capacity;
            public string udid;
            public string imei;
            public string iccid;
            
            public ListViewItem item;
            public string[] data;
        }


        private void update_listview_normal_mode_idevices(iDeviceHandle device_handle)
        {
            NORMAL_MODE_IDEVICE_INFO normal_info = new NORMAL_MODE_IDEVICE_INFO();
            normal_info.data = new string[5];


            var lockdown = LibiMobileDevice.Instance.Lockdown;
            LockdownClientHandle lockdowndevice;         
            lockdown.lockdownd_client_new_with_handshake(device_handle, out lockdowndevice, "Ghost").ThrowOnError();



            try
            {
                lockdown.lockdownd_get_value(lockdowndevice, null, "ProductType", out var model).ThrowOnError();
                LibiMobileDevice.Instance.Plist.plist_get_string_val(model, out normal_info.model);              
            }
            catch (Exception)
            {
                normal_info.model = "";               
            }


            if (string.IsNullOrWhiteSpace(normal_info.model))
            
                normal_info.data[0] = "N/A";

            else
            {
                foreach (KeyValuePair<string, string> iphone_make in apple_device_mods.devices_iphones)
                {
                    if (normal_info.model == iphone_make.Key)
                    {
                        normal_info.data[0] = iphone_make.Value;
                        break;
                    }
                }
            }          



            try
            {
                lockdown.lockdownd_get_value(lockdowndevice, "com.apple.disk_usage.factory", "TotalDiskCapacity", out var storage);
                LibiMobileDevice.Instance.Plist.plist_get_uint_val(storage, ref normal_info.capacity);                               
            }
            catch (Exception)
            {
                normal_info.capacity = 0;
            }


            if (normal_info.capacity == 0)
            {              
                normal_info.data[1] = "N/A"; 
            }

            else

                normal_info.data[1] = normal_info.capacity.ToString().Split('0')[0];



            try
            {
                lockdown.lockdownd_get_device_udid(lockdowndevice, out normal_info.udid);                
            }
            catch (Exception)
            {
                normal_info.udid = "";               
            }
            

            if (string.IsNullOrWhiteSpace(normal_info.udid))

                normal_info.data[2] = "N/A";

            else

                normal_info.data[2] = normal_info.udid;     //.Split('-')[1]  =  ECID



            try
            {
                lockdown.lockdownd_get_value(lockdowndevice, null, "InternationalMobileEquipmentIdentity", out var d_imei).ThrowOnError();
                LibiMobileDevice.Instance.Plist.plist_get_string_val(d_imei, out normal_info.imei);               
            }
            catch (Exception)
            {
                normal_info.imei = "";               
            }

            if (string.IsNullOrWhiteSpace(normal_info.imei))

                normal_info.data[3] = "N/A";

            else

                normal_info.data[3] = normal_info.imei;




            try
            {
                lockdown.lockdownd_get_value(lockdowndevice, null, "IntegratedCircuitCardIdentity", out var iccid).ThrowOnError();
                LibiMobileDevice.Instance.Plist.plist_get_string_val(iccid, out normal_info.iccid);
                
            }
            catch (Exception)
            {
                normal_info.iccid = "";                                   
            }

            if (string.IsNullOrWhiteSpace(normal_info.iccid))

                normal_info.data[4] = "N/A";

            else

                normal_info.data[4] = normal_info.iccid;


            normal_info.item = new ListViewItem(normal_info.data);

            try
            {
                change_via_thread.ControlInvoke(connected_normal_mode_devices_listview, () => connected_normal_mode_devices_listview.BeginUpdate());
                change_via_thread.ControlInvoke(connected_normal_mode_devices_listview, () => connected_normal_mode_devices_listview.Items.Add(normal_info.item));
                change_via_thread.ControlInvoke(connected_normal_mode_devices_listview, () => connected_normal_mode_devices_listview.EndUpdate());
            }
            catch (Exception)
            {
                return;
            }
        }






        private struct DEVICE_COUNT
        {

            public ReadOnlyCollection<string> udids;
            public int count;
            
        }

        private void get_device_count()
        {

            while (true)
            {
                DEVICE_COUNT structure = new DEVICE_COUNT();

                structure.count = 0;

                var idevice  = LibiMobileDevice.Instance.iDevice;
                var lockdown = LibiMobileDevice.Instance.Lockdown;
                var ret      = idevice.idevice_get_device_list(out structure.udids, ref structure.count);
                

                change_via_thread.ControlInvoke(connected_normal_mode_devices_listview, () => toolstrip_connected_devices_lable.Text = "NMM: "+ structure.count.ToString());
                Thread.Sleep(260);
            }         
        }


        
        




        private struct MAIN_FORM
        {
            public Task[] form_load_tasks;
        }

        private void main_form_Load(object sender, EventArgs e)
        {
            MAIN_FORM essentials = new MAIN_FORM();
            
            connected_normal_mode_devices_listview.View = View.Details;
            connected_normal_mode_devices_listview.ListViewItemSorter = null;
            connected_normal_mode_devices_listview.Columns.Add("MODEL", 125);
            connected_normal_mode_devices_listview.Columns.Add("CAPACITY", 70);
            connected_normal_mode_devices_listview.Columns.Add("UDID", 180);
            connected_normal_mode_devices_listview.Columns.Add("IMEI", 150);
            connected_normal_mode_devices_listview.Columns.Add("ICCID", 70);
                                    

            essentials.form_load_tasks    = new Task[1];
            essentials.form_load_tasks[0] = Task.Run(() => { get_device_count(); });
        }

        




        

        private void main_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (Process process = Process.GetCurrentProcess())
            {
                process.Kill();
            }
        }
                            
        private void toolstrip_shutdown_devices_button_Click_1(object sender, EventArgs e)
        {
            ReadOnlyCollection<string> udids;
            int count = 0;


            var idevice = LibiMobileDevice.Instance.iDevice;
            var result  = idevice.idevice_get_device_list(out udids, ref count);


            foreach (var device_id in udids)
            {
                iDeviceHandle device_handle = null;
                idevice.idevice_new(out device_handle, device_id);

                Thread t_shutdown_iosdevices = new Thread(() => basic_ios_essentials.mass_boot_iosdevice(device_id, 1));
                t_shutdown_iosdevices.Start();
            }
        }

        private void toolstrip_reboot_devices_button_Click(object sender, EventArgs e)
        {
            ReadOnlyCollection<string> udids;
            int count = 0;


            var idevice = LibiMobileDevice.Instance.iDevice;
            var result  = idevice.idevice_get_device_list(out udids, ref count);


            foreach (var device_id in udids)
            {
                iDeviceHandle device_handle = null;
                idevice.idevice_new(out device_handle, device_id);

                Thread t_shutdown_iosdevices = new Thread(() => basic_ios_essentials.mass_boot_iosdevice(device_id, 2));
                t_shutdown_iosdevices.Start();
            }
        }

        private void toolstrip_enter_recovery_devices_button_Click_1(object sender, EventArgs e)
        {
            ReadOnlyCollection<string> udids;
            int count = 0;


            var idevice = LibiMobileDevice.Instance.iDevice;
            var result  = idevice.idevice_get_device_list(out udids, ref count);


            foreach (var device_id in udids)
            {
                iDeviceHandle device_handle = null;
                idevice.idevice_new(out device_handle, device_id);

                Thread t_enter_recovery_iosdevices = new Thread(() => basic_ios_essentials.recovery_enterrecoverymode(device_handle));
                t_enter_recovery_iosdevices.Start();
            }
        }

        private void toolstrip_exit_recovery_devices_button_Click_1(object sender, EventArgs e)
        {
            Thread t_exit_recovery_iosdevices = new Thread(() => basic_ios_essentials.recovery_exitrecoverymode());
            t_exit_recovery_iosdevices.Start();
        }

        private void toolstrip_connected_devices_lable_TextChanged_1(object sender, EventArgs e)
        {
            connected_normal_mode_devices_listview.Items.Clear();

            ReadOnlyCollection<string> udids;
            int count = 0;


            var idevice = LibiMobileDevice.Instance.iDevice;
            var result  = idevice.idevice_get_device_list(out udids, ref count);


            foreach (var device_id in udids)
            {
                iDeviceHandle device_handle = null;
                idevice.idevice_new(out device_handle, device_id);

                Thread t_update_normal_info = new Thread(() => update_listview_normal_mode_idevices(device_handle));
                t_update_normal_info.Start();
            }
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
