namespace FluxorExperiments.ClassLibrary.PlainTextEditor;


/// <summary>
/// This allows the selection to be relative to the document
/// and span multiple rows (lines)
/// </summary>
public record struct SelectionSpanRecord
{
	public SelectionSpanRecord(int inclusiveStartingColumnIndexRelativeToDocument,
		int offsetDisplacement,
		SelectionDirectionBinding initialDirectionBinding)
	{
		InclusiveStartingColumnIndexRelativeToDocument = inclusiveStartingColumnIndexRelativeToDocument;
		OffsetDisplacement = offsetDisplacement;
		InitialDirectionBinding = initialDirectionBinding;
	}
	
	public SelectionSpanRecord(SelectionSpanRecord otherSelectionSpanRecord)
	{
		InclusiveStartingColumnIndexRelativeToDocument = otherSelectionSpanRecord.InclusiveStartingColumnIndexRelativeToDocument;
		OffsetDisplacement = otherSelectionSpanRecord.OffsetDisplacement;
		InitialDirectionBinding = otherSelectionSpanRecord.InitialDirectionBinding;
	}
	
	/// <summary>
	/// InclusiveStartingDocumentTextIndex is the total of the
	/// lengths of all previous rows plus the column in the current row
	/// and is the first column to highlight.
	/// </summary>
	public int InclusiveStartingColumnIndexRelativeToDocument { get; init; }
	/// <summary>
	/// A positive displacement is to say from the marker
	/// highlight additionally the next {x} column
	/// 
	/// A negative displacement is to say from the marker
	/// highlight additionally the previous {x} column
	/// </summary>
	public int OffsetDisplacement { get; init; }

	/// <summary>
	/// Upon instantiation of the SelectionSpanRecord in what direction did the
	/// offset displacement initially go (was it initially {Left:Negative} or
	/// {Right:Positive})
	/// </summary>
	public SelectionDirectionBinding InitialDirectionBinding { get; init; }
}