using System.Runtime.InteropServices;

public class libvk7016n
{
    public enum SyncDir
    {
        SyncDisable_DirIn = 0,
        SyncDisable_DirOut = 1,
        SyncEnable_DirIn = 2,
        SyncEnable_DirOut = 3,
        Sync_Dir_Error = 4
    }

    public enum FilterType
    {
        FilterType_Sinc5 = 0,
        FilterType_Wideband = 1,
        FilterType_Error = 2
    }

    public enum PGIAInputType
    {
        PGIAInputType_Differential_ADC = 0,
        PGIAInputType_Single_Ended_ADC = 1,
        PGIAInputType_Single_Ended_IEPE = 2,
        PGIAInputType_Error = 3
    }

    public enum InputRange
    {
        InputRange_10V = 0,
        InputRange_5V = 1,
        InputRange_2_5V = 2,
        InputRange_1V = 3,
        InputRange_500mV = 4,
        InputRange_100mV = 5,
        InputRange_50mV = 6,
        InputRange_20mV = 7,
        InputRange_Error = 8
    }

    public enum TriggerMode
    {
        TriggerMode_Disable = 0,
        TriggerMode_Default = 1,
        TriggerMode_Posedge = 2,
        TriggerMode_Negedge = 3,
        TriggerMode_High_Level = 4,
        TriggerMode_Low_Level = 5,
        TriggerMode_Error = 6
    }

    public enum TriggerSource
    {
        TriggerSource_DIO4 = 0,
        TriggerSource_ADC0 = 1,
        TriggerSource_ADC1 = 2,
        TriggerSource_ADC2 = 3,
        TriggerSource_ADC3 = 4,
        TriggerSource_ADC4 = 5,
        TriggerSource_ADC5 = 6,
        TriggerSource_ADC6 = 7,
        TriggerSource_ADC7 = 8,
        TriggerSource_ADC8 = 9,
        TriggerSource_ADC9 = 10,
        TriggerSource_ADC10 = 11,
        TriggerSource_ADC11 = 12,
        TriggerSource_ADC12 = 13,
        TriggerSource_ADC13 = 14,
        TriggerSource_ADC14 = 15,
        TriggerSource_ADC15 = 16,
        TriggerSource_Error = 17
    }

    public enum AOUT
    {
        AOUT1 = 0,
        AOUT2 = 1,
        AOUT_Error = 2
    }

    public enum OfflineSave
    {
        OfflineSave_Disable = 0,
        OfflineSave_Enable = 1,
        OfflineSave_Error = 2
    }

    public enum SaveFormat
    {
        SaveFormat_BIN = 0,
        SaveFormat_CSV = 1,
        SaveFormat_Error = 2
    }

    public enum SaveParam
    {
        SaveParam_Disable = 0,
        SaveParam_Enable = 1,
        SaveParam_Error = 2
    }

    public enum SaveChannel
    {
        SaveChannel_Disable = 0,
        SaveChannel_Enable = 1,
        SaveChannel_Error = 2
    }

    //2.1 获取DLL函数的版本信息
    //2.1 Get the version information of DLL
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr VK7016N_GetVersionLot();
    //3.1 获取USB设备列表
    //3.1 Get USB device list
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetUSBDeviceList(IntPtr[] list, int size);
    //3.2 通过编号获取USB采集卡名称
    //3.2 Get USB device by number
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr VK7016N_GetUSBDeviceUri(int num);
    //3.3 创建USB连接
    //3.3 Create USB connection
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr VK7016N_CreateUSBContext(IntPtr ptr);
    //4.1 创建连接
    //4.1 Create a network connection
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr VK7016N_CreateNetworkContext(IntPtr ptr);
    //4.2 销毁连接
    //4.2 Destroy Connection
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern void VK7016N_ContextDestroy(ref IntPtr vk7016n);
    //4.3 获取主机名
    //4.3 Get hostname
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr VK7016N_GetHostname(IntPtr vk7016n);
    //4.4 设置主机名
    //4.4 Set hostname
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetHostname(IntPtr vk7016n, IntPtr hostname);
    //5.2.1 获取同步类型
    //5.2.1 Get Sync Dir
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetSyncDir(IntPtr vk7016n, IntPtr sync_dir);
    //5.2.2 设置同步类型
    //5.2.2 Set Sync Dir
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetSyncDir(IntPtr vk7016n, int sync_dir);
    //5.2.3 获取ADC通道数量
    //5.2.3 Get the number of ADC channels
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetNumberOfChannels(IntPtr vk7016n, IntPtr nb_channels);
    //5.2.4 设置ADC通道数量
    //5.2.4 Set the number of ADC channels
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetNumberOfChannels(IntPtr vk7016n, int nb_channels);
    //5.2.5 获取采样频率
    //5.2.5 Get sampling frequency
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetSamplingFrequency(IntPtr vk7016n, IntPtr sampling_frequency);
    //5.2.6 设置采样频率
    //5.2.6 Set sampling frequency
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetSamplingFrequency(IntPtr vk7016n, int sampling_frequency);
    //5.2.7 获取滤波器类型
    //5.2.7 Get filter type
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetFilterType(IntPtr vk7016n, IntPtr chname, IntPtr type);
    //5.2.8 设置滤波器类型
    //5.2.8 Set filter type
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetFilterType(IntPtr vk7016n, IntPtr chname, int type);
    //5.2.9 获取PGIA输入类型
    //5.2.9 Get PGIA input type
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Get_PGIA_InputType(IntPtr vk7016n, IntPtr chname, IntPtr type);
    //5.2.10 设置PGIA输入类型
    //5.2.10 Set PGIA input type
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Set_PGIA_InputType(IntPtr vk7016n, IntPtr chname, int type);
    //5.2.11 设置输入电压范围
    //5.2.11 Get input range
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetInputRange(IntPtr vk7016n, IntPtr chname, IntPtr range);
    //5.2.12 设置输入电压范围
    //5.2.12 Set input range
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetInputRange(IntPtr vk7016n, IntPtr chname, int range);
    //5.2.14 获取触发源
    //5.2.14 Get trigger source
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetTriggerSource(IntPtr vk7016n, IntPtr source, IntPtr level);
    //5.2.15 设置触发源
    //5.2.15 Set trigger source
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetTriggerSource(IntPtr vk7016n, int source, double level);
    //5.2.16 获取触发模式
    //5.2.16 Get trigger mode
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetTriggerMode(IntPtr vk7016n, IntPtr mode);
    //5.2.17 设置触发模式
    //5.2.17 Set trigger mode
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetTriggerMode(IntPtr vk7016n, int mode);
    //5.2.18 获取负延时
    //5.2.18 Get negative delay
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetNegativeDelay(IntPtr vk7016n, IntPtr delay);
    //5.2.19 设置负延时
    //5.2.19 Set negative delay
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetNegativeDelay(IntPtr vk7016n, double delay);
    //5.2.20 获取ADC通道偏置修正值
    //5.2.20 Get ADC channel offset value
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetOffset(IntPtr vk7016n, IntPtr chname, IntPtr offset);
    //5.2.21 设置ADC通道偏置修正值
    //5.2.21 Set ADC channel offset value
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetOffset(IntPtr vk7016n, IntPtr chname, double offset);
    //5.3.1 开始采样
    //5.3.1 Start sampling
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_StartSampling(IntPtr vk7016n);
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_StartSampling_WithTrigger(IntPtr vk7016n, int mode);
    //5.3.2 开始N采样
    //5.3.2 Start sampling N points
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_StartSampling_NPoints(IntPtr vk7016n, int Npointsnums);
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_StartSampling_NPoints_WithTrigger(IntPtr vk7016n, int mode, int Npointsnums);
    //5.3.3 停止采样
    //5.3.3 Stop sampling
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_StopSampling(IntPtr vk7016n);
    //5.3.4 获取负延时采样数据
    //5.3.4 Get negative delay sampling data
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetNegativeAllChannel(IntPtr vk7016n, IntPtr adcbuffer, int rsamplenum, int waittime);
    //5.3.5 获取采样数据
    //5.3.5 Get sampling data
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetAllChannel(IntPtr vk7016n, IntPtr adcbuffer, int rsamplenum, int waittime);
    //6.1.1 获取GPIO方向
    //6.1.1 Get GPIO dir
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Get_GPIO_Dir(IntPtr vk7016n, IntPtr dir1, IntPtr dir2, IntPtr dir3, IntPtr dir4);
    //6.1.2 设置GPIO方向
    //6.1.2 Set GPIO dir
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Set_GPIO_Dir(IntPtr vk7016n, int dir1, int dir2, int dir3, int dir4);
    //6.1.3 获取GPIO数据
    //6.1.3 Get GPIO data
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Get_GPIO_Data(IntPtr vk7016n, IntPtr data1, IntPtr data2, IntPtr data3, IntPtr data4);
    //6.1.4 设置GPIO数据
    //6.1.4 Set GPIO data
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Set_GPIO_Data(IntPtr vk7016n, int data1, int data2, int data3, int data4);
    //6.1.5 设置DAC输出
    //6.1.5 Set DAC output
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Init_DAC(IntPtr vk7016n, double dac1value, double dac2value);
    //6.1.6 获取DAC输出电压
    //6.1.6 Get DAC output voltage
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetAout(IntPtr vk7016n, int ch, IntPtr value);
    //6.1.7 设置DAC输出电压
    //6.1.7 Set DAC output voltage
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetAout(IntPtr vk7016n, int ch, double value);
    //6.1.8 设置DAC输出波形
    //6.1.8 Set DAC output waveform
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetAoutWave(IntPtr vk7016n, int rate, IntPtr aout1wave, IntPtr aout2wave, int length);
    //6.1.9 设置频率采样模式
    //6.1.9 Set frequency sampling mode
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Set_exFreq(IntPtr vk7016n, uint ms);
    //6.1.10 获取频率
    //6.1.10 Get frequency value
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Get_exFreq(IntPtr vk7016n, IntPtr freq, int waittime);
    //6.1.11 设置计数模式
    //6.1.11 Set counter mode
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Set_Counter(IntPtr vk7016n, uint ms);
    //6.1.12 获取计数器的值
    //6.1.12 Get counter value
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Get_Counter(IntPtr vk7016n, IntPtr cnt, int waittime);
    //6.1.13 设置PWM1输出
    //6.1.13 Set PWM1 output
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Set_PWM1(IntPtr vk7016n, uint freq, double perc);
    //6.1.14 设置PWM2输出
    //6.1.14 Set PWM2 output
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_Set_PWM2(IntPtr vk7016n, uint freq, double perc);
    //6.1.15 获取板载温度传感器值
    //6.1.15 Get the onboard temperature sensor value
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetTemperature(IntPtr vk7016n, IntPtr temperature);
    //6.1.16 获取旋转编码器0计数值
    //6.1.16 Get the rotary encoder0 count value
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetRotaryEncoder0(IntPtr vk7016n, IntPtr cnt);
    //6.1.17 获取旋转编码器1计数值
    //6.1.17 Get the rotary encoder1 count value
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetRotaryEncoder1(IntPtr vk7016n, IntPtr cnt);
    //6.1.18 清零旋转编码器0计数值
    //6.1.18 Clear the rotary encoder0 count
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_ClearRotaryEncoder0(IntPtr vk7016n);
    //6.1.19 清零旋转编码器1计数值
    //6.1.19 Clear the rotary encoder1 count
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_ClearRotaryEncoder1(IntPtr vk7016n);
    //7.2.1 获取离线保存状态
    //7.2.1 Get offline save status
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetOfflineSave(IntPtr vk7016n, IntPtr offlinesave);
    //7.2.2 设置离线保存状态
    //7.2.2 Set offline save status
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetOfflineSave(IntPtr vk7016n, int offlinesave);
    //7.2.3 获取可用的离线保存目录
    //7.2.3 Get available offline save directory
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetSaveDirAvailable(IntPtr vk7016n, IntPtr[] list, int size);
    //7.2.4 获取离线保存目录
    //7.2.4 Get offline save directory
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern IntPtr VK7016N_GetSaveDir(IntPtr vk7016n);
    //7.2.5 设置离线保存目录
    //7.2.5 Set offline save directory
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetSaveDir(IntPtr vk7016n, IntPtr dir);
    //7.2.6 获取离线保存格式
    //7.2.6 Get offline save format
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetSaveFormat(IntPtr vk7016n, IntPtr format);
    //7.2.7 设置离线保存格式
    //7.2.7 Set offline save format
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetSaveFormat(IntPtr vk7016n, int format);
    //7.2.8 获取离线保存参数状态
    //7.2.8 Get offline saved parameter status
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetSaveParam(IntPtr vk7016n, IntPtr param);
    //7.2.9 设置离线保存参数状态
    //7.2.9 Set offline saved parameter status
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetSaveParam(IntPtr vk7016n, int param);
    //7.2.10 获取离线保存时间
    //7.2.10 Get offline save time
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetSaveTime(IntPtr vk7016n, IntPtr time);
    //7.2.11 设置离线保存时间
    //7.2.11 Get offline save time
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetSaveTime(IntPtr vk7016n, int time);
    //7.2.12 获取通道离线保存状态
    //7.2.12 Get offline save state of the channel
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetSaveChannel(IntPtr vk7016n, IntPtr chname, IntPtr save);
    //7.2.13 设置通道离线保存状态
    //7.2.13 Set offline save state of the channel
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetSaveChannel(IntPtr vk7016n, IntPtr chname, int save);
    //7.2.14 获取定时N采样状态
    //7.2.14 Get offline save timed N sampling status
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_GetTimeIntervalNSamples(IntPtr vk7016n, IntPtr time_interval, IntPtr n_samples);
    //7.2.15 设置定时N采样状态
    //7.2.15 Set offline save timed N sampling status
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern int VK7016N_SetTimeIntervalNSamples(IntPtr vk7016n, int time_interval, int n_samples);
    //浮点数转双长整型
    //double to long long
    [DllImport("libvk7016n.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
    public static extern long double2longlong(double val);
}
