namespace FluxorExperiments.ClassLibrary.Clipboard;

public interface IClipboardProvider
{
	public ValueTask<string> ReadClipboard();
	public ValueTask SetClipboard(string value);
}