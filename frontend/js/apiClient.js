const apiBaseUrl = "/api/visit-reports";

/// <summary>
/// Handles API responses and throws an error for unsuccessful requests.
/// </summary>
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

/// <summary>
/// Sends a request to create a new visit report.
/// </summary>
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

/// <summary>
/// Returns a filtered, sorted and paginated list of visit reports.
/// </summary>
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

/// <summary>
/// Returns a single visit report by its identifier.
/// </summary>
async function getVisitReportById(id) {
    const response = await fetch(apiBaseUrl + "/" + id);

    return await handleResponse(response);
}

/// <summary>
/// Sends a request to update an existing visit report.
/// </summary>
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

/// <summary>
/// Exports a single visit report by its identifier.
/// </summary>
async function exportSingleReport(id) {
    const response = await fetch(apiBaseUrl + "/" + id + "/export");

    return await handleResponse(response);
}

/// <summary>
/// Exports multiple selected visit reports by their identifiers.
/// </summary>
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

/// <summary>
/// Exports all visit reports that have not been exported yet.
/// </summary>
async function exportUnexportedReports() {
    const response = await fetch(apiBaseUrl + "/export-unexported", {
        method: "POST"
    });

    return await handleResponse(response);
}

/// <summary>
/// Deletes a single exported visit report by its identifier.
/// </summary>
async function deleteVisitReport(id) {
    const response = await fetch(apiBaseUrl + "/" + id, {
        method: "DELETE"
    });

    return await handleResponse(response);
}

/// <summary>
/// Deletes all visit reports that have already been exported.
/// </summary>
async function deleteAllExportedReports() {
    const response = await fetch(apiBaseUrl + "/exported", {
        method: "DELETE"
    });

    return await handleResponse(response);
}
