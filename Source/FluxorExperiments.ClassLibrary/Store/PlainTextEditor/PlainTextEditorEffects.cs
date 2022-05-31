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
	public async Task HandleKeyDownEventAction(KeyDownEventAction keyDownEventAction,
		IDispatcher dispatcher)
	{
		var handledCtrlKeyAction = keyDownEventAction.KeyDownEventRecord.CtrlWasPressed;
		
		if (keyDownEventAction.KeyDownEventRecord.CtrlWasPressed)
		{
			if (keyDownEventAction.KeyDownEventRecord.Key == "v")
			{
				var clipboardContents = await _clipboardProvider.ReadClipboard();
				
				dispatcher.Dispatch(new BulkPlainTextEditorTextInsertAction(clipboardContents));
			}
			else if (keyDownEventAction.KeyDownEventRecord.Key == "c")
			{
				dispatcher.Dispatch(new PlainTextEditorCopyAction(_clipboardProvider));
			}
			else
			{
				handledCtrlKeyAction = false;
			}
		}
		
		if (!handledCtrlKeyAction)
		{
			dispatcher
				.Dispatch(new PlainTextEditorKeyDownEventAction(keyDownEventAction.KeyDownEventRecord));
		}
	}
}