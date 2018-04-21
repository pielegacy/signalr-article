using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace server.Hubs
{
    /// <summary>
    /// Defines the events associated with the Hub.
    /// </summary>
    public interface IValuesClient
    {
        /// <summary>
        /// This event occurs when a value is posted to the
        /// ValuesController.
        /// </summary>
        /// <param name="value">The value that has been created.</param>
        Task Add(string value);

        /// <summary>
        /// This event occurs when a value is deleted using the
        /// ValuesController.
        /// </summary>
        /// <param name="value">The value that has been deleted</param>
        Task Delete(string value);
    }

    /// <summary>
    /// Used for providing realtime updates for the ValuesController.
    /// </summary>
    public class ValuesHub : Hub<IValuesClient>
    {
        /// <summary>
        /// Notify all users that a value has been added.
        /// </summary>
        /// <param name="value">The new value</param>
        public async Task Add(string value) => await Clients.All.Add(value);

        /// <summary>
        /// Notify all users that a value has been removed.
        /// </summary>
        /// <param name="value">The removed value</param>
        public async Task Delete(string value) => await Clients.All.Delete(value);
    }
}