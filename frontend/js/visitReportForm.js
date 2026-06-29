const visitReportForm = document.getElementById("visitReportForm");
const formMessage = document.getElementById("formMessage");

function showFormMessage(message, type) {
    formMessage.innerHTML = '<div class="alert alert-' + type + '">' + message + "</div>";
}

function getFormValue(id) {
    return document.getElementById(id).value.trim();
}

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