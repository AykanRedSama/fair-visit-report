
let currentPage = 1;
let currentPageSize = 25;
let totalPages = 1;

const reportsTableBody = document.getElementById("reportsTableBody");
const paginationInfo = document.getElementById("paginationInfo");
const previousPageButton = document.getElementById("previousPageButton");
const nextPageButton = document.getElementById("nextPageButton");
const refreshReportsButton = document.getElementById("refreshReportsButton");
const applyFiltersButton = document.getElementById("applyFiltersButton");
const clearFiltersButton = document.getElementById("clearFiltersButton");

function getFilterOptions() {
    return {
        page: currentPage,
        pageSize: currentPageSize,
        name: document.getElementById("filterName").value.trim(),
        company: document.getElementById("filterCompany").value.trim(),
        exported: document.getElementById("filterExported").value,
        sortBy: document.getElementById("sortBy").value,
        sortDirection: document.getElementById("sortDirection").value
    };
}

function formatDate(value) {
    if (!value) {
        return "";
    }

    return new Date(value).toLocaleString();
}

function renderReports(result) {
    reportsTableBody.innerHTML = "";

    if (!result.items || result.items.length === 0) {
        reportsTableBody.innerHTML = '<tr><td colspan="10" class="text-center">No reports found.</td></tr>';
        paginationInfo.textContent = "Page 1 of 1";
        previousPageButton.disabled = true;
        nextPageButton.disabled = true;
        return;
    }

    result.items.forEach(function (report) {
        const row = document.createElement("tr");
        const exportStatusClass = report.exported ? "status-exported" : "status-not-exported";
        const exportStatusText = report.exported ? "Exported" : "Not Exported";
        const exportButtonDisabled = report.exported ? "disabled" : "";
        const deleteButtonDisabled = report.exported ? "" : "disabled";

        row.innerHTML =
            '<td class="text-center">' +
            '<input type="checkbox" class="form-check-input report-selection-checkbox" value="' + report.id + '">' +
            "</td>" +
            "<td>" + report.id + "</td>" +
            "<td>" + report.name + "</td>" +
            "<td>" + (report.position || "") + "</td>" +
            "<td>" + (report.company || "") + "</td>" +
            "<td>" + (report.mailAddress || "") + "</td>" +
            "<td>" + (report.phoneNumber || "") + "</td>" +
            "<td>" + formatDate(report.createdAt) + "</td>" +
            '<td><span class="' + exportStatusClass + '">' + exportStatusText + "</span></td>" +
            "<td>" +
            '<button class="btn btn-sm btn-outline-primary me-1" onclick="showReportDetails(' + report.id + ')">Details</button>' +
            '<button class="btn btn-sm btn-outline-success me-1" onclick="exportReportFromTable(' + report.id + ')" ' + exportButtonDisabled + ">Export</button>" +
            '<button class="btn btn-sm btn-outline-danger" onclick="deleteReportFromTable(' + report.id + ')" ' + deleteButtonDisabled + ">Delete</button>" +
            "</td>";

        reportsTableBody.appendChild(row);
    });

    currentPage = result.page;
    totalPages = result.totalPages || 1;

    paginationInfo.textContent = "Page " + currentPage + " of " + totalPages;
    previousPageButton.disabled = currentPage <= 1;
    nextPageButton.disabled = currentPage >= totalPages;
}

async function loadReports() {
    try {
        const options = getFilterOptions();
        const result = await getVisitReports(options);
        renderReports(result);
    } catch (error) {
        reportsTableBody.innerHTML =
            '<tr><td colspan="10" class="text-center text-danger">' +
            error.message +
            "</td></tr>";
    }
}

async function showReportDetails(id) {
    try {
        const report = await getVisitReportById(id);

        alert(
            "ID: " + report.id + "\n" +
            "Name: " + report.name + "\n" +
            "Position: " + (report.position || "") + "\n" +
            "Company: " + (report.company || "") + "\n" +
            "Mail Address: " + (report.mailAddress || "") + "\n" +
            "Phone Number: " + (report.phoneNumber || "") + "\n" +
            "Created At: " + formatDate(report.createdAt) + "\n" +
            "Updated At: " + formatDate(report.updatedAt) + "\n" +
            "Exported: " + report.exported + "\n" +
            "Exported At: " + formatDate(report.exportedAt) + "\n\n" +
            "Report Text:\n" + report.reportText
        );
    } catch (error) {
        alert(error.message);
    }
}

refreshReportsButton.addEventListener("click", async function () {
    await loadReports();
});

applyFiltersButton.addEventListener("click", async function () {
    currentPage = 1;
    await loadReports();
});

clearFiltersButton.addEventListener("click", async function () {
    document.getElementById("filterName").value = "";
    document.getElementById("filterCompany").value = "";
    document.getElementById("filterExported").value = "";
    document.getElementById("sortBy").value = "createdAt";
    document.getElementById("sortDirection").value = "desc";

    currentPage = 1;
    await loadReports();
});

previousPageButton.addEventListener("click", async function () {
    if (currentPage > 1) {
        currentPage--;
        await loadReports();
    }
});

nextPageButton.addEventListener("click", async function () {
    if (currentPage < totalPages) {
        currentPage++;
        await loadReports();
    }
});

document.addEventListener("DOMContentLoaded", async function () {
    await loadReports();
});
