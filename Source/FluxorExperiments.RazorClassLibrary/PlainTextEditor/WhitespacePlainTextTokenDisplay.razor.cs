using FluxorExperiments.ClassLibrary.PlainTextEditor;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.RazorClassLibrary.PlainTextEditor;

public partial class WhitespacePlainTextTokenDisplay : ComponentBase
{
	[Parameter, EditorRequired]
	public WhitespacePlainTextToken WhitespacePlainTextToken { get; set; } = null!;
	[Parameter, EditorRequired]
	public bool ShouldDisplayCursor { get; set; }
	[Parameter, EditorRequired]
	public int TokenRerenderCount { get; set; }	
}