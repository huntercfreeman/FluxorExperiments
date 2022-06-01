using FluxorExperiments.ClassLibrary.FeatureStateContainer;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.RazorClassLibrary.PlainTextEditor;

public partial class PlainTextEditorLineNumberDisplay : ComponentBase
{
	[CascadingParameter(Name=nameof(CharacterDisplay.RowIndex))]
	public int RowIndex { get; set; }
	[CascadingParameter(Name=nameof(LargestLineNumberString))]
	public string LargestLineNumberString { get; set; } = null!;
	
	[Parameter, EditorRequired]
	public int RowRerenderCount { get; set; }
	[Parameter, EditorRequired]
	public SequenceKeyRecord RowSequenceKey { get; set; } = null!;

	private int LineNumber => RowIndex + 1;

	private int LineNumberPadding => LargestLineNumberString.Length - LineNumber.ToString().Length;
}