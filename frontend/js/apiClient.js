const apiBaseUrl = "http://localhost:5245/api/visit-reports";

async function handleResponse(response) {
    if (!response.ok) {
        let errorMessage = "request failed";

        try {
            const errorData = await response.json();
            errorMessage = errorData.error || errorMessage;
        } catch {
            errorMessage = response.statusText || errorMessage;
        }

        throw new Error(errorMessage);
    }

    if (response.status === 204) {
        return null;
    }

    return await response.json();
}

async function createVisitReport(report) {
    const response = await fetch(apiBaseUrl, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(report)
    });

    return await handleResponse(response);
}

async function getVisitReports(options = {}) {
    const params = new URLSearchParams();

    params.set("page", options.page || 1);
    params.set("pageSize", options.pageSize || 25);

    if (options.name) {
        params.set("name", options.name);
    }

    if (options.company) {
        params.set("company", options.company);
    }

    if (options.exported !== undefined && options.exported !== "") {
        params.set("exported", options.exported);
    }

    if (options.sortBy) {
        params.set("sortBy", options.sortBy);
    }

    if (options.sortDirection) {
        params.set("sortDirection", options.sortDirection);
    }

    const url = apiBaseUrl + "?" + params.toString();
    const response = await fetch(url);

    return await handleResponse(response);
}

async function getVisitReportById(id) {
    const response = await fetch(apiBaseUrl + "/" + id);

    return await handleResponse(response);
}

async function updateVisitReport(id, report) {
    const response = await fetch(apiBaseUrl + "/" + id, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(report)
    });

    return await handleResponse(response);
}

async function exportSingleReport(id) {
    const response = await fetch(apiBaseUrl + "/" + id + "/export");

    return await handleResponse(response);
}

async function exportSelectedReports(ids) {
    const response = await fetch(apiBaseUrl + "/export", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            ids: ids
        })
    });

    return await handleResponse(response);
}

async function exportUnexportedReports() {
    const response = await fetch(apiBaseUrl + "/export-unexported", {
        method: "POST"
    });

    return await handleResponse(response);
}

async function deleteVisitReport(id) {
    const response = await fetch(apiBaseUrl + "/" + id, {
        method: "DELETE"
    });

    return await handleResponse(response);
}

async function deleteAllExportedReports() {
    const response = await fetch(apiBaseUrl + "/exported", {
        method: "DELETE"
    });

    return await handleResponse(response);
}