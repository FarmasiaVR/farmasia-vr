using Unity.ProjectAuditor.Editor;
using UnityEngine;

public static class ProjectAuditorCI {
    public static void AuditAndExport() {
        var projectAuditor = new Unity.ProjectAuditor.Editor.ProjectAuditor();
        var projectReport = projectAuditor.Audit();

        var codeIssues = projectReport.GetIssues(IssueCategory.Code);

        Debug.Log("Project Auditor found " + codeIssues.Length + " code issues");
    }
}