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
                wp.Volume = 0.10f;
                wp.Play();
                Console.WriteLine("Playing...\nPress any key to stop.\nP to pause/play.");

                while (wp.PlaybackState != PlaybackState.Stopped)
                {
                    Thread.Sleep(100);

                    if (Console.KeyAvailable)
                    {
                        char input = char.ToUpper(Console.ReadKey(intercept: true).KeyChar);

                        if (input == 'P' && wp.PlaybackState == PlaybackState.Playing)
                        {
                            wp.Pause();
                            Console.WriteLine("Paused.");
                        }
                        else if (input == 'P' && wp.PlaybackState == PlaybackState.Paused)
                        {
                            wp.Play();
                            Console.WriteLine("Continue playing...");
                        }
                        else
                        {
                            wp.Stop();
                        }
                    }
                }

                wp.Dispose();
                Console.WriteLine("Player stopped.");
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
