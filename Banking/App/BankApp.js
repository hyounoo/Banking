var app = angular.module('BankApp', []),
    // Create a proxy to signalR hub on Web server
    bank = $.connection.bankingHub;
