using FluxorExperiments.ClassLibrary.Store.PlainTextEditor;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.RazorClassLibrary.PlainTextEditor;

public partial class PlainTextEditorDebugDisplay : ComponentBase
{
	[Parameter, EditorRequired]
	public PlainTextEditorState PlainTextEditorState { get; set; } = null!;
}