namespace FluxorExperiments.ClassLibrary.KeyDownEvent;

public record struct KeyDownEventRecord(string Key, //refernce type on struct is okay if it is readonly apparently
	string Code,
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