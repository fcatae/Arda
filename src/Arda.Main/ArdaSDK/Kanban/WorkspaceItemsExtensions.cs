// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace ArdaSDK.Kanban
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for WorkspaceItems.
    /// </summary>
    public static partial class WorkspaceItemsExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            public static void GetItem(this IWorkspaceItems operations, System.Guid itemId)
            {
                operations.GetItemAsync(itemId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task GetItemAsync(this IWorkspaceItems operations, System.Guid itemId, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.GetItemWithHttpMessagesAsync(itemId, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            /// <param name='newItem'>
            /// </param>
            public static void Edit(this IWorkspaceItems operations, System.Guid itemId, object newItem = default(object))
            {
                operations.EditAsync(itemId, newItem).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            /// <param name='newItem'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task EditAsync(this IWorkspaceItems operations, System.Guid itemId, object newItem = default(object), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.EditWithHttpMessagesAsync(itemId, newItem, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            /// <param name='newItem'>
            /// </param>
            public static void Delete(this IWorkspaceItems operations, System.Guid itemId, object newItem = default(object))
            {
                operations.DeleteAsync(itemId, newItem).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            /// <param name='newItem'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task DeleteAsync(this IWorkspaceItems operations, System.Guid itemId, object newItem = default(object), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.DeleteWithHttpMessagesAsync(itemId, newItem, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            /// <param name='newStatus'>
            /// </param>
            public static void UpdateStatus(this IWorkspaceItems operations, System.Guid itemId, int newStatus)
            {
                operations.UpdateStatusAsync(itemId, newStatus).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='itemId'>
            /// </param>
            /// <param name='newStatus'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task UpdateStatusAsync(this IWorkspaceItems operations, System.Guid itemId, int newStatus, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.UpdateStatusWithHttpMessagesAsync(itemId, newStatus, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

    }
}
