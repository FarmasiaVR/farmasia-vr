using Unity.ProjectAuditor.Editor;
using Unity.ProjectAuditor.Editor.Auditors;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Unity.ProjectAuditor.Editor.CodeAnalysis;

public static class ProjectAuditorCI {
    public static void AuditAndExport() {
        ProjectAuditorConfig config = AssetDatabase.LoadAssetAtPath<ProjectAuditorConfig>(ProjectAuditor.DefaultAssetPath);
        var projectAuditor = new Unity.ProjectAuditor.Editor.ProjectAuditor(config);

        var projectReport = projectAuditor.Audit();

        var issues = projectReport.GetIssues(IssueCategory.Code);
        ProjectIssue[] codeIssues = issues
            .Where(x => !IsMuted(x, config))
            .Where(x => x.GetCustomProperty(CodeProperty.Assembly).Equals("GameAssembly"))
            .ToArray();

        Debug.Log("ProjectAuditorCI found " + codeIssues.Length + " code issues");
    }

    private static bool IsMuted(ProjectIssue issue, ProjectAuditorConfig config) {
        Rule[] rules = config.m_Rules.ToArray();
        foreach (Rule rule in rules) {
            if (rule.id == issue.descriptor.id) {
                if (rule.filter == issue.GetCallingMethod() || rule.filter == "") {
                    return true;
                }
            }
        }
        return false;
    }
}