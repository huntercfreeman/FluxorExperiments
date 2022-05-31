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
		if (keyDownEventAction.KeyDownEventRecord.CtrlWasPressed)
		{
			if (keyDownEventAction.KeyDownEventRecord.Key == "v")
			{
				var clipboardContents = await _clipboardProvider.ReadClipboard();

				foreach (var character in clipboardContents)
				{
					if (character == '\r')
						continue;

					var code = character switch {
						'\t' => KeyboardFacts.WhitespaceKeys.TAB_CODE,
						'\n' => KeyboardFacts.WhitespaceKeys.ENTER_CODE,
						' ' => KeyboardFacts.WhitespaceKeys.SPACE_CODE,
						_ => character.ToString()
					};

					dispatcher.Dispatch(new KeyDownEventAction(new KeyDownEventRecord(character.ToString(),
						code,
						false,
						false,
						false)));
				}
			}
			else if (keyDownEventAction.KeyDownEventRecord.Key == "c")
			{
				dispatcher.Dispatch(new PlainTextEditorCopyAction(_clipboardProvider));
			}
		}
		else
		{
			dispatcher
				.Dispatch(new PlainTextEditorKeyDownEventAction(keyDownEventAction.KeyDownEventRecord));
		}
	}
}