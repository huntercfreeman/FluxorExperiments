using Fluxor;
using Fluxor.Blazor.Web.Components;
using FluxorExperiments.ClassLibrary.Store.PlainTextEditor;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.RazorClassLibrary.PlainTextEditor;

public partial class PlainTextEditorDisplay : FluxorComponent
{
	[Inject]
	private IState<PlainTextEditorState> PlainTextEditorState { get; set; } = null!;

	private bool IsDebugEnvironment =>
#if DEBUG
	true;
#else
	false;
#endif
}