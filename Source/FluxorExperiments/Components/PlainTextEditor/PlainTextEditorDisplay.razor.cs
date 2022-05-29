using Fluxor;
using Fluxor.Blazor.Web.Components;
using FluxorExperiments.Store.PlainTextEditor;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.Components.PlainTextEditor;

public partial class PlainTextEditorDisplay : FluxorComponent
{
	[Inject]
	private IState<PlainTextEditorState> PlainTextEditorState { get; set; } = null!;
}