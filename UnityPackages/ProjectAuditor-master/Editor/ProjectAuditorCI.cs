using Unity.ProjectAuditor.Editor.Auditors;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Unity.ProjectAuditor.Editor.CodeAnalysis;

namespace Unity.ProjectAuditor.Editor {
    public static class ProjectAuditorCI {
        public static void AuditAndExport() {
            ProjectAuditorConfig config =
                AssetDatabase.LoadAssetAtPath<ProjectAuditorConfig>(ProjectAuditor.DefaultAssetPath);
            var projectAuditor = new Unity.ProjectAuditor.Editor.ProjectAuditor(config);

            var projectReport = projectAuditor.Audit();

            var issues = projectReport.GetIssues(IssueCategory.Code);
            ProjectIssue[] codeIssues = issues
                .Where(x => x.GetCustomProperty(CodeProperty.Assembly).Equals("GameAssembly"))
                .Where(x => !IsMuted(x, config) && x.isPerfCriticalContext)
                .ToArray();

            Debug.Log("ProjectAuditorCI found " + codeIssues.Length + " code issues");

            if (codeIssues.Length > 0) {
                foreach (ProjectIssue issue in codeIssues) {
                    Debug.Log($"{issue.description} at {issue.filename}: {issue.line}");
                }
            }
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
}
