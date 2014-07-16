using System;

namespace WebClient.ICS.Client.Model
{
	[Flags]
	internal enum Operation
	{
		Subscribe,
		Report,
		Modify,
		ReadWrite,
		Read,
		Grant,
		Enumerate,
		None
	}
}