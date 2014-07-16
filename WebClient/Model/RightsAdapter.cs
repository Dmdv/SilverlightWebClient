using WebClient.ICS.Client.ServiceReference;

namespace WebClient.ICS.Client.Model
{
	// ReSharper disable UnusedMember.Global

	/// <summary>
	/// RightsAdapter.
	/// </summary>
	public class RightsAdapter
	{
		private readonly Permission _permission;
		private Operation _operation;

		public RightsAdapter(Permission permission)
		{
			_permission = permission;
			_operation = permission.Operations.Cast<Operation>();
		}

		public bool Grant
		{
			get { return (_operation & Operation.Grant) > 0; }
			set { if (Grant ^ value) _operation ^= Operation.Grant; UpdateSource(); }
		}

		public bool Report
		{
			get { return (_operation & Operation.Report) > 0; }
			set { if (Report ^ value) _operation ^= Operation.Report; UpdateSource(); }
		}

		public bool Subscribe
		{
			get { return (_operation & Operation.Subscribe) > 0; }
			set { if (Subscribe ^ value) _operation ^= Operation.Subscribe; UpdateSource(); }
		}

		public bool Modify
		{
			get { return (_operation & Operation.Modify) > 0; }
			set { if (Modify ^ value) _operation ^= Operation.Modify; UpdateSource(); }
		}

		public bool ReadWrite
		{
			get { return (_operation & Operation.ReadWrite) > 0; }
			set { if (ReadWrite ^ value) _operation ^= Operation.ReadWrite; UpdateSource(); }
		}

		public bool Read
		{
			get { return (_operation & Operation.Read) > 0; }
			set { if (Read ^ value) _operation ^= Operation.Read; UpdateSource(); }
		}

		public bool Enumerate
		{
			get { return (_operation & Operation.Enumerate) > 0; }
			set { if (Enumerate ^ value) _operation ^= Operation.Enumerate; UpdateSource(); }
		}

		public bool None
		{
			get { return _operation == 0; }
			set { if (None ^ value) _operation ^= Operation.None; UpdateSource(); }
		}

		private void UpdateSource()
		{
			_permission.Operations = _operation.Cast<byte>();
		}
	}
}
