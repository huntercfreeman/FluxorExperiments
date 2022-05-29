using FluxorExperiments.Classes.PlainTextEditor;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.Components.PlainTextEditor;

public partial class StartOfRowPlainTextTokenDisplay : ComponentBase
{
	[Parameter, EditorRequired]
	public StartOfRowPlainTextToken StartOfRowPlainTextToken { get; set; } = null!;
	[Parameter, EditorRequired]
	public bool ShouldDisplayCursor { get; set; }
	[Parameter, EditorRequired]
	public int TokenRerenderCount { get; set; }	
}