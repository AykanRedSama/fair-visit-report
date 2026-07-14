
async function deleteReportFromTable(id) {
    const confirmed = confirm("Do you really want to delete this exported visit report?");

    if (!confirmed) {
        return;
    }

    try {
        await deleteVisitReport(id);
        await loadReports();
    } catch (error) {
        alert(error.message);
    }
}

async function deleteAllExportedReportsFromTable() {
    const confirmed = confirm("Do you really want to delete all exported visit reports?");

    if (!confirmed) {
        return;
    }

    try {
        const result = await deleteAllExportedReports();

        alert(result.deletedCount + " exported visit report(s) deleted.");

        await loadReports();
    } catch (error) {
        alert(error.message);
    }
}
