using Fluxor;
using FluxorExperiments.ClassLibrary.FeatureStateContainer;
using FluxorExperiments.ClassLibrary.KeyDownEvent;
using FluxorExperiments.ClassLibrary.PlainTextEditor;
using System.Collections.Immutable;
using System.Text;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

[FeatureState]
public partial record PlainTextEditorState 
	: FeatureStateContainerRecord<PlainTextEditorState, PlainTextRowKey, PlainTextRow>;