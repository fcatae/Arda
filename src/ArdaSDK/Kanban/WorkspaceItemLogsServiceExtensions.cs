// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace ArdaSDK.Kanban
{
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for WorkspaceItemLogsService.
    /// </summary>
    public static partial class WorkspaceItemLogsServiceExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            public static IList<WorkspaceItemLog> GetLogs(this IWorkspaceItemLogsService operations, System.Guid itemId)
            {
                return operations.GetLogsAsync(itemId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<WorkspaceItemLog>> GetLogsAsync(this IWorkspaceItemLogsService operations, System.Guid itemId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetLogsWithHttpMessagesAsync(itemId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            /// <param name='input'>
            /// </param>
            /// <param name='user'>
            /// </param>
            public static void AppendLog(this IWorkspaceItemLogsService operations, System.Guid itemId, InputAppendLog input = default(InputAppendLog), string user = default(string))
            {
                operations.AppendLogAsync(itemId, input, user).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            /// <param name='input'>
            /// </param>
            /// <param name='user'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task AppendLogAsync(this IWorkspaceItemLogsService operations, System.Guid itemId, InputAppendLog input = default(InputAppendLog), string user = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.AppendLogWithHttpMessagesAsync(itemId, input, user, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

    }
}