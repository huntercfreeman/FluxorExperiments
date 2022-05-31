﻿namespace FluxorExperiments.ClassLibrary.Clipboard;

public interface IClipboardProvider
{
	public Task<string> ReadClipboard();
	public Task SetClipboard(string value);
}