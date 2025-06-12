# Reporters

Reporters is a C# application designed to track and manage incident reports using a MySQL database. It allows users to submit reports, records relevant data, manages user identities (including codenames), and tracks alerts based on the frequency and timing of reports. The system is intended for environments where monitoring and responding to repeated incidents or behaviors is critical.

## Features

- **Submit Reports:** Input individual or multiple reports into the database.
- **Automatic User Management:** Adds new reporters or reported individuals to the database as needed.
- **Codename Support:** Allows reporters to use codenames for privacy.
- **Alert System:** Detects and tallies alerts when individuals are reported multiple times within a short period.
- **Comprehensive Tracking:** Maintains counts for how often users are reported, including both short-term and long-term metrics.
- **Permission Checks:** Validates user access before outputting alert information.

## Getting Started

### Prerequisites

- .NET (Core or Framework) compatible with your project
- MySQL database server
- MySql.Data library for C#
- EntityFramework (and EntityFramework.SqlServer)
- Google.Protobuf, BouncyCastle, K4os.Compression.LZ4, K4os.Hash.xxHash, and other standard libraries (as referenced in the project)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/ymhorn/Reporters.git
   cd Reporters
   ```

2. **Restore dependencies:**
   - Open the project in Visual Studio or use the .NET CLI to restore NuGet packages.

3. **Configure the database:**
   - Create a MySQL database and set up the necessary tables: `personalinfo`, `personalreports`, and `reports`.
   - Update the `connectionString` variable in your code with your database credentials.

4. **Build the project:**
   - Use Visual Studio or run:
     ```bash
     dotnet build
     ```

5. **Run the application:**
   - Via Visual Studio or:
     ```bash
     dotnet run
     ```

## Usage

- Input a single report or a list of reports, which will be processed and stored in the MySQL database.
- The application checks if the reporter or reported person exists and adds them to the database if not.
- Codename input is supported for anonymizing reporter identity.
- Alerts are automatically calculated if a person is reported three or more times within 15 minutes.
- An admin or authorized user can retrieve current alerts and their counts.

## Example Code

```csharp
// To add a single report
ReportToDB.AddReport(report);

// To add multiple reports
ReportToDB.AddReport(listOfReports);

// To display current alerts (with permission check)
Alerts.AllAlerts();
```

## Contributing

Contributions are welcome! Please fork the repository, make your changes, and submit a pull request. For major changes, open an issue to discuss your ideas.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contact

For questions or suggestions, please contact [ymhorn](https://github.com/ymhorn).
