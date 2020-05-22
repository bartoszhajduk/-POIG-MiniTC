using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace MiniTC.Model
{
    public class Model
    {
        Boolean IsFolder = true;
        public string Name { get; private set; }
        public List<string> Drives { get; private set; } = new List<string>();
        public List<string> Files { get; private set; } = new List<string>();


        private void prevPath()
        {
            string[] values = Name.Split('\\');
            int len = values[values.Length - 2].Length;
            Name = Name.Remove(Name.Length - len - 1, len + 1);
        }
 
        public void setPath(string path)
        {
            string tmp = Name;
            try
            {
                if (IsFolder == false)
                {
                    string[] values = Name.Split('\\');
                    int len = values[values.Length - 2].Length;
                    Name = Name.Remove(Name.Length - len - 1, len + 1);
                }
                if (path.Contains("<D>"))
                {
                    IsFolder = true;
                    Name += path.Remove(0, 3) + '\\';
                }
                else if (path == "...")
                {
                    IsFolder = true;
                    string[] values = Name.Split('\\');
                    int len = values[values.Length - 2].Length;
                    Name = Name.Remove(Name.Length - len - 1, len + 1);
                }
                else if (path.Length <= 4)
                {
                    IsFolder = true;
                    Name = path;
                }
                else
                {
                    IsFolder = false;
                    Name += path + '\\';
                }
                Directory.GetAccessControl(Name);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Brak dostępu");
                Name = tmp;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Nie Znaleziono pliku");
                IsFolder = true;
                prevPath();
            }
            catch (DirectoryNotFoundException)
            {
                IsFolder = true;
                while (Directory.Exists(Name) == false)
                {
                    MessageBox.Show("Nie Znaleziono ścieżki");
                    prevPath();
                }
                getCurrentFilesAndFolders();
            }
        }

        public List<string> getLogicalDrives()
        {
            Drives.Clear();
            string[] tmp = Directory.GetLogicalDrives();
            foreach (var i in tmp)
            {
                Drives.Add(i);
            }
            return Drives;
        }

        public List<string> getCurrentFilesAndFolders()
        {
            try
            {
                if (IsFolder == true)
                {
                    Files.Clear();
                    if (Name.Length > 4)
                    {
                        Files.Add("...");
                    }
                    string[] tmp = Directory.GetDirectories(Name);
                    string[] tmp2 = Directory.GetFiles(Name);
                    foreach (var i in tmp)
                    {
                        string name = new FileInfo(i).Name;
                        Files.Add("<D>" + name);

                    }
                    foreach (var i in tmp2)
                    {
                        string name = new FileInfo(i).Name;
                        Files.Add(name);
                    }
                }
            }
            catch(DirectoryNotFoundException)
            {
                MessageBox.Show("Folder nadrzędny już nie istnieje");
                IsFolder = true;
                while (Directory.Exists(Name) == false)
                {
                    prevPath();       
                }
                getCurrentFilesAndFolders();
            }    
            return Files;
        }

        public void CopyFile(Model Output)
        {
            try
            {
                if (IsFolder == false)
                {
                    string[] values = Name.Split('\\');
                    if (Output.IsFolder == true)
                    {
                        File.Copy(Name, Output.Name + values[values.Length - 2]);
                    }
                    else
                    {
                        Output.prevPath();
                        File.Copy(Name, Output.Name + values[values.Length - 2]);   
                        
                        Output.IsFolder = true;
                        Output.Files.Clear();
                        if (Output.Name.Length > 4)
                        {
                            Output.Files.Add("...");
                        }
                        string[] tmp = Directory.GetDirectories(Output.Name);
                        string[] tmp2 = Directory.GetFiles(Output.Name);
                        foreach (var i in tmp)
                        {
                            string name = new FileInfo(i).Name;
                            Output.Files.Add("<D>" + name);

                        }
                        foreach (var i in tmp2)
                        {
                            string name = new FileInfo(i).Name;
                            Output.Files.Add(name);
                        }
                    } 
                }
                else 
                {
                    MessageBox.Show("Nie wybrano pliku do kopiowania");
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Plik już istnieje w podanej ścieżce lub kopiowany element już nie istnieje");
                IsFolder = true;
                while (Directory.Exists(Name) == false)
                {
                    prevPath();
                }
                Output.IsFolder = true;
                while (Directory.Exists(Output.Name) == false)
                {
                    Output.prevPath();
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Brak uprawnień dostępu");
            }
        }
    }
}
