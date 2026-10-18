using NAudio.Wave;

namespace MyHouseMedia
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			Console.Write("Please enter your name: ");
			string name = Console.ReadLine()!.Trim();
			if (string.IsNullOrWhiteSpace(name)) name = "user";
			Console.WriteLine($"Welcome, {name}!");

			using (Form owner = new Form 
			{ 
				TopMost = true,
				ShowInTaskbar = false,
				WindowState = FormWindowState.Minimized,
				Opacity = 0
			})
			{
				using (FolderBrowserDialog fbd = new FolderBrowserDialog())
				{
					fbd.RootFolder = Environment.SpecialFolder.MyComputer;
					fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\";
					owner.Show();
					
					if (fbd.ShowDialog(owner) == DialogResult.OK)
					{
						Console.WriteLine($"Selected folder: {fbd.SelectedPath}\nFiles:");
						string[] files = Directory.GetFiles(fbd.SelectedPath);
						
						foreach (string file in files)
						{
							Console.WriteLine(Path.GetFileName(file));
						}
						
						Console.Write("Paste audio file name with extension: ");
						string chosenFile = Console.ReadLine()!.Trim();
						using AudioFileReader afr = new AudioFileReader(fbd.SelectedPath + $"\\{chosenFile}");
						using WaveOut wo = new WaveOut();
						wo.Init(afr);
						wo.Play();
						Console.WriteLine("Playing... Press any key to stop.");
						Console.ReadKey();
						wo.Stop();
					}
					else
					{
						Console.WriteLine("Folder selection cancelled.");
					}
				}
			}
		}
	}
}
