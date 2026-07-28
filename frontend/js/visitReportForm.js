const visitReportForm = document.getElementById("visitReportForm");
const formMessage = document.getElementById("formMessage");

/// <summary>
/// Displays a Bootstrap message below the visit report form.
/// </summary>
function showFormMessage(message, type) {
    formMessage.innerHTML = '<div class="alert alert-' + type + '">' + message + "</div>";
}

/// <summary>
/// Returns the trimmed value of a form field by its identifier.
/// </summary>
function getFormValue(id) {
    return document.getElementById(id).value.trim();
}

/// <summary>
/// Creates a visit report from the form data and submits it to the backend API.
/// </summary>
visitReportForm.addEventListener("submit", async function (event) {
    event.preventDefault();

    const report = {
        name: getFormValue("name"),
        position: getFormValue("position") || null,
        company: getFormValue("company") || null,
        mailAddress: getFormValue("mailAddress") || null,
        phoneNumber: getFormValue("phoneNumber") || null,
        reportText: getFormValue("reportText")
    };

    try {
        await createVisitReport(report);
        visitReportForm.reset();
        showFormMessage("Visit report saved successfully.", "success");

        if (typeof loadReports === "function") {
            await loadReports();
        }
    } catch (error) {
        showFormMessage(error.message, "danger");
    }
});