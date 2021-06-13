using Microsoft.Win32;

namespace Test
{
    public static class DialogService
    {
        static string path = null;
        public static string SaveFile(string format)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = $"{format} files|*.{format}";

            if (saveFileDialog1.ShowDialog() == true)
            {
                path = saveFileDialog1.FileName;
            }

            return path;
        }

        public static string OpenFile(string format)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = $"{format} files|*.{format}";

            if (openFileDialog1.ShowDialog() == true)
            {
                path = openFileDialog1.FileName;
            }

            return path;
        }
    }
}
