using WakeOnLAN.Services;

namespace WakeOnLAN;

public partial class MainPage : ContentPage
{
	private readonly WakeOnLanService _wakeOnLanService;

	public MainPage()
	{
		InitializeComponent();
		_wakeOnLanService = new WakeOnLanService();
	}

	private void OnMacAddressTextChanged(object sender, TextChangedEventArgs e)
	{
		var entry = (Entry)sender;
		var macAddress = entry.Text;

		if (string.IsNullOrWhiteSpace(macAddress))
		{
			ValidationLabel.IsVisible = false;
			SendButton.IsEnabled = false;
			return;
		}

		bool isValid = _wakeOnLanService.IsValidMacAddress(macAddress);

		if (isValid)
		{
			ValidationLabel.IsVisible = false;
			SendButton.IsEnabled = true;
		}
		else
		{
			ValidationLabel.Text = "Invalid MAC address format. Use XX:XX:XX:XX:XX:XX or XX-XX-XX-XX-XX-XX";
			ValidationLabel.IsVisible = true;
			SendButton.IsEnabled = false;
		}
	}

	private async void OnSendClicked(object sender, EventArgs e)
	{
		try
		{
			SendButton.IsEnabled = false;
			StatusLabel.Text = "Sending Wake-on-LAN packet...";
			StatusLabel.TextColor = Colors.Blue;

			var macAddress = MacAddressEntry.Text;
			var portText = PortEntry.Text;
			int port = 9;

			if (!string.IsNullOrWhiteSpace(portText) && int.TryParse(portText, out int parsedPort))
			{
				port = parsedPort;
			}

			bool success = await _wakeOnLanService.SendWakeOnLanAsync(macAddress, port);

			if (success)
			{
				StatusLabel.Text = $"Wake-on-LAN packet sent successfully to {macAddress}!";
				StatusLabel.TextColor = Colors.Green;
			}
		}
		catch (Exception ex)
		{
			StatusLabel.Text = $"Error: {ex.Message}";
			StatusLabel.TextColor = Colors.Red;
		}
		finally
		{
			SendButton.IsEnabled = true;
		}
	}
}
