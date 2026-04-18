using KeePass;
using KeePass.App;
using KeePass.Forms;
using KeePass.Plugins;
using KeePass.Util;
using KeePass.Util.Spr;
using KeePassLib.Utility;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ToggleableTCATO
{
	public sealed class ToggleableTCATOExt : Plugin
	{
		private IPluginHost host = null;
		private const string tcatoTruePlaceholder = "{TCATO:true}";
		private const string tcatoFalsePlaceholder = "{TCATO:false}";
		private readonly Regex regex = new Regex("({TCATO:false}|{TCATO:true})", RegexOptions.IgnoreCase);
		private string sequence = "";
		public override string UpdateUrl
		{
			get
			{
				return "https://raw.githubusercontent.com/CarrieForle/ToggleableTCATOKeePass/refs/heads/main/update.txt";
			}
		}

		public override bool Initialize(IPluginHost host)
		{
			if (host == null)
			{
				return false;
			}

			this.host = host;

			AutoType.FilterSend += OnFilterSend;
			SprEngine.FilterPlaceholderHints.Add(tcatoTruePlaceholder);
			SprEngine.FilterPlaceholderHints.Add(tcatoFalsePlaceholder);

			return true;
		}

		public override void Terminate()
		{
			AutoType.FilterSend -= OnFilterSend;
		}

		private void OnFilterSend(object sender, AutoTypeEventArgs eventArgs)
		{
			var subsequences = regex.Split(eventArgs.Sequence);

			System.Diagnostics.Debug.WriteLine("OnFilterSendPre");

			if (subsequences.Length <= 1)
			{
				return;
			}

			sequence = eventArgs.Sequence;
			eventArgs.Sequence = "";
			bool isPlaceHolder = false;
			bool isObfuscated = eventArgs.SendObfuscated;
			Exception ex = null;

			System.Diagnostics.Debug.WriteLine(string.Join("\n", subsequences));

			foreach (var subsequence in subsequences)
			{
				if (subsequence != "")
				{
					if (isPlaceHolder)
					{
						isObfuscated = subsequence.Equals(tcatoTruePlaceholder, StringComparison.OrdinalIgnoreCase);
					}
					else
					{
						try
						{
							SendInputEx.SendKeysWait(subsequence, isObfuscated);
						}
						catch (Exception x)
						{
							ex = x;
							break;
						}
					}
				}

				isPlaceHolder = !isPlaceHolder;
			}

			eventArgs.Sequence = regex.Replace(sequence, "");
			RaiseSendPost(eventArgs);
			eventArgs.Sequence = "";

			if (ex != null)
			{
				try
				{
					MainForm mainForm = Program.MainForm;

					if (mainForm != null)
					{
						mainForm.EnsureVisibleForegroundWindow(false, false);
					}
				}
				catch
				{
				}

				string text = AppPolicy.Current.UnhidePasswords ? sequence : null;
				MessageService.ShowWarning(new object[] { text, ex });
			}
		}

		private void RaiseSendPost(AutoTypeEventArgs eventArgs)
		{
			var eventDelegate = (MulticastDelegate)typeof(AutoType).GetField("SendPost", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(null);

			if (eventDelegate != null)
			{
				eventDelegate.DynamicInvoke(null, eventArgs);
			}
		}
	}
}
