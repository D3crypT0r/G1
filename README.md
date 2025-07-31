## Features ✨

- **User Authentication**: Secure login/signup with SQL Server
- **Modern UI**: Dark theme with neon accents
- **Gameplay**: Classic snake mechanics with score tracking
- **Database**: SQL Server backend for user management
- **Security**: Password hashing with SHA256

## Prerequisites 📋

- Visual Studio 2019/2022
- .NET Framework 4.8
- SQL Server (2016+)
- SQL Server Management Studio (SSMS)

## Installation & Setup 🚀

### 1. Database Setup
```sql
CREATE DATABASE SnakeGameDB;
GO
USE SnakeGameDB;
GO
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL
);
```

### 2. Project Configuration
1. Clone repository:
   ```bash
   git clone https://github.com/yourusername/snake-game.git
   ```
2. Open `SnakeGame.sln` in Visual Studio
3. Update connection string in `App.config`:
   ```xml
   <add name="SnakeDB" 
        connectionString="Data Source=YOUR_SERVER;Initial Catalog=SnakeGameDB;Integrated Security=True"
        providerName="System.Data.SqlClient"/>
   ```

### 3. Build & Run
| Action | Shortcut | Output Location |
|--------|----------|----------------|
| Debug Build | F5 | `bin\Debug\SnakeGame.exe` |
| Release Build | Ctrl+Shift+B | `bin\Release\SnakeGame.exe` |

## Game Controls 🎮
- **Arrow Keys**: Move snake
- **Space**: Pause/resume
- **R**: Restart after game over

## Deployment Options 🚢

### ClickOnce Deployment
1. Right-click project → Publish
2. Select "ClickOnce"
3. Configure:
   - Install Mode: Online/Offline
   - Version: Auto-increment
   - Prerequisites: .NET Framework 4.8

### Standalone EXE
1. Build Release version
2. Copy from `bin\Release`:
   ```
   SnakeGame.exe
   SnakeGame.exe.config
   System.Data.SqlClient.dll
   ```

## Configuration Options ⚙️

### Environment-Specific Settings
Create `App.Release.config`:
```xml
<?xml version="1.0"?>
<configuration xmlns:xdt="http://schemas.microsoft.com/XML-Document-Transform">
  <connectionStrings>
    <add name="SnakeDB" 
         connectionString="Data Source=PROD_SERVER;Initial Catalog=SnakeGameDB;Integrated Security=True"
         xdt:Transform="SetAttributes" xdt:Locator="Match(name)"/>
  </connectionStrings>
</configuration>
```

### Game Settings
Modify `Settings.cs`:
```csharp
public static class Settings
{
    public static int Width { get; } = 20;   // Grid size
    public static int Height { get; } = 20;
    public static int Speed { get; } = 12;   // Game speed (higher = faster)
    public static int Points { get; } = 10;  // Points per food
}
```

## Troubleshooting 🛠️

| Issue | Solution |
|-------|----------|
| SQL Connection Failed | Verify SQL service is running |
| Login Fails | Check password hashing in database |
| Controls Not Responding | Set KeyPreview=true on GameForm |
| Rendering Issues | Ensure pbCanvas has correct size |

## Contributing 🤝
1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a pull request
