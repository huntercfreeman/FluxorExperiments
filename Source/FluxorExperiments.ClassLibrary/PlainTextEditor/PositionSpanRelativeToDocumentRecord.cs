﻿namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record struct PositionSpanRelativeToDocumentRecord(int InclusiveStartingDocumentIndex, 
		int ExclusiveEndingDocumentIndex);