using System.Windows.Forms;

Console.Write("Please enter your name: ");
string name = Console.ReadLine()!.Trim();
if (string.IsNullOrWhiteSpace(name)) name = "user";
Console.WriteLine($"Welcome, {name}!");

using (FolderBrowserDialog fbd = new FolderBrowserDialog())
{
	fbd.Description = "Select the folder with your music:";
	fbd.RootFolder = Environment.SpecialFolder.MyMusic;
	DialogResult dr = fbd.ShowDialog();

	if (dr == DialogResult.OK)
	{
		Console.WriteLine($"Selected folder: {fbd.SelectedPath}");
	}
	else
	{
		Console.WriteLine("Folder selection cancelled.");
	}
}
