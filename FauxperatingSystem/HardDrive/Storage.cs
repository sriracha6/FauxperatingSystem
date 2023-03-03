using System;
using System.Collections.Specialized;
using System.IO;

namespace FauxperatingSystem
{
    public static class Storage
    {
        public const string STORAGE_DIR = "./hdd/";
        public const string STORAGE_DIR_NAME = "hdd";
        public static string STORAGE_ROOT;
        public const string ROOT = "C:\\";
        public static string CurrentDirectory = "";
        public static string CurrentLOCALDirectory { get { return CurrentDirectory.Replace("C:\\", STORAGE_DIR); } }

        public static void Init()
        {
            if (!Directory.Exists(STORAGE_DIR))
                Directory.CreateDirectory(STORAGE_DIR);
            STORAGE_ROOT = Directory.GetCurrentDirectory() + "\\" + STORAGE_DIR_NAME + "\\";
        }

        public static void ChangeCD(string to)
        {
            Directory.SetCurrentDirectory(to);
            CurrentDirectory = Directory.GetCurrentDirectory().Replace(STORAGE_ROOT, "C:\\");
            if(!Directory.GetCurrentDirectory().Contains(STORAGE_ROOT))
                FauxSystem.Halt("Attempted to access invalid file");
        }

        public static void CreateFile(string path)
        {
            if (path.Length > 255)
                FauxSystem.Halt("MAXPATH of 255 exceeded");
            File.Create(path.Replace("C:\\", STORAGE_ROOT));
        }

        public static void DeleteFile(string path)
        {
            if (path.Length > 255)
                FauxSystem.Halt("MAXPATH of 255 exceeded");
            File.Delete(path.Replace("C:\\", STORAGE_ROOT));
        }

        public static void CreateDirectory(string path)
        {
            if (path.Length > 255)
                FauxSystem.Halt("MAXPATH of 255 exceeded");
            Directory.CreateDirectory(path.Replace("C:\\", STORAGE_ROOT));
        }

        public static void DeleteEmptyDirectory(string path)
        {
            if (path.Length > 255)
                FauxSystem.Halt("MAXPATH of 255 exceeded");
            Directory.Delete(path.Replace("C:\\", STORAGE_ROOT));
        }

        public static void DeleteFullDirectory(string path)
        {
            if (path.Length > 255)
                FauxSystem.Halt("MAXPATH of 255 exceeded");
            Directory.Delete(path.Replace("C:\\", STORAGE_ROOT), true);
        }

        public static int GetDirectoryFileCount(string path)
        {
            if (path.Length > 255)
                FauxSystem.Halt("MAXPATH of 255 exceeded");
            return Directory.GetFiles(path.Replace("C:\\", STORAGE_ROOT)).Length;
        }

        public static long GetDirectorySize(string path)
        {
            if (path.Length > 255)
                FauxSystem.Halt("MAXPATH of 255 exceeded");

            DirectoryInfo d = new DirectoryInfo(path.Replace("C:\\", STORAGE_ROOT));
            return DirSize(d);
        }

        private static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
                size += DirSize(di);
            return size;
        }

        public static string FindFile(string path)
        {
            if (path.Length > 255)
                FauxSystem.Halt("MAXPATH of 255 exceeded");

            if (File.Exists(path.Replace("C:\\", STORAGE_ROOT)))
                return path;

            string[] allFiles = Directory.GetFiles(path.Replace("C:\\", STORAGE_ROOT));
            for(int i = 0; i < allFiles.Length; i++)
            {
                var fauxized = allFiles[i].Replace(STORAGE_ROOT, "C:\\");
                if (fauxized.Contains(path))
                    return fauxized;
            }
            return "\u0000";
        }

        public static long FileSize(string path)
        {
            if (path.Length > 255)
                FauxSystem.Halt("MAXPATH of 255 exceeded");

            string x = path.Replace("C:\\", STORAGE_ROOT);
            if (File.Exists(x))
                return new FileInfo(x).Length;
            else return -1;
        }

        public static void MoveFile(string file, string to)
        {
            if (file.Length > 255 || to.Length > 255)
                FauxSystem.Halt("MAXPATH of 255 exceeded");

            File.Move(file.Replace("C:\\", STORAGE_ROOT), to.Replace("C:\\", STORAGE_ROOT));
        }
    }
}