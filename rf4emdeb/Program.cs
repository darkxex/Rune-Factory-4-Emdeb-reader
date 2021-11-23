using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace rf4emdeb
{
    class Program
    {
        struct Header
        {
            public byte[] idtype;
            public int idcode;


        }
        static void Main(string[] args)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Header header;
            String namefile = @"C:\Users\darkx\source\repos\rf4emdeb\rf4emdeb\bin\Debug\net5.0\emdeb.bin";
            using (BinaryReader reader = new BinaryReader(File.Open(namefile, FileMode.Open)))
            {
                header.idtype = reader.ReadBytes(4);
                header.idcode = reader.ReadInt32();
                reader.BaseStream.Position = 0x30;
                long savepos = reader.BaseStream.Position;
                int firstLength = reader.ReadInt32();
                int firstOffset = reader.ReadInt32();
                string text = Encoding.UTF8.GetString(header.idtype);
                Console.WriteLine("Type: " + text);
                Console.WriteLine("Code: " + header.idcode.ToString("X"));

                Console.WriteLine("Initial Offset: " + firstOffset.ToString("X"));
                reader.BaseStream.Position = savepos;

                List<string> result = new List<string>();
                while (reader.BaseStream.Position < 0x2234)
                {
                    
                   
                    int textLength = reader.ReadInt32();

                    if (textLength == 0x100)
                    { textLength = reader.ReadInt32();
                        Console.WriteLine("hey");
                    }

                    int textOffset = reader.ReadInt32();
                    if (textOffset == 0x100)
                    { textOffset = reader.ReadInt32();
                        Console.WriteLine("hey");
                    }
                    long savePosLength = reader.BaseStream.Position;
                    long savePosOffset = reader.BaseStream.Position;
                   
                    Console.WriteLine("Length: " + textLength.ToString("X"));
                    Console.WriteLine("Offset: " + textOffset.ToString("X"));
                    Console.WriteLine("Real Offset: " + reader.BaseStream.Position.ToString("X"));
                    long gotooffset = reader.BaseStream.Position;
                    reader.BaseStream.Position = textOffset;
                    Byte[] texttemp = reader.ReadBytes(textLength);
                    string texttemputf8 = Encoding.GetEncoding(932).GetString(texttemp).Replace("\n", "{LF}");
                    result.Add(texttemputf8);


                    //Console.WriteLine("Line: " + texttemputf8);
                    reader.BaseStream.Position = gotooffset;


                }

                while (reader.BaseStream.Position < 0x2234)
                {


                    int textLength = reader.ReadInt32();

                    if (textLength == 0x100)
                    {
                        textLength = reader.ReadInt32();
                        Console.WriteLine("hey");
                    }

                    int textOffset = reader.ReadInt32();
                    if (textOffset == 0x100)
                    {
                        textOffset = reader.ReadInt32();
                        Console.WriteLine("hey");
                    }
                    long savePosLength = reader.BaseStream.Position;
                    long savePosOffset = reader.BaseStream.Position;

                    Console.WriteLine("Length: " + textLength.ToString("X"));
                    Console.WriteLine("Offset: " + textOffset.ToString("X"));
                    Console.WriteLine("Real Offset: " + reader.BaseStream.Position.ToString("X"));
                    long gotooffset = reader.BaseStream.Position;
                    reader.BaseStream.Position = textOffset;
                    Byte[] texttemp = reader.ReadBytes(textLength);
                    string texttemputf8 = Encoding.GetEncoding(932).GetString(texttemp).Replace("\n", "{LF}");
                    result.Add(texttemputf8);


                    //Console.WriteLine("Line: " + texttemputf8);
                    reader.BaseStream.Position = gotooffset;


                }


                reader.Close();
                File.WriteAllLines(namefile + ".txt", result);
                Console.WriteLine(namefile + ".txt Generated.");
            }
        }
    }
}
