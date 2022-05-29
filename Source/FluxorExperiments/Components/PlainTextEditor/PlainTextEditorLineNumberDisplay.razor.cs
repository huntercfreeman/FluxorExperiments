using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.Components.PlainTextEditor;

public partial class PlainTextEditorLineNumberDisplay : ComponentBase
{
	[CascadingParameter(Name=nameof(LineNumber))]
	public int LineNumber { get; set; }
	[CascadingParameter(Name=nameof(LargestLineNumberString))]
	public string LargestLineNumberString { get; set; } = null!;
	
	[Parameter, EditorRequired]
	public int RowRerenderCount { get; set; }

	private int LineNumberPadding => LargestLineNumberString.Length - LineNumber.ToString().Length;
}