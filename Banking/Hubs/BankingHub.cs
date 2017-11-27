using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Banking.Models;
using System.Threading.Tasks;

namespace Banking.Hubs
{
    public class BankingHub : Hub
    {
        private readonly Bank bank;

        public BankingHub() : this(Bank.Instance) { }

        public BankingHub(Bank bank)
        {
            this.bank = bank;
        }

        public void LogIn(int id)
        {
            // Store connectionId in-memory to manage connected clients
            var connectionId = Context.ConnectionId;
            bank.Login(id, connectionId);
        }

        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;

            bank.Connected(connectionId);
            return base.OnConnected();
        }

        public void GetConnectedClients()
        {
            var connectionId = Context.ConnectionId;
            //List<int> connectedClients = Bank.Instance.GetConnectedClients();
            bank.GetConnectedClients(connectionId);
        }
        public void UpdateClient(int senderAccountNumber, int recieverAccountNumber, decimal amount, string currencyCode)
        {
            bank.UpdateClient(senderAccountNumber, recieverAccountNumber, amount, currencyCode);
        }
    }
}