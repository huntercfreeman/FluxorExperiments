﻿namespace FluxorExperiments.ClassLibrary.PlainTextEditor;

public record PlainTextTokenMetaData(PlainTextTokenBase PlainTextToken,
	PositionSpanRelativeToRowRecord? PositionSpanRelativeToRowRecord,
	PositionSpanRelativeToDocumentRecord? PositionSpanRelativeToDocumentRecord);