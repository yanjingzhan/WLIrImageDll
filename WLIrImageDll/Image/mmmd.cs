using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WLIrImageDll.Image
{
    public class mmmd
    {
        private const string mmmDllFile = "mmm.dll";

        #region mmm.dll

        /// <summary>
        /// 控制镜头方向调整视角
        /// </summary>
        /// <param name="iType">
        /// 1-向上
        /// 2-向下
        /// 3-向左
        /// 4-向右
        /// 5-放大
        /// 6-缩小
        /// 0-停止
        /// </param>
        [DllImport(mmmDllFile, EntryPoint = "ControlScaleLadder", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void ControlScaleLadder(int iType);

        /// <summary>
        /// 控制镜头调整焦距
        /// </summary>
        /// <param name="iType">
        /// 0-停止
        /// 1-近
        /// 2-远
        /// </param>

        [DllImport(mmmDllFile, EntryPoint = "AutoFocus", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void AutoFocus(int iType);

        /// <summary>
        /// 设置镜头校验方式
        /// </summary>
        /// <param name="bType">
        /// true-自动
        /// false-非自动
        /// </param>
        [DllImport(mmmDllFile, EntryPoint = "SetJiaoYanType", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetJiaoYanType(bool bType);

        /// <summary>
        /// 开始镜头校验
        /// </summary>
        [DllImport(mmmDllFile, EntryPoint = "BeginAutoJiaoYan", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void BeginAutoJiaoYan();

        /// <summary>
        /// 是否使用图像滤波
        /// </summary>
        /// <param name="bType"></param>
        [DllImport(mmmDllFile, EntryPoint = "SetImageFilter", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void SetImageFilter(bool bType);

        /// <summary>
        /// 传入灰度数据，得到温度数据
        /// </summary>
        /// <param name="sVlaue">灰度数据</param>
        /// <param name="bf">灰度数组</param>
        [DllImport(mmmDllFile, EntryPoint = "GetTemperature", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern float GetTemperatureByGrayValue(ushort sVlaue, ushort[] bf);

        /// <summary>
        /// 传入温度数据，得到灰度数据
        /// </summary>
        /// <param name="fVlaue">温度数据</param>
        [DllImport(mmmDllFile, EntryPoint = "GetGrayValueByTemperature", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern ushort GetGrayValueByTemperature(float temperature);

        /// <summary>
        /// 操作：在定义的回调函数里，将sbuf里面的320*240个WORD数据拷贝出来
        /// </summary>
        /// <param name="pWnd">窗口句柄</param>
        /// <param name="callBack">回调函数句柄</param>
        /// <param name="sbuf">获取到的灰度数组</param>
        [DllImport(mmmDllFile, EntryPoint = "OpenDevice", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long OpenDevice(IntPtr pWnd, IntPtr callBack, ushort[] sbuf);


        /// <summary>
        /// 重载不同镜头打开USB设备的函数
        /// </summary>
        /// <param name="pWnd">窗口句柄</param>
        /// <param name="callBack">回调函数句柄</param>
        /// <param name="sbuf">获取到的灰度数组</param>
        [DllImport(mmmDllFile, EntryPoint = "OpenDevice", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long OpenDevice(IntPtr pWnd, IntPtr hPar, IntPtr callBack, ushort[] sbuf);

        /// <summary>
        /// 关闭镜头
        /// </summary>
        [DllImport(mmmDllFile, EntryPoint = "CloseDevice", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void CloseDevice();

        /// <summary>
        /// 通过设置温度值从新取得两点校验数据
        /// </summary>
        /// <param name="fTemperature">温度数值</param>
        [DllImport(mmmDllFile, EntryPoint = "ChangeTheDatFile", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern void ChangeTheDatFile(float fTemperature);


        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);

        #endregion
    }
}
