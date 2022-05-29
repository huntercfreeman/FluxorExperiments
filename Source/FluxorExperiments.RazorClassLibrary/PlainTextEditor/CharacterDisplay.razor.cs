using Microsoft.AspNetCore.Components;

namespace FluxorExperiments.RazorClassLibrary.PlainTextEditor;

public partial class CharacterDisplay : ComponentBase
{
	[Parameter, EditorRequired]
	public bool ShouldDisplayCursor { get; set; }
	/// <summary>
	/// Some html characters look like: {AMPERSAND_CHARACTER}nbsp; for non-breaking-space for example.
	///
	/// Therefore a string type is needed for the parameter.
	/// </summary>
	[Parameter, EditorRequired]
	public string Character { get; set; }
}