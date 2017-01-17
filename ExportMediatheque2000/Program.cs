using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportMediatheque2000
{
    class Program
    {
        static void Read(byte[] buffer, System.IO.BinaryReader br, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                buffer[0] = buffer[1];
                buffer[1] = buffer[2];
                buffer[2] = br.ReadByte();
            }
        }

        static void Main(string[] args)
        {
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("out.txt"))
                {
                    using (System.IO.BinaryReader br = new System.IO.BinaryReader(System.IO.File.OpenRead("NOT2709")))
                    {
                        string acc = "";
                        byte[] buffer = new byte[3];
                        Read(buffer, br, 3);
                        int j = 0;
                        do
                        {
                            // 1E 20 20 = nouveau champ
                            // 1F xx = valeur
                            if (buffer[0] == 0x1e)
                                if (buffer[1] == 0x20)
                                    if (buffer[2] == 0x20)
                                    {
                                        if (acc.Contains("cam0"))
                                            sw.WriteLine();
                                        sw.Write(acc + "\t");
                                        acc = "";
                                        Read(buffer, br, 3);
                                    }
                            if (buffer[0] <= 0x1f)
                                Read(buffer, br, 2);

                            acc += Convert.ToChar(buffer[0]);
                            Read(buffer, br, 1);
                            j++;
                        }
                        while (j < 10000);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
