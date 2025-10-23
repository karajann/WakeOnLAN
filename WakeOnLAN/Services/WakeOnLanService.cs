using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace WakeOnLAN.Services;

public class WakeOnLanService
{
    /// <summary>
    /// Sends a Wake-on-LAN magic packet to the specified MAC address
    /// </summary>
    /// <param name="macAddress">MAC address in format XX:XX:XX:XX:XX:XX or XX-XX-XX-XX-XX-XX</param>
    /// <param name="port">UDP port (default is 9)</param>
    /// <returns>True if packet was sent successfully</returns>
    public async Task<bool> SendWakeOnLanAsync(string macAddress, int port = 9)
    {
        try
        {
            // Validate and normalize MAC address
            var normalizedMac = NormalizeMacAddress(macAddress);
            if (string.IsNullOrEmpty(normalizedMac))
            {
                throw new ArgumentException("Invalid MAC address format. Use XX:XX:XX:XX:XX:XX or XX-XX-XX-XX-XX-XX");
            }

            // Convert MAC address to byte array
            byte[] macBytes = ConvertMacToBytes(normalizedMac);

            // Create magic packet
            byte[] magicPacket = CreateMagicPacket(macBytes);

            // Send the packet
            using (var client = new UdpClient())
            {
                client.EnableBroadcast = true;
                await client.SendAsync(magicPacket, magicPacket.Length, new IPEndPoint(IPAddress.Broadcast, port));
            }

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error sending Wake-on-LAN packet: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Normalizes MAC address to remove separators
    /// </summary>
    private string NormalizeMacAddress(string macAddress)
    {
        if (string.IsNullOrWhiteSpace(macAddress))
            return string.Empty;

        // Remove common separators (: - .)
        var normalized = macAddress.Replace(":", "").Replace("-", "").Replace(".", "").Trim();

        // Validate format (12 hexadecimal characters)
        if (Regex.IsMatch(normalized, @"^[0-9A-Fa-f]{12}$"))
            return normalized;

        return string.Empty;
    }

    /// <summary>
    /// Converts MAC address string to byte array
    /// </summary>
    private byte[] ConvertMacToBytes(string macAddress)
    {
        byte[] macBytes = new byte[6];
        for (int i = 0; i < 6; i++)
        {
            macBytes[i] = Convert.ToByte(macAddress.Substring(i * 2, 2), 16);
        }
        return macBytes;
    }

    /// <summary>
    /// Creates the Wake-on-LAN magic packet
    /// Magic packet consists of:
    /// - 6 bytes of 0xFF
    /// - 16 repetitions of the target MAC address
    /// </summary>
    private byte[] CreateMagicPacket(byte[] macBytes)
    {
        byte[] magicPacket = new byte[102]; // 6 + (6 * 16) = 102 bytes

        // First 6 bytes are 0xFF
        for (int i = 0; i < 6; i++)
        {
            magicPacket[i] = 0xFF;
        }

        // Repeat MAC address 16 times
        for (int i = 1; i <= 16; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                magicPacket[i * 6 + j] = macBytes[j];
            }
        }

        return magicPacket;
    }

    /// <summary>
    /// Validates if a MAC address string is in correct format
    /// </summary>
    public bool IsValidMacAddress(string macAddress)
    {
        if (string.IsNullOrWhiteSpace(macAddress))
            return false;

        // Check common MAC address formats
        var patterns = new[]
        {
            @"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$", // XX:XX:XX:XX:XX:XX or XX-XX-XX-XX-XX-XX
            @"^[0-9A-Fa-f]{12}$" // XXXXXXXXXXXX
        };

        return patterns.Any(pattern => Regex.IsMatch(macAddress, pattern));
    }
}
