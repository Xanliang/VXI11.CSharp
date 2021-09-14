using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VXI11
{
    /// <summary>
/// A collection of constants used by the "vxi11" ONC/RPC program.
/// </summary>
    public class vxi11
    {
        public static int DEVICE_CORE = 0x0607AF;
        public static int device_trigger_1 = 14;
        public static int DEVICE_ASYNC_VERSION = 1;
        public static int device_read_1 = 12;
        public static int DEVICE_CORE_VERSION = 1;
        public static int destroy_intr_chan_1 = 26;
        public static int device_lock_1 = 18;
        public static int device_remote_1 = 16;
        public static int destroy_link_1 = 23;
        public static int device_docmd_1 = 22;
        public static int device_unlock_1 = 19;
        public static int device_readstb_1 = 13;
        public static int device_local_1 = 17;
        public static int device_enable_srq_1 = 20;
        public static int device_abort_1 = 1;
        public static int device_clear_1 = 15;
        public static int create_intr_chan_1 = 25;
        public static int device_intr_srq_1 = 30;
        public static int DEVICE_INTR = 0x0607B1;
        public static int DEVICE_INTR_VERSION = 1;
        public static int create_link_1 = 10;
        public static int device_write_1 = 11;
        public static int DEVICE_ASYNC = 0x0607B0;
    }
}
