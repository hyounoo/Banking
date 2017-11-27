'use strict'
app.controller('HomeController', ['$scope', '$http', '$filter', '$timeout', '$interval', function ($scope, $http, $filter, $timeout, $interval) {
    $scope.model = {
        clients: [],
        connectedClients: [],
        currencies: [],
        isRun: undefined
    };

    $scope.fn = {};

    $scope.fn.getCurrencyInfo = function () {
        $http({
            url: rootPath + 'api/currencies',
            method: "GET"
        }).then(function (response) {
            if (response.status == 200) {
                $scope.model.currencies = response.data;
            } else {
                alert(response.statusText);
            }
        });
    }
    
    $scope.fn.GetClients = function () {
        $http({
            url: rootPath + 'api/clients',
            method: "GET"
        }).then(function (response) {
            if (response.status == 200) {
                // Get all clients.
                $scope.model.clients = response.data;
            } else {
                alert(response.statusText);
            }
        });
    }

    $scope.fn.OpenClients = function () {
        if ($scope.model.connectedClients.length == 0) {
            angular.forEach($scope.model.clients, function (value, key) {
                // Limit windows for at most 3 at a time.
                if (key < 3) {
                    window.open('/Admin/Clients/Details/' + value.Id);
                }
            });

            // Give it a good 5secs delay to start a demo.(wait until new windows opened).
            $timeout(function () { $scope.fn.GeConnectedClients(); }, 3000);
        }
    }

    $scope.fn.StartDemo = function () {
        if (angular.isDefined($scope.model.isRun)) return;

        $scope.model.isRun = $interval(function () {
            $scope.fn.ProcessRandomFunctions();
            var elem = document.getElementById('message');
            elem.scrollTop = elem.scrollHeight;
        }, 1000);        
    }

    $scope.fn.StopDemo = function () {
        if (angular.isDefined($scope.model.isRun)) {
            $interval.cancel($scope.model.isRun);
            $scope.model.isRun = undefined;
        }
    }

    $scope.fn.GetRandomClient = function () {
        var client = $scope.model.clients[Math.floor(Math.random() * $scope.model.clients.length)];
        return client;
    }

    $scope.fn.GetRandomObject = function (list) {
        return list[Math.floor(Math.random() * list.length)];
    }

    $scope.fn.GeConnectedClients = function () {
        // Get connected clients.
        bank.server.getConnectedClients();
    }

    $scope.fn.ProcessRandomFunctions = function () {        

        // Executes random functions.
        var func = $scope.fn.GetRandomObject(['$scope.fn.balance', '$scope.fn.depositRandom', '$scope.fn.withdrawRandom', '$scope.fn.transfer']);

        eval(func + '()');
    }

    $scope.fn.balance = function () {
        var client = $scope.fn.GetRandomObject($scope.model.clients);
        var account = $scope.fn.GetRandomObject(client.Accounts);
        $http({
            url: rootPath + 'api/accounts/balance',
            method: "GET",
            params: {
                accountNumber: account.AccountNumber
            }
        }).then(function (response) {
            var data = response.data;
            if (data.Successful == true) {

                $('#messages').append('<li><strong> Balance of account "' + account.Title + '": ' + account.Balance + '</strong></li>');

            } else {
                alert(data.Message);
            }
        });
    }

    $scope.fn.depositRandom = function () {
        var client = $scope.fn.GetRandomObject($scope.model.clients);
        var account = $scope.fn.GetRandomObject(client.Accounts);
        var amount = Math.floor((Math.random() * 1000000) + 1);
        var currency = $scope.fn.GetRandomObject($scope.model.currencies);
        $scope.fn.deposit(account, amount);
    }

    $scope.fn.deposit = function (account, amount, sendingAccount, currencyCode){        
        var currency = $scope.fn.GetRandomObject($scope.model.currencies);
        var code = currencyCode != null ? currencyCode : currency.CurrencyCode;

        $http({
            url: rootPath + 'api/accounts/deposit',
            method: "POST",
            params: {
                accountNumber: account.AccountNumber,
                amount: amount,
                currency: currencyCode
            }
        }).then(function (response) {
            var data = response.data;
            if (data.Successful == true) {

                $('#messages').append('<li><strong> Deposit to "' + account.Title + '" for ' + currencyCode + ' ' + amount + '.</strong></li>');

                if (sendingAccount != null) {
                    bank.server.updateClient(account.AccountNumber, sendingAccount.AccountNumber, amount, currencyCode);
                }

            } else {
                alert(data.Message);
            }
        });
    }

    $scope.fn.withdrawRandom = function () {
        var client = $scope.fn.GetRandomObject($scope.model.clients);
        var account = $scope.fn.GetRandomObject(client.Accounts);
        var amount = Math.floor((Math.random() * 1000000) + 1);
        var currency = $scope.fn.GetRandomObject($scope.model.currencies);
        $scope.fn.withdraw(account, amount);
    }

    $scope.fn.withdraw = function (account, amount, isTransfer) {
        var currency = $scope.fn.GetRandomObject($scope.model.currencies);
        var succeed = false;
        $http({
            url: rootPath + 'api/accounts/withdraw',
            method: "POST",
            params: {
                accountNumber: account.AccountNumber,
                amount: amount,
                currency: currency.CurrencyCode
            }
        }).then(function (response) {
            var data = response.data;
            if (data.Successful == true) {
                                
                $('#messages').append('<li><strong> Withdraw from "' + account.Title + '" for ' + currency.CurrencyCode + ' ' + amount + '.</strong></li>');
                succeed = true;
            } else {
                $('#messages').append('<li><strong> Error message of withdraw from "' + account.Title + '" for ' + currency.CurrencyCode + ' ' + amount + ': ' + data.Message + '.</strong></li>');                
            }
        }).then(function (response) {
            if (isTransfer && succeed) {
                var recievingClient = $scope.fn.GetRandomObject($scope.model.clients);
                var recievingAccount = $scope.fn.GetRandomObject(recievingClient.Accounts);

                $scope.fn.deposit(recievingAccount, amount, account, currency.CurrencyCode);
            }
        });
    }

    $scope.fn.transfer = function () {
        var sendingClient = $scope.fn.GetRandomObject($scope.model.clients);
        var sendingAccount = $scope.fn.GetRandomObject(sendingClient.Accounts);
        var amount = Math.floor((Math.random() * 1000000) + 1);
                
        $('#messages').append('<li><strong> Preparing transfer...</strong></li>');
        $scope.fn.withdraw(sendingAccount, amount, true);
    }

    angular.element(document).ready(function () {

        $scope.fn.getCurrencyInfo();
        $scope.fn.GetClients();

        // Start the connection.
        $.connection.hub.start();

        // Signal Client functions

        // Client is connected.
        bank.client.Connected = function () {
            bank.server.logIn(0);
        }

        // Declares message function when login succeed.
        bank.client.logedIn = function (connectionId) {
            $scope.model.connectionId = connectionId;
            $('#messages').append('<li><strong> Your connection ID:' + $scope.model.connectionId + '</strong></li>');
            
            bank.server.getConnectedClients();           
        };

        bank.client.ConnectedClients = function (clients) {
            $scope.model.connectedClients = clients;
        }

        bank.client.logedOut = function () {
            $('#messages').append('<li><strong> You are loged out from server.</strong></li>');
        };
    });
}]);