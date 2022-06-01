using Fluxor;
using FluxorExperiments.ClassLibrary.Clipboard;
using FluxorExperiments.ClassLibrary.Keyboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.Store.KeyDownEvent;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

public class PlainTextEditorEffects
{
	private readonly IClipboardProvider _clipboardProvider;

	public PlainTextEditorEffects(IClipboardProvider clipboardProvider)
	{
		_clipboardProvider = clipboardProvider;
	}

	[EffectMethod]
	public async Task HandleKeyDownEventAction(CopyEventAction copyEventAction,
		IDispatcher dispatcher)
	{
		dispatcher.Dispatch(new PlainTextEditorCopyAction(_clipboardProvider));
	}
	
	[EffectMethod]
	public async Task HandleKeyDownEventAction(PasteEventAction pasteEventAction,
		IDispatcher dispatcher)
	{
		var clipboardContents = await _clipboardProvider.ReadClipboard();
				
		dispatcher.Dispatch(new BulkPlainTextEditorTextInsertAction(clipboardContents));
	}
}