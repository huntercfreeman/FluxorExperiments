namespace FluxorExperiments.ClassLibrary.KeyDownEvent;

public record KeyDownEventRecord(string? Key,
	string? Code,
	bool CtrlWasPressed,
	bool ShiftWasPressed,
	bool AltWasPressed)
{
	public static KeyDownEventRecord CloneWithoutCtrlModifier(KeyDownEventRecord onKeyDownEventArgs)
	{
		return onKeyDownEventArgs with
		{
			CtrlWasPressed = false
		};
	}
}