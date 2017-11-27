using Banking.Areas.Admin.Controllers;
using Banking.Areas.Admin.ViewModels;
using Banking.Controllers;
using Banking.Hubs;
using Banking.ViewModels;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Banking.Models
{
    public class Bank
    {
        private readonly static Lazy<Bank> instance = new Lazy<Bank>(() => new Bank(GlobalHost.ConnectionManager.GetHubContext<BankingHub>().Clients));

        private readonly ConcurrentDictionary<int, string> connectedClients = new ConcurrentDictionary<int, string>();

        private readonly object updateBankMessagesLock = new object();

        Areas.Admin.Controllers.ClientsController clientController = new Areas.Admin.Controllers.ClientsController();
        Areas.Admin.Controllers.AccountsController accountController = new Areas.Admin.Controllers.AccountsController();

        private Bank(IHubConnectionContext<dynamic> clientHub)
        {
            ClientHub = clientHub;
            connectedClients.Clear();
        }

        public static Bank Instance
        {
            get
            {
                return instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> ClientHub
        {
            get;
            set;
        }

        public List<string> GetConnectionIds()
        {
            return connectedClients.Select(u => u.Value).ToList();
        }

        internal void GetConnectedClients(string connectionId)
        {
            var clients = connectedClients.Where(u => u.Key != 0).Select(u => u.Key).ToList();
            ClientHub.Client(connectionId).ConnectedClients(clients);
        }

        string GetExistingConnectionId(int clientId)
        {
            string existingConnectionId;

            connectedClients.TryGetValue(clientId, out existingConnectionId);

            return existingConnectionId;
        }

        ClientViewModel GetClient(int clientId)
        {
            var client = clientController.SearchClientById(clientId);
            return client;
        }

        AccountViewModel GetAccount(int accountNumber)
        {
            var account = accountController.SearchAccountByAccountNumber(accountNumber);
            return account;
        }

        internal void Login(int clientId, string connectionId)
        {
            var existingConnectionId = GetExistingConnectionId(clientId);

            if (existingConnectionId != null)
            {
                if (existingConnectionId != connectionId)
                {
                    LogOut(existingConnectionId);
                }
                connectedClients.TryUpdate(clientId, connectionId, existingConnectionId);
            }
            else
            {
                connectedClients.TryAdd(clientId, connectionId);
            }

            ClientHub.Client(connectionId).logedIn(connectionId);

        }

        private void LogOut(string existingConnectionId)
        {
            ClientHub.Client(existingConnectionId).logedOut();
        }

        internal void Connected(string connectionId)
        {
            ClientHub.Client(connectionId).Connected(connectionId);
        }

        internal void UpdateClient(int senderAccountNumber, int recieverAccountNumber, decimal amount, string currencyCode)
        {
            var sendingAccount = GetAccount(senderAccountNumber);
            var receivingAccount = GetAccount(recieverAccountNumber);

            var connectionId = GetExistingConnectionId(receivingAccount.ClientId);

            // Call UpdateClient method to update connected client.
            if (connectionId != null)
            {
                var sender = GetClient(sendingAccount.ClientId);
                ClientHub.Client(connectionId).UpdateClient(sender.ClientName, receivingAccount.Title, amount, currencyCode);
            }
        }
    }
}