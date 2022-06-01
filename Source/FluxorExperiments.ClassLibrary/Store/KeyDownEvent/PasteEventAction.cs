﻿using FluxorExperiments.ClassLibrary.Clipboard;
using FluxorExperiments.ClassLibrary.KeyDownEvent;

namespace FluxorExperiments.ClassLibrary.Store.KeyDownEvent;

public record PasteEventAction(KeyDownEventRecord KeyDownEventRecord, IClipboardProvider ClipboardProvider);