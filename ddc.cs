/*
 * DeaddropCreator
 * by Dorian Warboys / Red Skäl
 * https://github.com/redskal
 * 
 * This was a derivative of my BAC project. DDC was coded in
 * December 2013. My original blurb follows...
 * ----------------------------------------------------------
 * 
 * With this you can create modular self-extracting toolkits.
 * This should work on any Windows supporting .Net framework.
 * 
 * Compile output using commandline C# compiler (csc.exe)
 * found in framework directory.
 * 
 * Dropper will extract to working directory.  Add a specific
 * target directory if you wish...dropper is open source,
 * after all!
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DeaddropCreator
{
    class ddc
    {
        static void Main(string[] args)
        {
            byte[] buffer = null;

            // because banners are l33t
            System.Console.WriteLine(" [*] DDC - by Dorian Warboys / Red Skäl");
            System.Console.WriteLine(" [*] https://github.com/redskal");
            System.Console.WriteLine(" [*] ");

            if (args.Count() < 2)
            {
                System.Console.WriteLine(" [!] Not enough args!!");
                System.Console.WriteLine(" [!]\tddc.exe <out_file> <in_file> [[[in_file2] in_file3] ...]");
                return;
            }

            System.Console.WriteLine(" [*] Writing stub to {0}", args[0].ToString());
            StreamWriter outFile = new StreamWriter(args[0].ToString(), false);

            // stub
            outFile.WriteLine("using System;");
            outFile.WriteLine("using System.Collections.Generic;");
            outFile.WriteLine("using System.Linq;");
            outFile.WriteLine("using System.Text;");
            outFile.WriteLine("using System.IO;");
            outFile.WriteLine("");
            outFile.WriteLine("namespace ddc");
            outFile.WriteLine("{");
            outFile.WriteLine("\tclass dropper");
            outFile.WriteLine("\t{");
            outFile.WriteLine("\t\tstatic void Main(string[] args)");
            outFile.WriteLine("\t\t{");
            string argc = Convert.ToString(args.Length - 1);
            outFile.Write("\t\tstring[,] deaddrop = new string[");
            outFile.Write(argc);
            outFile.Write(", 2] {\r\n");

            int i = 0;
            foreach (string file in args)
            {
                // Check thyself before one wrecketh thyself
                if (file == args[0]) { i++; continue; }

                if (!(File.Exists(file)))
                {
                    System.Console.WriteLine(" [!] Error: {0} does not exist", file);
                    return;
                }

                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long n = new FileInfo(file).Length;

                buffer = br.ReadBytes((Int32)n);

                Console.WriteLine(" [*] Adding file: {0} - {1} bytes", file, n.ToString());

                string _hex = BitConverter.ToString(buffer);
                
                for (int j = 80; j < _hex.Length; j += 90)
                {
                    _hex = _hex.Insert(j, "\" +\r\n\t\t\t\t\"");
                }
                
                outFile.WriteLine("\t\t\t\t// {0} - {1} bytes", Path.GetFileName(file), n.ToString());
                outFile.Write("\t\t\t\t{ \"");
                outFile.Write(Path.GetFileName(file));
                outFile.Write("\",\r\n\t\t\t\t\"");
                outFile.Write(_hex);
                outFile.Write("\" }");
                if (i != (args.Count() - 1))
                {
                    i++;
                    outFile.Write(",\r\n");
                }
            }

            Console.WriteLine(" [*] Writing extraction code");

            // drop the extraction code
            outFile.WriteLine("\r\n\t\t\t};");
            outFile.WriteLine("");
            outFile.WriteLine("\t\t\tSystem.Console.WriteLine(\"\\n [*] DDC-dropper by Dorian Warboys / Red Skäl\");");
            outFile.WriteLine("\t\t\tSystem.Console.WriteLine(\" [*] https://github.com/redskal\");");
            outFile.WriteLine("\t\t\tSystem.Console.WriteLine(\" [*] \");");
            outFile.WriteLine("\t\t\tSystem.Console.WriteLine(\" [*] Extracting files...\");");
            outFile.WriteLine("");
            outFile.WriteLine("\t\t\tfor (int i = 0; i <= deaddrop.Rank; i++) {");
            outFile.WriteLine("\t\t\t\tSystem.Console.WriteLine(\" [*]\\tDropping {0} - {1}\", deaddrop[i,0], deaddrop[i,1].ToString().Length);");
            outFile.WriteLine("");
            outFile.WriteLine("\t\t\t\tFileStream fs = new FileStream(deaddrop[i, 0], FileMode.Create, FileAccess.Write);");
            outFile.WriteLine("");
            outFile.WriteLine("\t\t\t\tbyte[] bytes = deaddrop[i, 1].Split(\'-\').Select(x => Convert.ToByte(x, 16)).ToArray();");
            outFile.WriteLine("\t\t\t\tfs.Write(bytes, 0, bytes.Length);");
            outFile.WriteLine("");
            outFile.WriteLine("\t\t\t\tfs.Close();");
            outFile.WriteLine("\t\t\t}");
            outFile.WriteLine("");
            outFile.WriteLine("\t\t\tSystem.Console.WriteLine(\" [*]\");");
            outFile.WriteLine("\t\t\tSystem.Console.WriteLine(\" [*] Mission complete...\");");
            outFile.WriteLine("\t\t}");
            outFile.WriteLine("\t}");
            outFile.WriteLine("}");

            outFile.Close();

            Console.WriteLine(" [*]");
            Console.WriteLine(" [*] Process complete.");

        }
    }
}
