using FluxorExperiments.ClassLibrary.FeatureStateContainer;
using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.RazorClassLibrary.PlainTextEditor;

public partial class PlainTextEditorLineNumberDisplay : ComponentBase
{
	[CascadingParameter(Name=nameof(CharacterDisplay.RowIndex))]
	public int RowIndex { get; set; }
	[CascadingParameter(Name = nameof(LargestLineNumberStringCount))]
	public int LargestLineNumberStringCount { get; set; }
	
	[Parameter, EditorRequired]
	public int RowRerenderCount { get; set; }
	[Parameter, EditorRequired]
	public SequenceKeyRecord RowSequenceKey { get; set; }

	private int LineNumber => RowIndex + 1;

	private int LineNumberPadding => LargestLineNumberStringCount - (int)Math.Log10(LineNumber);
}