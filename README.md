# Installing MentalLog MCP Server for Claude Desktop

This guide will help you connect the local MentalLog server to the Claude desktop application.

> **⚠️ Important Privacy Warning**
> The MentalLog tool itself (including the SQLite database and semantic embedding generation via the built-in ONNX neural network) runs **entirely locally** on your computer. The program does not make any background internet requests.
> **HOWEVER**, when you use this tool inside Claude Desktop, the "consuming" language model (in this case, Claude) will request the text of your entries for processing. This data **will be sent to the provider's servers (Anthropic)** to generate a response. Please keep this in mind when keeping highly confidential logs.

> **📌 Note on Builds**
> This guide covers only the setup of a **standard compiled build** (Release) distributed as a zip archive. If you plan to make custom modifications to the source code and run the project on the fly using `dotnet run`, you will need to set up the environment and absolute paths to the SDK yourself.

---

## Step 1. Preparing the Files

1. Obtain the appropriate zip archive containing the compiled project for your operating system:
   * `MentalLog_OSX_ARM64.zip` (for Mac on Apple Silicon)
   * `MentalLog_OSX_X64.zip` (for Mac on Intel processors)
   * `MentalLog_WIN_64.zip` (for Windows)
   * `MentalLog_linux_64.zip` (for Linux)
2. **Extract the contents of the archive** to a safe location on your drive where it won't be accidentally deleted. You can name the extracted folder whatever you like (for example, `C:\MentalLog\` for Windows or `/home/YourUsername/MentalLog/` for Mac/Linux). 
   *⚠️ Make sure you extract **all** files from the archive. For the server to work, it is critical that the accompanying libraries (e.g., the `onnxruntime` native libraries and other `.dll` files) are located next to the executable file.*
3. **For macOS / Linux only:** The operating system might drop execution permissions upon extraction. You must open the terminal and grant execution rights:
   ```bash
   chmod +x /path/to/folder/MentalLog/Presentation
   ```

## Step 2. Locating the Configuration File

Claude Desktop reads the MCP server configuration from a special `claude_desktop_config.json` file. Find it on your system (if the file or path doesn't exist, create them manually):

* **Windows:** `%APPDATA%\Claude\claude_desktop_config.json`
  *(Usually this is `C:\Users\YourName\AppData\Roaming\Claude\claude_desktop_config.json`)*
* **macOS:** `~/Library/Application Support/Claude/claude_desktop_config.json`
* **Linux:** `~/.config/Claude/claude_desktop_config.json`
  *(Depending on your installation method, it might also be located in `~/.config/claude-desktop/`)*

## Step 3. Adding the Server to the Configuration

Open the `claude_desktop_config.json` file in any text editor and add the server configuration to the `mcpServers` block.

### 🪟 For Windows
> **Critically important:** In JSON files on Windows, paths must contain double backslashes `\\`.

```json
{
  "mcpServers": {
    "mental-log": {
      "command": "C:\\MentalLog\\Presentation.exe",
      "args": []
    }
  }
}
```

### 🍎 For macOS / 🐧 Linux
> Use full absolute paths. Avoid using the tilde `~` to denote the home directory.

```json
{
  "mcpServers": {
    "mental-log": {
      "command": "/home/YourName/MentalLog/Presentation",
      "args": []
    }
  }
}
```

*(If you already have other servers in the file, simply add `"mental-log"` separated by a comma within the `"mcpServers"` object).*

## Step 4. Restart and Verify

1. **Completely close Claude Desktop.** * On macOS: Press `Cmd + Q`.
   * On Windows/Linux: Make sure the app is completely closed and not running in the background (check the system tray
