using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerWithStream
{
    class Manager
    {
        public Manager()
        {
        }
        public string adressName { get; set; }
        private bool AdressHaving()
        {
            return File.Exists(adressName);
        }
        public void FileManager()
        {
            SelectDisk();
            while (true)
            {
                Console.WriteLine("If you want to select a different folder or file, click Enter");
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    InFolderOrFile();
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    BackFolder();
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    throw new OperationCanceledException();
                }
            }
        }
        public void BackFolder()
        {
            try
            {
                adressName = adressName.Remove(adressName.Length - 1);
                adressName = Path.GetDirectoryName(adressName);
                OutPutFoldersAndFiles();
            }
            catch (ArgumentNullException)
            {
                SelectDisk();
            }
        }
        private void InFolderOrFile()
        {
            Console.WriteLine("Select a folder");
            while (true)
            {
                var saveAdress = adressName;
                try
                {
                    var fileName = Console.ReadLine();
                    var adress = $"{fileName}\\";
                    adressName = $"{adressName}{adress}";
                    OutPutFoldersAndFiles();
                    break;
                }
                catch (DirectoryNotFoundException ex)
                {
                    adressName = saveAdress;
                    Console.WriteLine($"Bed input {ex}, try again");
                }
                catch (IOException)
                {
                    adressName = adressName.Substring(0, adressName.Length - 1);
                    FileStreamClass fileStreamClass = new FileStreamClass(adressName);
                    fileStreamClass.RedactFile();
                    BackFolder();
                    break;
                }
            }
        }
        private void SelectDisk()
        {
            Console.Write("You have disks: ");
            var allDisks = Directory.GetLogicalDrives();
            foreach (var disk in allDisks)
            {
                Console.Write($"{disk}, ");
            }
            Console.WriteLine();
            Console.WriteLine("Select a disc");
            while (true)
            {
                Console.WriteLine("Write name your disk");
                var line = Console.ReadLine();
                adressName = $"{line}:\\";
                var flag = false;
                foreach (var disk in allDisks)
                {
                    if (disk == adressName)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    OutPutFoldersAndFiles();
                    break;
                }
            }
        }
        private void OutPutFoldersAndFiles()
        {
            var allDirectories = Directory.GetDirectories(adressName);
            var allFiles = Directory.GetFiles(adressName);
            //var allFiles = allDirectories.Concat(allDocument);
            if (allDirectories.Length != 0)
            {
                Console.WriteLine();
                Console.WriteLine("This directory has following directories: ");
                foreach (var directory in allDirectories)
                {
                    Console.WriteLine($"{Path.GetFileName(directory)}, ");
                }
            }
            if (allFiles.Length != 0)
            {
                Console.WriteLine();
                Console.WriteLine("This directory has following files: ");
                foreach (var file in allFiles)
                {
                    Console.WriteLine($"{Path.GetFileName(file)}, ");
                }
            }
            Console.WriteLine();
        }
    }
}
