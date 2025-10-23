# WakeOnLAN

A cross-platform .NET MAUI application for sending Wake-on-LAN (WoL) magic packets to wake up computers on your network.

## Features

- Send Wake-on-LAN magic packets to wake up computers remotely
- Support for multiple MAC address formats (XX:XX:XX:XX:XX:XX, XX-XX-XX-XX-XX-XX, XXXXXXXXXXXX)
- Customizable UDP port (default: 9)
- Real-time MAC address validation
- Cross-platform support for Windows, macOS, and Android

## Supported Platforms

- **Windows 10/11** (version 10.0.17763.0 or higher)
- **macOS** (via Mac Catalyst, version 14.0 or higher)
- **Android** (API level 21 or higher)

## Requirements

- .NET 9.0 SDK or later
- Visual Studio 2022 (17.8 or later) with .NET MAUI workload installed
  - For Windows development: Windows App SDK
  - For macOS development: Xcode
  - For Android development: Android SDK

## Building the Application

### Install .NET MAUI Workload

```bash
# For Windows
dotnet workload install maui-windows

# For macOS
dotnet workload install maui-maccatalyst

# For Android
dotnet workload install maui-android
```

### Build from Command Line

```bash
# Build for all platforms
dotnet build

# Build for specific platform
dotnet build -f net9.0-android
dotnet build -f net9.0-maccatalyst
dotnet build -f net9.0-windows10.0.19041.0
```

### Build with Visual Studio

1. Open `WakeOnLAN.sln` in Visual Studio 2022
2. Select your target platform from the dropdown
3. Press F5 to build and run

## How to Use

1. Launch the WakeOnLAN application
2. Enter the MAC address of the computer you want to wake up
   - Supported formats: `00:11:22:33:44:55`, `00-11-22-33-44-55`, or `001122334455`
3. Optionally, change the UDP port (default is 9)
4. Click "Send Wake-on-LAN Packet"
5. The application will send a magic packet to broadcast address

## Prerequisites for Target Computer

For Wake-on-LAN to work, the target computer must:

1. Have Wake-on-LAN enabled in BIOS/UEFI settings
2. Have Wake-on-LAN enabled in network adapter settings
3. Be connected to the network via Ethernet (most WiFi adapters don't support WoL)
4. Be on the same local network or reachable via directed broadcast

## How Wake-on-LAN Works

Wake-on-LAN works by sending a special "magic packet" to the broadcast address of the network. The magic packet contains:
- 6 bytes of 0xFF (255 decimal)
- 16 repetitions of the target computer's MAC address

When a computer with WoL enabled receives this packet, it powers on.

## Project Structure

```
WakeOnLAN/
├── Services/
│   └── WakeOnLanService.cs      # Core WoL functionality
├── MainPage.xaml                 # UI definition
├── MainPage.xaml.cs              # UI code-behind
├── App.xaml                      # Application resources
├── AppShell.xaml                 # Shell navigation
├── MauiProgram.cs                # App initialization
├── Platforms/                    # Platform-specific code
│   ├── Android/
│   ├── MacCatalyst/
│   └── Windows/
└── Resources/                    # App resources (fonts, images, etc.)
```

## License

This project is open source.

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.