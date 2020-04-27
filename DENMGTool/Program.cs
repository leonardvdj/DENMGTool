using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DENMGTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Looking for game...");
            while (true)
            {
                Process game = Process.GetProcessesByName("DOOMEternalx64vk").FirstOrDefault() ?? null;
                if (game != null)
                {
                    try
                    {
                        IntPtr baseAddr = game.MainModule.BaseAddress;
                        int offset = 0x0;
                        int moduleSize = game.MainModule.ModuleMemorySize;
                        if (moduleSize == 507191296 || moduleSize == 515133440 || moduleSize == 510681088) // Steam version
                        {
                            offset = 0x6126430;
                        }
                        else if (moduleSize == 450445312 || moduleSize == 444944384) // Bethesda version
                        {
                            offset = 0x60E8130;
                        }
                        if ((long)game.MainWindowHandle != 0)
                        {
                            game.WriteValue<int>(baseAddr + offset, 0);
                            Thread.Sleep(1000);
                            int state = 1;
                            game.ReadValue<int>(baseAddr + offset, out state);
                            if (state == 0)
                            {
                                Console.WriteLine("pm_allowRampJumping set to 0. Press any key to exit...");
                                Console.ReadKey();
                                break;
                            }
                        }
                    }
                    catch (Win32Exception ex)
                    {
                        
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
