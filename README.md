# Modding Assistant

![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![License](https://img.shields.io/badge/License-MIT-green)

A comprehensive mod management tool designed to help gamers organize, track, and manage their mod collections for various games including Skyrim, Oblivion, Fallout, and more.

## Features

- **Mod Database**: Store and organize all your mod information in one place
- **Detailed Mod Tracking**: Keep records of mod names, versions, descriptions, installation instructions, potential issues and etc
- **Update Monitoring**: Track when mods were last updated to keep your collection current
- **Dependency Tracking**: Record mod dependencies to avoid conflicts
- **Loading Files**: Automatically move archives to a convenient folder with the transfer of a file, a convenient name and upload date to the program.
- **Order control**: Keep the order of mods in the program to avoid losing sorting when the main organizer loses data.

## Download
1. Visit the [Releases page](https://github.com/VidyakinIvan/Modding-Assistant/releases)
2. Download the latest version of Modding Assistant
3. Extract the ZIP file to your preferred location
4. Run `Modding-Assistant.exe`

## Usage

### Adding Mods
1. Click the blank row
2. Fill in the mod details:
   - **Name**: The display name of the mod
   - **Version**: Current version of the mod
   - **Install Instructions**: Steps to properly install the mod (via MO2, other utilities or manual)
   - **URL**: Link to the mod's download page
   - **Dependencies**: Other mods this mod requires
   - **ModRawName**: Original filename of the mod archive
   - **Last Updated**: When the mod was last updated
   - **Description**: Detailed description of the mod's functionality
   - **Potential Issues**: Known conflicts or potential problems

### Organizing Mods
- Use context menu to reorder mods
- Sort by any column by clicking the header

## Building from Source

### Requirements
- .NET 9.0 SDK
- Git

### Steps
1. Clone the repository:
```bash
git clone https://github.com/VidyakinIvan/Modding-Assistant.git
```
2. Navigate to the project directory:
```bash
cd Modding-Assistant
```
3. Restore dependencies:
```bash
dotnet restore
```
4. Build the project:
```bash
dotnet build
```
5. Run the application:
```bash
dotnet run
```

## License
This project is licensed under the MIT License - see the LICENSE file for details.
