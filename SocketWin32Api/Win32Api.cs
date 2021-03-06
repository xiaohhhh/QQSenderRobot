﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SocketWin32Api
{
    public class Win32Api
    {
        private static Win32Api instance;

        public static Win32Api getInstance()
        {
            if (instance == null)
            {
                instance = new Win32Api();
            }
            return instance;
        }

        public struct LUID
        {
            uint LowPart;
            int HighPart;
        };

        public enum ACCESS_MASK : uint
        {
            DELETE = 0x00010000,
            READ_CONTROL = 0x00020000,
            WRITE_DAC = 0x00040000,
            WRITE_OWNER = 0x00080000,
            SYNCHRONIZE = 0x00100000,

            STANDARD_RIGHTS_REQUIRED = 0x000f0000,

            STANDARD_RIGHTS_READ = 0x00020000,
            STANDARD_RIGHTS_WRITE = 0x00020000,
            STANDARD_RIGHTS_EXECUTE = 0x00020000,

            STANDARD_RIGHTS_ALL = 0x001f0000,

            SPECIFIC_RIGHTS_ALL = 0x0000ffff,

            ACCESS_SYSTEM_SECURITY = 0x01000000,

            MAXIMUM_ALLOWED = 0x02000000,

            GENERIC_READ = 0x80000000,
            GENERIC_WRITE = 0x40000000,
            GENERIC_EXECUTE = 0x20000000,
            GENERIC_ALL = 0x10000000,

            DESKTOP_READOBJECTS = 0x00000001,
            DESKTOP_CREATEWINDOW = 0x00000002,
            DESKTOP_CREATEMENU = 0x00000004,
            DESKTOP_HOOKCONTROL = 0x00000008,
            DESKTOP_JOURNALRECORD = 0x00000010,
            DESKTOP_JOURNALPLAYBACK = 0x00000020,
            DESKTOP_ENUMERATE = 0x00000040,
            DESKTOP_WRITEOBJECTS = 0x00000080,
            DESKTOP_SWITCHDESKTOP = 0x00000100,

            WINSTA_ENUMDESKTOPS = 0x00000001,
            WINSTA_READATTRIBUTES = 0x00000002,
            WINSTA_ACCESSCLIPBOARD = 0x00000004,
            WINSTA_CREATEDESKTOP = 0x00000008,
            WINSTA_WRITEATTRIBUTES = 0x00000010,
            WINSTA_ACCESSGLOBALATOMS = 0x00000020,
            WINSTA_EXITWINDOWS = 0x00000040,
            WINSTA_ENUMERATE = 0x00000100,
            WINSTA_READSCREEN = 0x00000200,

            WINSTA_ALL_ACCESS = 0x0000037f
        }

        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_LOGON_NETWORK = 3;
        private const int LOGON32_LOGON_BATCH = 4;
        private const int LOGON32_LOGON_SERVICE = 5;
        private const int LOGON32_LOGON_UNLOCK = 7;
        private const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;
        private const int LOGON32_LOGON_NEW_CREDENTIALS = 9;

        public const uint ES_SYSTEM_REQUIRED = 0x00000001;
        public const uint ES_DISPLAY_REQUIRED = 0x00000002;
        public const uint ES_CONTINUOUS = 0x80000000;
        // 声明INI文件的写操作函数 WritePrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数 GetPrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

        // 声明INI文件的读操作函数 GetPrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        public static extern void SetThreadExecutionState(uint esFlags);
        /// <summary>
        /// 模拟windows登录域
        /// </summary>
        [DllImport("advapi32.DLL", SetLastError = true)]
        public static extern int LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        /// <summary>
        /// 函数查看系统权限的特权值，返回信息到一个LUID结构体里
        /// </summary>
        [DllImport("advapi32.DLL", SetLastError = true)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, ref LUID pLuid);
        /// <summary>
        /// Opens the specified desktop object.
        /// </summary>
        [DllImport("user32.DLL", SetLastError = true)]
        public static extern IntPtr OpenDesktop(string lpszDesktop, uint dwFlags, bool fInherit, ACCESS_MASK dwDesiredAccess);
        /// <summary>
        /// Opens the specified desktop object.
        /// </summary>
        [DllImport("user32.DLL", SetLastError = true)]
        public static extern bool CloseDesktop(IntPtr hDesktop);
        /// <summary>
        /// Makes the specified desktop visible and activates it. This enables the desktop to receive input from the user. 
        /// The calling process must have DESKTOP_SWITCHDESKTOP access to the desktop for the SwitchDesktop function to succeed.
        /// </summary>
        [DllImport("user32.DLL", SetLastError = true)]
        public static extern bool SwitchDesktop(IntPtr hDesktop);
        /// <summary>
        /// GetLastError
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("user32.DLL", SetLastError = true)]
        public static extern IntPtr OpenWindowStation(string lpszWinSta, bool fInherit, ACCESS_MASK dwDesiredAccess);

        [DllImport("user32.DLL", SetLastError = true)]
        public static extern bool SetThreadDesktop(IntPtr hDesktop);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetDesktopWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetProcessWindowStation();
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetCurrentThreadId();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetThreadDesktop(IntPtr dwThread);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr OpenWindowStation(string a, bool b, int c);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr OpenDesktop(string lpszDesktop, uint dwFlags,
        bool fInherit, uint dwDesiredAccess);
        [DllImport("rpcrt4.dll", SetLastError = true)]
        public static extern IntPtr RpcImpersonateClient(int i);
        [DllImport("rpcrt4.dll", SetLastError = true)]
        public static extern IntPtr RpcRevertToSelf();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetProcessWindowStation(IntPtr a);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CloseWindowStation(IntPtr a);

        public delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "IsWindow")]
        public static extern bool IsWindow(IntPtr hWnd);

        private string sPath = null;
        public Win32Api setPath(string path)
        {
            sPath = path;
            return this;
        }

        public void WriteValue(string section, string key, string value)
        {
            // section=配置节，key=键名，value=键值，path=路径
            WritePrivateProfileString(section, key, value, sPath);
        }

        public string ReadValue(string section, string key)
        {
            // 每次从ini中读取多少字节
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);
            // section=配置节，key=键名，temp=上面，path=路径
            GetPrivateProfileString(section, key, "", temp, 255, sPath);
            return temp.ToString();
        }

        public string ReadValue(string section, string key, string def)
        {
            // 每次从ini中读取多少字节
            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);
            // section=配置节，key=键名，temp=上面，path=路径
            GetPrivateProfileString(section, key, def, temp, 255, sPath);
            return temp.ToString();
        }

        public static void SystemUnsleepLock()
        {
            SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED);
        }
        public static void SystemUnsleepLockRelase()
        {
            SetThreadExecutionState(ES_CONTINUOUS);
        }

        IntPtr admin_token = default(IntPtr);

        public IntPtr Login(string name, string pwd)
        {
            LogonUser(name, ".", pwd, LOGON32_LOGON_NEW_CREDENTIALS, 0, ref admin_token);
            return admin_token;
        }

        public bool isLockedWindow()
        {
            bool rel = false;
            IntPtr hDesk = OpenDesktop("Default", 0, false, ACCESS_MASK.DESKTOP_SWITCHDESKTOP);
            if (hDesk != IntPtr.Zero)
            {
                rel = !SwitchDesktop(hDesk);
                // cleanup   
                CloseDesktop(hDesk);
            }
            return rel;
        }

        public void initWindowsDesktop()
        {
            IntPtr token = Login("guoyao", "guoyao19");
            if (token == IntPtr.Zero)
            {
                throw new Exception("Login faild, errCode = " + GetLastError());
            }

            IntPtr hDesk = OpenDesktop("Default", 0, false,
                ACCESS_MASK.DESKTOP_CREATEMENU |
                ACCESS_MASK.DESKTOP_CREATEWINDOW |
                ACCESS_MASK.DESKTOP_HOOKCONTROL |
                ACCESS_MASK.DESKTOP_ENUMERATE |
                ACCESS_MASK.DESKTOP_JOURNALPLAYBACK |
                ACCESS_MASK.DESKTOP_JOURNALRECORD |
                ACCESS_MASK.DESKTOP_READOBJECTS |
                ACCESS_MASK.DESKTOP_SWITCHDESKTOP |
                ACCESS_MASK.DESKTOP_WRITEOBJECTS);
            if (hDesk == IntPtr.Zero)
            {
                throw new Exception("OpenDesktop faild, errCode = " + GetLastError());
            }
            bool result = SetThreadDesktop(hDesk);
            if (!result)
            {
                throw new Exception("SetThreadDesktop faild, errCode = " + GetLastError());
            }

        }

        public IntPtr FindWindowServer(string name)
        {
            GetDesktopWindow();
            IntPtr hwinstaSave = GetProcessWindowStation();
            IntPtr dwThreadId = GetCurrentThreadId();
            IntPtr hdeskSave = GetThreadDesktop(dwThreadId);
            IntPtr hwinstaUser = OpenWindowStation("WinSta0", false, 33554432);
            if (hwinstaUser == IntPtr.Zero)
            {
                RpcRevertToSelf();
                return IntPtr.Zero;
            }
            SetProcessWindowStation(hwinstaUser);
            IntPtr hdeskUser = OpenDesktop("Default", 0, false, 33554432);
            RpcRevertToSelf();
            if (hdeskUser == IntPtr.Zero)
            {
                SetProcessWindowStation(hwinstaSave);
                CloseWindowStation(hwinstaUser);
                return IntPtr.Zero;
            }
            SetThreadDesktop(hdeskUser);
            IntPtr dwGuiThreadId = dwThreadId;

            //SenderApi.StartQQFindHwnd("四人帮");
            IntPtr mWnd = IntPtr.Zero;//= SenderApi.GetFindHwnd();


            dwGuiThreadId = IntPtr.Zero;
            SetThreadDesktop(hdeskSave);
            SetProcessWindowStation(hwinstaSave);
            CloseDesktop(hdeskUser);
            CloseWindowStation(hwinstaUser);

            return mWnd;

        }
        /// <summary>
        /// 运行cmd命令
        /// 会显示命令窗口
        /// </summary>
        /// <param name="cmdExe">指定应用程序的完整路径</param>
        /// <param name="cmdStr">执行命令行参数</param>
        public bool RunCmd(string cmdExe, string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    //指定启动进程是调用的应用程序和命令行参数
                    ProcessStartInfo psi = new ProcessStartInfo(cmdExe, cmdStr);
                    myPro.StartInfo = psi;
                    myPro.Start();
                    myPro.WaitForExit();
                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }
        /// <summary>
        /// 运行cmd命令
        /// 不显示命令窗口
        /// </summary>
        /// <param name="cmdExe">指定应用程序的完整路径</param>
        /// <param name="cmdStr">执行命令行参数</param>
        public bool RunCmd2(string cmdExe, string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    myPro.StartInfo.FileName = "cmd.exe";
                    myPro.StartInfo.UseShellExecute = false;
                    myPro.StartInfo.RedirectStandardInput = true;
                    myPro.StartInfo.RedirectStandardOutput = true;
                    myPro.StartInfo.RedirectStandardError = true;
                    myPro.StartInfo.CreateNoWindow = true;
                    myPro.Start();
                    //如果调用程序路径中有空格时，cmd命令执行失败，可以用双引号括起来 ，在这里两个引号表示一个引号（转义）
                    string str = string.Format(@"""{0}"" {1} {2}", cmdExe, cmdStr, "&exit");

                    myPro.StandardInput.WriteLine(str);
                    myPro.StandardInput.AutoFlush = true;
                    myPro.WaitForExit();

                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }
    }
}
