using FluxorExperiments.ClassLibrary.KeyDownEvent;
using System.Collections.Immutable;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

public record PlainTextEditorCharacterOnClickAction(int RowIndex, int PlainTextTokenKeyIndex, int CharacterIndex);