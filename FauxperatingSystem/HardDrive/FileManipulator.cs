using System;
using System.Collections.Specialized;
using System.IO;

namespace FauxperatingSystem
{
    public static class FileManipulator
    {
        public static void OpenFile(string path)
        {
            FauxSystem.OpenFileName = path;
            FauxSystem.OpenFileLOCAL = path.Replace("C:\\", Storage.STORAGE_ROOT);
        }

        public static void CloseFile()
        {
            FauxSystem.OpenFileName = "";
            FauxSystem.OpenFileLOCAL = "";
        }

        public static void Write(string text) =>
            File.WriteAllText(FauxSystem.OpenFileLOCAL, text);

        public static void Append(string text) =>
            File.AppendAllText(FauxSystem.OpenFileLOCAL, text);

        public static void LoadIntoMemory(short location, short length)
        {
            ushort l = (ushort)length;
            ushort loc = (ushort)location;

            byte[] file = File.ReadAllBytes(FauxSystem.OpenFileLOCAL).Take(l).ToArray();

            for(int i = loc; i < file.Length; i++)
                FauxSystem.Memory[i] = file[i - loc];
        }
    }
}