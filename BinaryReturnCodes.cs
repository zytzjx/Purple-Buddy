using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurpleBuddy
{
    public class BinaryReturnCodes
    {
        /// <summary>
        /// https://github.com/libimobiledevice/libimobiledevice/blob/master/include/libimobiledevice/diagnostics_relay.h
        /// </summary>
        public enum DiagnosticsRelay
        {
            Success = 0,
            ArgumentInvalid = -1,
            PlistError = -2,
            MuxError = -3,
            UnknownError = -256
        }

        /// <summary>
        /// https://github.com/libimobiledevice-win32/libideviceactivation/blob/msvc/master/include/libideviceactivation.h
        /// </summary>
        public enum DeviceActivation
        {
            Success = 0,
            IncompleteInfo = -1,
        }
    }
}
