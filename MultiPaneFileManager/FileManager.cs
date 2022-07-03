using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace MultiPaneFileManager
{
    public partial class FileManager : UserControl
    {
        public string CurrentDrive { get => (string)DrivesComboBox.SelectedItem; set => DrivesComboBox.SelectedItem = value; }
        public string CurrentPath { get; set; }

        /// <summary>
        /// Open folder watcher. Required to update the list when content changes
        /// </summary>
        private FileSystemWatcher CurrentWatcher { get; set; }

        /// <summary>
        /// Context menu for selected files and folders
        /// </summary>
        private ContextMenuStrip FileContextMenu { get; }
        /// <summary>
        /// Context menu for open folder
        /// </summary>
        private ContextMenuStrip DirectoryContextMenu { get; }
        /// <summary>
        /// Last mouse button pressed
        /// </summary>
        private MouseButtons LastMouseButtonUp { get; set; }
        
        private enum DataType
        {
            Directory,
            File,
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="imageList">Images for list with files</param>
        public FileManager(ImageList imageList)
        {
            InitializeComponent();
            FilesListView.SmallImageList = imageList;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            AutoSize = true;
            Dock = DockStyle.Fill;

            FileContextMenu = new();

            ToolStripMenuItem copy = new("Copy");
            ToolStripMenuItem move = new("Paste");
            ToolStripMenuItem rename = new("Rename");
            ToolStripMenuItem delete = new("Delete");

            copy.Click += ContextMenuCopy_Click;
            move.Click += ContextMenuMove_Click;
            rename.Click += ContextMenuRename_Click;
            delete.Click += ContextMenuDelete_Click;

            FileContextMenu.Items.AddRange(new[] { copy, move, rename, delete });

            DirectoryContextMenu = new();

            ToolStripMenuItem createFile = new("Create file");
            ToolStripMenuItem createDirectory = new("Create directory");
            ToolStripMenuItem paste = new("Paste");

            createFile.Click += ContextMenuCreateFile_Click;
            createDirectory.Click += ContextMenuCreateDirectory_Click;
            paste.Click += ContextMenuPaste_Click;

            DirectoryContextMenu.Items.AddRange(new[] { createFile, createDirectory, paste });

            UpdateDrives();
        }

        /// <summary>
        /// Method for changing the current open folder
        /// </summary>
        /// <param name="newPath">New path</param>
        private void ChangeDirectory(string newPath)
        {
            if (newPath is null)
            {
                return;
            }

            if (!Directory.Exists(newPath))
            {
                MessageBox.Show("Путь не существует", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Directory.GetDirectories(newPath);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Not enough rights", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Path.GetPathRoot(newPath) != CurrentDrive)
            {
                DrivesComboBox.SelectedItem = Path.GetPathRoot(newPath);
            }

            CurrentPath = newPath;
            DirectoryTextBox.Text = CurrentPath;

            CurrentWatcher?.Dispose();
            CurrentWatcher = new(CurrentPath)
            {
                NotifyFilter = NotifyFilters.LastWrite 
                    | NotifyFilters.Size
                    | NotifyFilters.LastAccess
                    | NotifyFilters.FileName
                    | NotifyFilters.DirectoryName
                    | NotifyFilters.FileName

            };
            CurrentWatcher.Changed += OnCurrentDirectoryChanged;
            CurrentWatcher.Created += OnCurrentDirectoryChanged;
            CurrentWatcher.Deleted += OnCurrentDirectoryChanged;
            CurrentWatcher.Renamed += OnCurrentDirectoryChanged;

            CurrentWatcher.EnableRaisingEvents = true;

            UpdateFilesListView();
        }

        /// <summary>
        /// Method for changing the contents of an open folder
        /// </summary>
        private void OnCurrentDirectoryChanged(object sender, FileSystemEventArgs e)
        {
            FilesListView.Invoke(new MethodInvoker(UpdateFilesListView));
        }

        /// <summary>
        /// Method for updating the list of folders and files
        /// </summary>
        private void UpdateFilesListView()
        {
            FilesListView.Items.Clear();
            DirectoryInfo directory = new(CurrentPath);
            DirectoryInfo[] subDirs = directory.GetDirectories();

            foreach (DirectoryInfo subDir in subDirs)
            {
                ListViewItem item = new(
                    new string[] { 
                        subDir.Name, 
                        "Directory", 
                        string.Empty, 
                        subDir.CreationTime.ToString() 
                }) { 
                    ImageIndex = FileIconsIndexes.Directory, 
                    Tag = DataType.Directory 
                };
                FilesListView.Items.Add(item);
            }

            foreach (FileInfo file in directory.GetFiles())
            {
                ListViewItem item = new(
                    new string[] { 
                        file.Name, 
                        file.Extension.Trim('.', ' ').ToUpper(), 
                        file.Length.ToString(), 
                        file.CreationTime.ToString() 
                }) { 
                    ImageIndex = FileIconsIndexes.File, 
                    Tag = DataType.File 
                };
                FilesListView.Items.Add(item);
            }
        }

        /// <summary>
        /// Method for updating the list with disks
        /// </summary>
        private void UpdateDrives()
        {
            DrivesComboBox.Items.Clear();

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                DrivesComboBox.Items.Add(drive.Name);
            }

            if (DrivesComboBox.SelectedIndex == -1)
            {
                DrivesComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Method for changing the current drive in the list
        /// </summary>
        private void DrivesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeDirectory((string)DrivesComboBox.SelectedItem);
        }

        /// <summary>
        /// Method for changing the current folder when pressing Enter
        /// </summary>
        private void DirectoryTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangeDirectory(DirectoryTextBox.Text);
            }
        }

        /// <summary>
        /// Method for changing the current folder when double-clicking on a folder
        /// </summary>
        private void FilesListView_DoubleClick(object sender, EventArgs e)
        {
            if (LastMouseButtonUp == MouseButtons.Left)
            {
                ListViewItem item = FilesListView.SelectedItems[0];
                if ((DataType)item.Tag == DataType.Directory)
                {
                    ChangeDirectory(Path.Combine(CurrentPath, item.Text));
                }
                else
                {
                    ProcessStartInfo psi = new();
                    psi.FileName = Path.Combine(CurrentPath, item.Text);
                    psi.UseShellExecute = true;
                    try
                    {
                        Process.Start(psi);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Method for opening context menus
        /// </summary>
        private void FilesListView_MouseUp(object sender, MouseEventArgs e)
        {
            LastMouseButtonUp = e.Button;

            if (LastMouseButtonUp == MouseButtons.Right)
            {
                ListViewItem focusedItem = FilesListView.FocusedItem;
                if (focusedItem is null || !focusedItem.Bounds.Contains(e.Location))
                {
                    DirectoryContextMenu.Show(Cursor.Position);
                }
                else
                {
                    FileContextMenu.Show(Cursor.Position);
                }
            }
        }

        /// <summary>
        /// Method for moving up folders
        /// </summary>
        private void BackButton_Click(object sender, EventArgs e)
        {
            ChangeDirectory(Path.GetDirectoryName(CurrentPath));
        }

        /// <summary>
        /// Method of pressing the "Copy" button of the context menu
        /// </summary>
        private void ContextMenuCopy_Click(object sender, EventArgs e)
        {
            StringCollection paths = new();
            
            foreach (ListViewItem item in FilesListView.SelectedItems)
            {
                paths.Add(Path.Combine(CurrentPath, item.Text));
            }

            DataObject data = new();
            data.SetFileDropList(paths);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);
        }

        /// <summary>
        /// Method of pressing the "Move" button of the context menu
        /// </summary>
        private void ContextMenuMove_Click(object sender, EventArgs e)
        {
            StringCollection paths = new();

            foreach (ListViewItem item in FilesListView.SelectedItems)
            {
                paths.Add(Path.Combine(CurrentPath, item.Text));
            }

            MemoryStream dropEffect = new();
            dropEffect.Write(BitConverter.GetBytes((int)DragDropEffects.Move), 0, 4);

            DataObject data = new();
            data.SetFileDropList(paths);
            data.SetData("Preferred DropEffect", dropEffect);

            Clipboard.Clear();
            Clipboard.SetDataObject(data, true);
        }

        /// <summary>
        /// Method of clicking the "Rename" button of the context menu
        /// </summary>
        private void ContextMenuRename_Click(object sender, EventArgs e)
        {
            string name = ShowPromptDialog("Enter a new name", "Rename");
            
            if (name is null)
            {
                return;
            }
            if (name == string.Empty)
            {
                MessageBox.Show("The file/directory must have a name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int index = 0;
            foreach (ListViewItem item in FilesListView.SelectedItems)
            {
                string path = Path.Combine(CurrentPath, item.Text);
                string newName = name + (index == 0 ? string.Empty : $" - ({index})");
                if (File.Exists(Path.Combine(CurrentPath, newName)) || Directory.Exists(Path.Combine(CurrentPath, newName)))
                {
                    MessageBox.Show($"A file with the same name already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    if (FileSys.IsDirectory(path))
                    {
                        FileSystem.RenameDirectory(path, newName);
                    }
                    else
                    {
                        FileSystem.RenameFile(path, newName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                index++;
            }
        }

        /// <summary>
        /// Method of pressing the "Delete" button of the context menu
        /// </summary>
        private void ContextMenuDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in FilesListView.SelectedItems)
            {
                if ((DataType)item.Tag == DataType.File)
                {
                    FileSystem.DeleteFile(Path.Combine(CurrentPath, item.Text), UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
                else
                {
                    FileSystem.DeleteDirectory(Path.Combine(CurrentPath, item.Text), UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                }
            }
            UpdateFilesListView();
        }

        /// <summary>
        /// Method of pressing the "Insert" button of the context menu
        /// </summary>
        private void ContextMenuPaste_Click(object sender, EventArgs e)
        {
            StringCollection fileDropList = Clipboard.GetFileDropList();
            if (fileDropList is null || fileDropList.Count == 0)
            {
                return;
            }

            bool moveFiles = false;

            object dataDropEffect = Clipboard.GetData("Preferred DropEffect");
            if (dataDropEffect is not null)
            {
                MemoryStream dropEffect = (MemoryStream)dataDropEffect;
                byte[] moveEffect = new byte[4];
                dropEffect.Read(moveEffect, 0, moveEffect.Length);
                DragDropEffects dragDropEffects = (DragDropEffects)BitConverter.ToInt32(moveEffect, 0);
                moveFiles = dragDropEffects.HasFlag(DragDropEffects.Move);
            }

            string destination = CurrentPath;
            _ = Task.Run(() => CopyOrMoveFiles(fileDropList, destination, moveFiles));
        }

        /// <summary>
        /// Method for clicking the "Create file" button of the context menu
        /// </summary>
        private void ContextMenuCreateFile_Click(object sender, EventArgs e)
        {
            string name = ShowPromptDialog("Enter filename", "Create file");
            if (name is null)
            {
                return;
            }
            string fullName = Path.Combine(CurrentPath, name);
            if (name == string.Empty)
            {
                MessageBox.Show("The file must be named", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (File.Exists(fullName) || Directory.Exists(fullName))
            {
                MessageBox.Show("A file with the same name already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                File.Create(fullName).Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Method for clicking the "Create Folder" button of the context menu
        /// </summary>
        private void ContextMenuCreateDirectory_Click(object sender, EventArgs e)
        {
            string name = ShowPromptDialog("Enter folder name", "Create a folder");
            if (name is null)
            {
                return;
            }
            string fullName = Path.Combine(CurrentPath, name);
            if (name == string.Empty)
            {
                MessageBox.Show("The folder must have a name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (File.Exists(fullName) || Directory.Exists(fullName))
            {
                MessageBox.Show("A folder with the same name already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Directory.CreateDirectory(fullName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Method for moving or copying files and folders
        /// </summary>
        private void CopyOrMoveFiles(StringCollection paths, string destination, bool isMove)
        {
            foreach (string path in paths)
            {
                if (FileSys.IsDirectory(path))
                {
                    if (isMove)
                    {
                        FileSystem.MoveDirectory(path, Path.Combine(destination, Path.GetFileName(path)), UIOption.AllDialogs, UICancelOption.DoNothing);
                    }
                    else
                    {
                        FileSystem.CopyDirectory(path, Path.Combine(destination, Path.GetFileName(path)), UIOption.AllDialogs, UICancelOption.DoNothing);
                    }
                }
                else
                {
                    if (isMove)
                    {
                        FileSystem.MoveFile(path, Path.Combine(destination, Path.GetFileName(path)), UIOption.AllDialogs, UICancelOption.DoNothing);
                    }
                    else
                    {
                        FileSystem.CopyFile(path, Path.Combine(destination, Path.GetFileName(path)), UIOption.AllDialogs, UICancelOption.DoNothing);
                    }
                }
            }
        }

        /// <summary>
        /// Method for opening a dialog with an input field
        /// </summary>
        private static string ShowPromptDialog(string text, string caption)
        {
            Form prompt = new()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new() { Left = 50, Top = 20, Text = text, Width = 400 };
            TextBox textBox = new() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new() { Text = "OK", Left = 350, Width = 100, Top = 80, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
        }

        /// <summary>
        /// Method for opening the file manager in new window
        /// </summary>
        private void ShowInNewWindowButton_Click(object sender, EventArgs e)
        {
            if (FindForm() is FileManagerForm form && form.FMCount > 1)
            {
                form.RemoveFM(this);
                new FileManagerForm(this).Show();
            } 
        }

        /// <summary>
        /// Method for removing the file manager
        /// </summary>
        private void Remove_Click(object sender, EventArgs e)
        {
            if (FindForm() is FileManagerForm form)
            {
                form.RemoveFM(this);
            }
        }
    }

    public static class FileIconsIndexes
    {
        public const int Directory = 0;
        public const int File = 1;
    }

    [Flags]
    public enum DragDropEffects
    {
        Scroll = int.MinValue,
        All = -2147483645,
        None = 0,
        Copy = 1,
        Move = 2,
        Link = 4
    }
}
