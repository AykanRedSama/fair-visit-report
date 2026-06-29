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

function downloadJsonFile(data) {
    const json = JSON.stringify(data, null, 2);
    const blob = new Blob([json], {
        type: "application/json"
    });

    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");

    link.href = url;
    link.download = createExportFileName();
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    URL.revokeObjectURL(url);
}

async function exportReportFromTable(id) {
    try {
        const result = await exportSingleReport(id);
        downloadJsonFile(result);

        if (typeof loadReports === "function") {
            await loadReports();
        }
    } catch (error) {
        alert(error.message);
    }
}