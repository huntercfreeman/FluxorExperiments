using FluxorExperiments.ClassLibrary.Clipboard;
using Microsoft.JSInterop;

namespace FluxorExperiments.RazorClassLibrary.Clipboard;

public class ClipboardProvider : IClipboardProvider
{
	private readonly IJSRuntime _jsRuntime;

	public ClipboardProvider(IJSRuntime jsRuntime)
	{
		_jsRuntime = jsRuntime;
	}

	public async ValueTask<string> ReadClipboard()
	{
		try
		{
			return await _jsRuntime.InvokeAsync<string>(
				"fluxorExperiments.readClipboard");
		}
		catch (TaskCanceledException)
		{
			return string.Empty;
		}
	}

	public async ValueTask SetClipboard(string value)
	{
		try
		{
			await _jsRuntime.InvokeVoidAsync(
				"fluxorExperiments.setClipboard",
				value);
		}
		catch (TaskCanceledException)
		{
		}
	}
}