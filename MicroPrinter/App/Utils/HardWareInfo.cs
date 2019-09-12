namespace MicroPrinter.App.Utils
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class HardWareInfo
    {
        private const uint CREATE_NEW = 1;
        private const uint DFP_GET_VERSION = 0x74080;
        private const uint DFP_RECEIVE_DRIVE_DATA = 0x7c088;
        private const uint DFP_SEND_DRIVE_COMMAND = 0x7c084;
        private const uint FILE_SHARE_READ = 1;
        private const uint FILE_SHARE_WRITE = 2;
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint OPEN_EXISTING = 3;

        private static void ChangeByteOrder(byte[] charArray)
        {
            for (int i = 0; i < charArray.Length; i += 2)
            {
                byte num2 = charArray[i];
                charArray[i] = charArray[i + 1];
                charArray[i + 1] = num2;
            }
        }

        [DllImport("kernel32.dll", SetLastError=true)]
        private static extern int CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll", SetLastError=true)]
        private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        [DllImport("kernel32.dll")]
        private static extern int DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, ref GetVersionOutParams lpOutBuffer, uint nOutBufferSize, ref uint lpBytesReturned, [Out] IntPtr lpOverlapped);
        [DllImport("kernel32.dll")]
        private static extern int DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, ref SendCmdInParams lpInBuffer, uint nInBufferSize, ref SendCmdOutParams lpOutBuffer, uint nOutBufferSize, ref uint lpBytesReturned, [Out] IntPtr lpOverlapped);
        private static unsafe string GetHardDiskSerialNumber()
        {
            byte num = 0;
            GetVersionOutParams structure = new GetVersionOutParams();
            SendCmdInParams params2 = new SendCmdInParams();
            SendCmdOutParams params3 = new SendCmdOutParams();
            uint lpBytesReturned = 0;
            IntPtr hDevice = CreateFile($"\\.\PhysicalDrive{num}", 0xc0000000, 3, IntPtr.Zero, 3, 0, IntPtr.Zero);
            if (hDevice == IntPtr.Zero)
            {
                throw new Exception("CreateFile faild.");
            }
            GetVersionOutParams* paramsPtr1 = &structure;
            if (DeviceIoControl(hDevice, 0x74080, IntPtr.Zero, 0, ref (GetVersionOutParams) ref paramsPtr1, (uint) Marshal.SizeOf<GetVersionOutParams>(structure), ref lpBytesReturned, IntPtr.Zero) == 0)
            {
                CloseHandle(hDevice);
                throw new Exception($"Drive {num + 1} may not exists.");
            }
            if ((structure.fCapabilities & 1) == 0)
            {
                CloseHandle(hDevice);
                throw new Exception("Error: IDE identify command not supported.");
            }
            params2.irDriveRegs.bDriveHeadReg = ((num & 1) == 0) ? 160 : 0xb0;
            if ((structure.fCapabilities & (0x10 >> (num & 0x1f))) != 0)
            {
                CloseHandle(hDevice);
                throw new Exception($"Drive {num + 1} is a ATAPI device, we don''t detect it.");
            }
            params2.irDriveRegs.bCommandReg = 0xec;
            params2.bDriveNumber = num;
            params2.irDriveRegs.bSectorCountReg = 1;
            params2.irDriveRegs.bSectorNumberReg = 1;
            params2.cBufferSize = 0x200;
            SendCmdInParams* paramsPtr2 = &params2;
            SendCmdOutParams* paramsPtr3 = &params3;
            if (DeviceIoControl(hDevice, 0x7c088, ref (SendCmdInParams) ref paramsPtr2, (uint) Marshal.SizeOf<SendCmdInParams>(params2), ref (SendCmdOutParams) ref paramsPtr3, (uint) Marshal.SizeOf<SendCmdOutParams>(params3), ref lpBytesReturned, IntPtr.Zero) == 0)
            {
                CloseHandle(hDevice);
                throw new Exception("DeviceIoControl failed: DFP_RECEIVE_DRIVE_DATA");
            }
            CloseHandle(hDevice);
            IdSector bBuffer = params3.bBuffer;
            ChangeByteOrder(bBuffer.sSerialNumber);
            return Encoding.ASCII.GetString(bBuffer.sSerialNumber).Trim();
        }

        public static string GetHardWareString()
        {
            string hardDiskSerialNumber = null;
            try
            {
                hardDiskSerialNumber = GetHardDiskSerialNumber();
            }
            catch
            {
            }
            return hardDiskSerialNumber;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct DriverStatus
        {
            public byte bDriverError;
            public byte bIDEStatus;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=2)]
            public byte[] bReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=2)]
            public uint[] dwReserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct GetVersionOutParams
        {
            public byte bVersion;
            public byte bRevision;
            public byte bReserved;
            public byte bIDEDeviceMap;
            public uint fCapabilities;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public uint[] dwReserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct IdeRegs
        {
            public byte bFeaturesReg;
            public byte bSectorCountReg;
            public byte bSectorNumberReg;
            public byte bCylLowReg;
            public byte bCylHighReg;
            public byte bDriveHeadReg;
            public byte bCommandReg;
            public byte bReserved;
        }

        [StructLayout(LayoutKind.Sequential, Size=0x200, Pack=1)]
        internal struct IdSector
        {
            public ushort wGenConfig;
            public ushort wNumCyls;
            public ushort wReserved;
            public ushort wNumHeads;
            public ushort wBytesPerTrack;
            public ushort wBytesPerSector;
            public ushort wSectorsPerTrack;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=3)]
            public ushort[] wVendorUnique;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=20)]
            public byte[] sSerialNumber;
            public ushort wBufferType;
            public ushort wBufferSize;
            public ushort wECCSize;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public byte[] sFirmwareRev;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=40)]
            public byte[] sModelNumber;
            public ushort wMoreVendorUnique;
            public ushort wDoubleWordIO;
            public ushort wCapabilities;
            public ushort wReserved1;
            public ushort wPIOTiming;
            public ushort wDMATiming;
            public ushort wBS;
            public ushort wNumCurrentCyls;
            public ushort wNumCurrentHeads;
            public ushort wNumCurrentSectorsPerTrack;
            public uint ulCurrentSectorCapacity;
            public ushort wMultSectorStuff;
            public uint ulTotalAddressableSectors;
            public ushort wSingleWordDMA;
            public ushort wMultiWordDMA;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=0x80)]
            public byte[] bReserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct SendCmdInParams
        {
            public uint cBufferSize;
            public HardWareInfo.IdeRegs irDriveRegs;
            public byte bDriveNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=3)]
            public byte[] bReserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=4)]
            public uint[] dwReserved;
            public byte bBuffer;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        internal struct SendCmdOutParams
        {
            public uint cBufferSize;
            public MicroPrinter.App.Utils.HardWareInfo.DriverStatus DriverStatus;
            public HardWareInfo.IdSector bBuffer;
        }
    }
}

