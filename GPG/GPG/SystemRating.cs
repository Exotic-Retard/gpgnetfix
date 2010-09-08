namespace GPG
{
    using GPG.Logging;
    using Microsoft.Win32;
    using System;
    using System.Management;

    public static class SystemRating
    {
        public static double GetRating()
        {
            try
            {
                double num = 0.0;
                int processorCount = Environment.ProcessorCount;
                if (processorCount == 2)
                {
                    num += 2.0;
                }
                else if (processorCount > 2)
                {
                    num += 4.0;
                }
                int num3 = 0x3e8;
                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0");
                    if (key != null)
                    {
                        num3 = (int) key.GetValue("~MHz");
                    }
                }
                catch (Exception exception)
                {
                    ErrorLog.WriteLine(exception);
                }
                if (num3 >= 0x3e8)
                {
                    if (num3 < 0x7d0)
                    {
                        num++;
                    }
                    else if (num3 < 0x898)
                    {
                        num += 1.5;
                    }
                    else if (num3 < 0x960)
                    {
                        num += 2.0;
                    }
                    else if (num3 < 0xa28)
                    {
                        num += 2.5;
                    }
                    else if (num3 < 0xaf0)
                    {
                        num += 3.0;
                    }
                    else if (num3 < 0xbb8)
                    {
                        num += 3.5;
                    }
                    else if (num3 < 0xc80)
                    {
                        num += 4.5;
                    }
                    else
                    {
                        num += 5.0;
                    }
                }
                double num4 = 0.0;
                ObjectQuery query = new ObjectQuery("select * from Win32_PhysicalMemory");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject obj2 in searcher.Get())
                {
                    num4 += Convert.ToDouble(obj2.GetPropertyValue("Capacity"));
                }
                num4 /= 1048576.0;
                if (num4 < 500.0)
                {
                    num -= 3.0;
                }
                else if (num4 < 1024.0)
                {
                    num--;
                }
                else if (num4 >= 1532.0)
                {
                    if (num4 < 2048.0)
                    {
                        num++;
                    }
                    else if (num4 < 3072.0)
                    {
                        num += 2.0;
                    }
                    else
                    {
                        num += 3.0;
                    }
                }
                double num5 = 0.0;
                ManagementScope scope = new ManagementScope(@"\\localhost\root\cimv2");
                query = new ObjectQuery("SELECT * FROM Win32_VideoController");
                searcher = new ManagementObjectSearcher(scope, query);
                foreach (ManagementObject obj3 in searcher.Get())
                {
                    double num6 = (double.Parse(obj3.GetPropertyValue("AdapterRAM").ToString()) / 1024.0) / 1024.0;
                    if (num6 > num5)
                    {
                        num5 = num6;
                        obj3.GetPropertyValue("VideoProcessor").ToString();
                    }
                }
                if (num5 < 256.0)
                {
                    num -= 5.0;
                }
                else if (num5 >= 512.0)
                {
                    if (num5 < 768.0)
                    {
                        num++;
                    }
                    else if (num5 < 1024.0)
                    {
                        num += 2.0;
                    }
                    else
                    {
                        num += 3.0;
                    }
                }
                return num;
            }
            catch (Exception exception2)
            {
                ErrorLog.WriteLine(exception2);
                return 0.0;
            }
        }
    }
}

