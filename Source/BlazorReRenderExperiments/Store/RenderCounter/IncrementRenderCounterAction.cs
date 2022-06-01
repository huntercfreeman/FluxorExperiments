﻿using BlazorReRenderExperiments.Records.RenderCounter;

namespace BlazorReRenderExperiments.Store.RenderCounter;

public record IncrementRenderCounterAction(RenderCounterRowRecordKey RenderCounterRowRecordKey,
	RenderCounterRecordKey RenderCounterRecordKey);