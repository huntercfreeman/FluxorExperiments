﻿using FluxorExperiments.ClassLibrary.Clipboard;

namespace FluxorExperiments.ClassLibrary.Store.PlainTextEditor;

public record PlainTextEditorCopyAction(IClipboardProvider ClipboardProvider);