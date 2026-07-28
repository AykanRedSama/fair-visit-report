/// <summary>
/// Creates a unique JSON export file name using the current date and time.
/// </summary>
function createExportFileName() {
    const now = new Date();
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, "0");
    const day = String(now.getDate()).padStart(2, "0");
    const hours = String(now.getHours()).padStart(2, "0");
    const minutes = String(now.getMinutes()).padStart(2, "0");
    const seconds = String(now.getSeconds()).padStart(2, "0");

    return "visit-reports-export-" + year + month + day + "-" + hours + minutes + seconds + ".json";
}

/// <summary>
/// Creates and downloads a JSON file containing the exported visit report data.
/// </summary>
function downloadJsonFile(data) {
    const json = JSON.stringify(data, null, 2);
    const blob = new Blob([json], { type: "application/json" });
    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");

    link.href = url;
    link.download = createExportFileName();

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    URL.revokeObjectURL(url);
}

/// <summary>
/// Exports and downloads a single visit report by its identifier.
/// </summary>
async function exportReportFromTable(id) {
    try {
        const result = await exportSingleReport(id);
        downloadJsonFile(result);
        await loadReports();
    } catch (error) {
        alert(error.message);
    }
}

/// <summary>
/// Exports and downloads all visit reports selected in the report table.
/// </summary>
async function exportSelectedReportsFromTable() {
    const selectedCheckboxes = document.querySelectorAll(".report-selection-checkbox:checked");

    const ids = Array.from(selectedCheckboxes).map(function (checkbox) {
        return Number(checkbox.value);
    });

    if (ids.length === 0) {
        alert("Please select at least one visit report.");
        return;
    }

    try {
        const result = await exportSelectedReports(ids);
        downloadJsonFile(result);
        await loadReports();
    } catch (error) {
        alert(error.message);
    }
}

/// <summary>
/// Exports and downloads all visit reports that have not been exported yet.
/// </summary>
async function exportAllUnexportedReportsFromTable() {
    try {
        const result = await exportUnexportedReports();

        if (!result.reports || result.reports.length === 0) {
            alert("There are no non-exported visit reports.");
            return;
        }

        downloadJsonFile(result);
        await loadReports();
    } catch (error) {
        alert(error.message);
    }
}