using NAudio.Wave;

namespace MyHouseMedia;

class Program
{
	[STAThread]
	static void Main(string[] args)
	{
		Console.Write("Please enter your name: ");
		string name = Console.ReadLine()!.Trim();

		if (string.IsNullOrWhiteSpace(name))
        {
            name = "user";
        }

		Console.WriteLine($"Welcome, {name}!");

        using Form owner = new()
        {
          TopMost = true,
          ShowInTaskbar = false,
          WindowState = FormWindowState.Minimized,
          Opacity = 0
        };

        using FolderBrowserDialog fbd = new();
        fbd.RootFolder = Environment.SpecialFolder.MyComputer;
        fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        owner.Show();
        DialogResult dr = fbd.ShowDialog(owner);
        owner.Dispose();

        if (dr == DialogResult.OK)
        {
            Console.WriteLine($"Selected folder: {fbd.SelectedPath}\nFiles:");
            string[] files = Directory.GetFiles(fbd.SelectedPath);

            foreach (string file in files)
            {
                Console.WriteLine(Path.GetFileName(file));
            }

            Console.Write("Paste audio file name with extension: ");
            string chosenFile = Console.ReadLine()!.Trim();
            
            try
            {
                using AudioFileReader afr = new(Path.Combine(fbd.SelectedPath, chosenFile));
                using WasapiPlayer wp = new WasapiPlayerBuilder().Build();
                wp.Init(afr);
                wp.Play();
                Console.WriteLine("Playing... Press any key to stop.");
                Console.ReadKey();
                wp.Stop();
            }
            catch
            {
                Console.WriteLine("Invalid event.");
                return;
            }
        }
        else
        {
          Console.WriteLine("Folder selection cancelled.");
        }
    }
}
