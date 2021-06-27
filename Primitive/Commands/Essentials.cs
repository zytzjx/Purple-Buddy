using System.IO;


namespace PurpleBuddy.Primitive.Commands
{
    public class Essentials
    {
        /// <summary>
        /// Directory where iMobile binaries should be located.
        /// File names need to be appended!
        /// </summary>
        public string BinaryPath { get; } = Directory.GetCurrentDirectory() + "\\" + "win-x64";

        /// <summary>
        /// idevicediagnostics true file name.
        /// </summary>
        public string DeviceDiagnostics { get; } = "idevicediagnostics.exe";

        /// <summary>
        /// ideviceactivation true file name.
        /// </summary>
        public string DeviceActivation { get; } = "ideviceactivation.exe";
    }
}
